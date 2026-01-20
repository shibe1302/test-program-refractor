#
# Name: lisnauto.py
#
# Platform: Windows 8, Windows 10, 
#
# SW Requirement: Python 2.7, iperf 3.x.x
#
# HW Requirement: LISN, Attenuator
#
# Description: This script will test
#               1 Ethernet Link Speed of DUT
#               2 TCP Tx and Rx Throughtput
#

import os,time,re,datetime,sys,argparse,subprocess,threading,ctypes
from subprocess import Popen, PIPE
from argparse import RawTextHelpFormatter

# Constants from the Windows API
STD_OUTPUT_HANDLE = -11
FOREGROUND_GREEN    = 0x0002 # text color contains red.
FOREGROUND_RED    = 0x0004 # text color contains red.

#Config variables
IPERF="D:\\test_program\\test-v2\\iperf-3.1.3-win32\\iperf3.exe"
#IPERF="C:\\Users\\swlab\\Downloads\\iperf-3.0.11-win64\\iperf3.exe"
#destination IP which is connected to the Golden unit and run iperf server
DSTIP="192.168.88.98"
# Ethernet card connected to DUT.
DUT_ETH="1"
# minimum Tx and Rx TCP Throughput
MIN_THROUGHPUT=[20, 18]
# maximum Tx and Rx TCP Throughput
MAX_THROUGHPUT=[65, 60]

CHECKETHLINK=True
TESTTIME="20"
NUM_STREAM="5"
CHANNELLEARNTIME="10"
TS=datetime.datetime.utcnow().strftime("%Y-%m-%d-%H-%M-%S")
BASENAME = os.path.splitext(__file__)[0]
LOGFILE = BASENAME + '-' + TS + ".log"
CSVFILE = BASENAME + '-' + TS + ".csv"


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

def get_csbi_attributes(handle):
    # Based on IPython's winconsole.py, written by Alexander Belchenko
    import struct
    csbi = ctypes.create_string_buffer(22)
    res = ctypes.windll.kernel32.GetConsoleScreenBufferInfo(handle, csbi)
    assert res

    (bufx, bufy, curx, cury, wattr,
    left, top, right, bottom, maxx, maxy) = struct.unpack("hhhhHhhhhhh", csbi.raw)
    return wattr
    
def parse_iperf_output(output):
    log_lines = re.split("\n+", output)
    tput = 0
    for line in log_lines:
        if '[SUM]' in line and 'receiver' in line:
            a = line.split()
            #print a[5]+a[6]
            if a[6] == 'Kbits/sec':
                tput = str(round(float(a[5]) * 1.0 / 1000, 3))
            elif a[6] == 'Gbits/sec':
                tput = str(float(a[5]) * 1000)
            else:
                #a[6] == 'Mbits/sec' or other:
                tput = str(a[5])
    return tput

handle = ctypes.windll.kernel32.GetStdHandle(STD_OUTPUT_HANDLE)
reset = get_csbi_attributes(handle)

parser = argparse.ArgumentParser(prog='python lisnauto.py', description="# Name: lisnauto.py\n#\n# Platform: Windows 8, Windows 10\n#\n# SW Requirement: Python 2.7, iperf 3.x.x\n#\n# HW Requirement: LISN, Attenuator\n#\n# Description: This script will test DUT's\n#               1 Ethernet Link Speed\n#               2 TCP Tx and Rx Throughtput to Golden Node", epilog="example: python lisnauto.py -o EM-CR", formatter_class=RawTextHelpFormatter)
parser.add_argument("-o", help="log and csv filename, will be appended .log and .csv")
parser.add_argument("-v", "--version", help="print version", action='version', version="0.2")
args = parser.parse_args()

#retrieve args
if args.o:
	LOGFILE = args.o + ".log"
	CSVFILE = args.o + ".csv"

