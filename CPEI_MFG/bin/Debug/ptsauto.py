import os,time,re,datetime,sys,argparse,subprocess,threading
from subprocess import Popen, PIPE

#Config variables
IPERF="C:\\Users\\production\\Downloads\\iperf-3.1.3-win64\\iperf3.exe"
#IPERF="C:\\Users\\production\\Downloads\\iperf-3.0.11-win64\\iperf3.exe"
DSTIP="192.168.1.22"
PTSCTL="C:\\Program Files (x86)\\Qualcomm Atheros\\Powerline Toolkit\\ptsctl.exe"
PORT="COM2"

TESTTIME="30"
NUM_STREAM="10"
#The range of PTS attenuator is from 4 to 130 dB. 
STARTATT="10"
ENDATT="131"
ATTSTEP="4"
CHANNELLEARNTIME="10"
TS=datetime.datetime.utcnow().strftime("%Y-%m-%d-%H-%M-%S")
LOGFILE = TS + ".log"
CSVFILE = TS + ".csv"


class Command(object):
    def __init__(self, cmd, log):
        #print cmd
        self.cmd = cmd
        self.process = None
        self.log = log
        self.output = ""

    def run(self, timeout):
        def target():
            self.log.write(self.cmd + "\n")
            self.process = subprocess.Popen(self.cmd, stdout=PIPE, stderr=PIPE)
            (output, err) = self.process.communicate()
            self.output += output
            self.log.write(output)
            self.log.write(err)
        
        thread = threading.Thread(target=target)
        thread.start()

        thread.join(timeout)
        if thread.is_alive():
            print 'Terminating process'
            self.log.write('Terminating process' + "\n")
            
            self.process.terminate()
            thread.join()
        if self.process.returncode != 0:
            sys.exit(1)
        return self.output

parser = argparse.ArgumentParser(prog='python ptsauto.py', description='This tool require python and has been tested in Windows 7 and python 2.7.13', epilog="example: python ptsauto.py -s 10 -e 131 -t 4 -l EM-CR")
parser.add_argument("-o", help="log and csc filename, will be appended .log and .csv")
parser.add_argument("-s", help="start attenuation(dB), e.g. 10")
parser.add_argument("-e", help="end attenuation(dB), e.g. 130")
parser.add_argument("-t", help="attenuation step(dB), e.g. 4")
parser.add_argument("-R", "--reverse", action="store_true", help="run in reverse mode (DUT sends, GN receives)")
parser.add_argument("-v", "--version", help="print version", action='version', version="0.1")
args = parser.parse_args()

#retrieve args
if args.o:
	LOGFILE = args.o + ".log"
	CSVFILE = args.o + ".csv"
if args.s:
	STARTATT = args.s
if args.e:
	ENDATT = args.e
if args.t:
	ATTSTEP = args.t

try:
    #open log file
    f_log = open(LOGFILE, "w")
    f_csv = open(CSVFILE, "w")

    #Power on DUT if not
    command = Command(PTSCTL + " -f " + PORT + " -g " + STARTATT + " -n " + STARTATT + " -p " + "1", f_log)
    command.run(timeout=10)
    
    #print "wait 10 seconds fot DUT to boot"
    time.sleep(10)

    ATTENRANGE = range(int(STARTATT), int(ENDATT), int(ATTSTEP))
    THROUGHPUT = [0 for i in ATTENRANGE]
    print "atten range = ", ATTENRANGE
    print "log file = ", LOGFILE
    print "csv file = ", CSVFILE

    index=0
    f_csv.write("Atten, Throughput\n")
    print 'Atten, ', 'Throughput'
    #loop each attenuation
    for atten in ATTENRANGE:
        sys.stdout.write(str(ATTENRANGE[index])+'dB, ')        
        f_log.write('set atten to ' + str(atten) + 'dB.\n')
        
        # 1 set attenuation
        command = Command(PTSCTL + " -f " + PORT + " -g " + str(atten) + " -n " + str(atten) + " -p " + "1", f_log)
        command.run(timeout=10)
        
        time.sleep(1)

        # 2 channel learning
        if atten > 80:
            # use 2 stream in high atten
            num_stream="2"
        else:
            num_stream=NUM_STREAM
        IPERF_CMD = IPERF + " -c " + DSTIP + " -i " + CHANNELLEARNTIME + " -t " + CHANNELLEARNTIME + " -P " + num_stream
        if args.reverse == True:
            IPERF_CMD += " -R "
        
        command = Command(IPERF_CMD, f_log)
        TIMEOUT = 100 + int(CHANNELLEARNTIME)
        command.run(timeout=TIMEOUT)
        
        time.sleep(10)
        
        # 3 send traffic
        IPERF_CMD = IPERF + " -c " + DSTIP + " -i " + TESTTIME + " -t " + TESTTIME + " -P " + num_stream
        if args.reverse == True:
            IPERF_CMD += " -R "
        
        command = Command(IPERF_CMD, f_log)    
        log_output = command.run(timeout=(100 + int(TESTTIME)))
        time.sleep(1)
        
        #write to log file
        log_lines = re.split("\n+", log_output)
        
        for line in log_lines:
            if '[SUM]' in line and 'receiver' in line:
                a = line.split()
                print a[5]+a[6]
                if a[6] == 'Kbits/sec':
                    THROUGHPUT[index] = str(round(float(a[5]) * 1.0 / 1000, 3))
                elif a[6] == 'Gbits/sec':
                    THROUGHPUT[index] = str(float(a[5]) * 1000)
                else:
                    #a[6] == 'Mbits/sec' or other:
                    THROUGHPUT[index] = str(a[5])
        f_csv.write( str( ATTENRANGE[index] ) + ", " + str(THROUGHPUT[index]) + "\n" )
        index+=1

    
except BaseException:
    print 'SystemExit'
finally:
    #write atten and throughput to log file
    print 'index =', index, len(ATTENRANGE)
    while index < len(ATTENRANGE):
        f_csv.write( str( ATTENRANGE[index] ) + ", 0\n" )
        index+=1
    
    # reset atten to 10dB
    command = Command(PTSCTL + " -f " + PORT + " -g " + "10" + " -n " + "10" + " -p " + "1", f_log)
    command.run(timeout=10)

    f_log.close()
    f_csv.close()
    
        