try:
    ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
    #open log file
    f_log = open(LOGFILE, "w")
    f_csv = open(CSVFILE, "w")
    #print "log file = ", LOGFILE
    #print "csv file = ", CSVFILE

    #1 check Eth link
    PLC_CMD = "int6keth.exe" + " -i " + DUT_ETH + " -r "
    command = Command(PLC_CMD, f_log)
    CMD_OUT = re.split("\r+\n+", command.run(timeout=10))
    
    for line in CMD_OUT:
        print "Test 1: Read DUT Eth Link, ", line
        if len(line) < 10:
            break
        
        if re.search("Speed=1000", line) == None:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
            print "\tEth Speed" + "\t\t\t\tFail"
        else:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
            print "\tEth Speed" + "\t\t\t\tPass"
        if re.search("Duplex=Full", line) == None:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
            print "\tEth Duplex" + "\t\t\t\tFail"
        else:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
            print "\tEth Duplex" + "\t\t\t\tPass"
        if re.search("LinkStatus=On", line) == None:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
            print "\tEth LinkStatus" + "\t\t\t\tFail"
        else:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
            print "\tEth LinkStatus" + "\t\t\t\tPass"
        if re.search("FlowControl=On", line) == None:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
            print "\tEth FLowControl" + "\t\t\t\tFail"
        else:
            ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
            print "\tEth FLowControl" + "\t\t\t\tPass"
        break
    
    ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
    # set powerline password to "HomePlugAV"
    PLC_CMD = "plctool" + " -i " + DUT_ETH + " -M " + " -K 50D3E4933F855B7040784DF815AA8DB7"
    command = Command(PLC_CMD, f_log)
    command.run(timeout=10)
    time.sleep(16)
    # reset DUT 
    PLC_CMD = "plctool" + " -i " + DUT_ETH + " -R"
    command = Command(PLC_CMD, f_log)
    command.run(timeout=10)
    
    
    time.sleep(8)
    THROUGHPUT = [0 for x in range(2)]    
    f_csv.write("DUT, Throughput\n")
    
    # 2 channel learning
    IPERF_CMD = IPERF + " -c " + DSTIP + " -i " + CHANNELLEARNTIME + " -t " + CHANNELLEARNTIME + " -P " + NUM_STREAM
    
    command = Command(IPERF_CMD, f_log)
    TIMEOUT = 10 + int(CHANNELLEARNTIME)
    command.run(timeout=TIMEOUT)
	
    time.sleep(1)
	
	# 3 test DUT Tx 
    #print "Tx"
    IPERF_CMD = IPERF + " -c " + DSTIP + " -i " + TESTTIME + " -t " + TESTTIME + " -P " + NUM_STREAM
    
    command = Command(IPERF_CMD, f_log)    
    log_output = command.run(timeout=(10 + int(TESTTIME)))
    time.sleep(1)
	
    THROUGHPUT[0] = parse_iperf_output( log_output )
    f_csv.write( "Tx" + ", " + str(THROUGHPUT[0]) + "\n" )
    output_msg = "Test 2: Tx Throughput = " + THROUGHPUT[0] + "Mbps in (" + str(MIN_THROUGHPUT[0]) + ", " + str(MAX_THROUGHPUT[0]) + ")"
    if float(THROUGHPUT[0]) > MIN_THROUGHPUT[0] and float(THROUGHPUT[0]) < MAX_THROUGHPUT[0]:
        print output_msg + "\t\t\t\tPass"
    else:
        ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
        print output_msg + "\t\t\t\tFail"
        ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
        
        
    # 4 test DUT Rx 
    #print "send traffic"
    IPERF_CMD = IPERF + " -c " + DSTIP + " -i " + TESTTIME + " -t " + TESTTIME + " -P " + NUM_STREAM + " -R"
	
    command = Command(IPERF_CMD, f_log)    
    log_output = command.run(timeout=(10 + int(TESTTIME)))
    time.sleep(1)
	
    THROUGHPUT[1] = parse_iperf_output( log_output )
    f_csv.write( "Rx" + ", " + str(THROUGHPUT[1]) + "\n" )
    output_msg = "Test 3: Rx Throughput = " + THROUGHPUT[1] + "Mbps in (" + str(MIN_THROUGHPUT[1]) + ", " + str(MAX_THROUGHPUT[1]) + ")"
    if float(THROUGHPUT[1]) > MIN_THROUGHPUT[1] and float(THROUGHPUT[1]) < MAX_THROUGHPUT[1]:
        print output_msg + "\t\t\t\tPass"
    else:
        ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
        print output_msg + "\t\t\t\tFail"
        ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_GREEN)
        
    # 5 reset DUT to factory default
    PLC_CMD = "plctool" + " -i " + DUT_ETH + " -T "
    command = Command(PLC_CMD, f_log)
    command.run(timeout=10)
    time.sleep(5)
        


    
except Exception as error:
    ctypes.windll.kernel32.SetConsoleTextAttribute(handle, FOREGROUND_RED)
    #print repr(error)
    print error.message
    
finally:
    f_log.close()
    f_csv.close()
    ctypes.windll.kernel32.SetConsoleTextAttribute(handle, reset)
    
        
