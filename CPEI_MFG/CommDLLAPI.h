//---------------------------------------------------------------------------
//      Author  : Martin lee
//      Ver     : 1.0.6
//
//
//
//---------------------------------------------------------------------------

#ifndef _COMMDLLAPI_H
#define _COMMDLLAPI_H
//---------------------------------------------------------------------------
#ifdef __cplusplus
extern "C" {
#endif

#ifdef COMMDLL_EXPORTS
#define FOXCONN_CFT_API __declspec(dllexport)
#else
#define FOXCONN_CFT_API __declspec(dllimport)
#endif

//---------------------------------------------------------------------------
/*= FindDUT =================================================================
* Description: 查找指定的設備名稱
*
* Inputs:      szDevDes           -- 需要查找的設備描述符
*              bIfGetDriverInfo   -- true:需要取回Driver版本;
*                                 -- false:不需要取回Driver版本
*              szDriverVer        -- 返回取回Driver的版本字符串
*              szDriverDate       -- 返回取回Driver的日期字符串
* Returns:     -1  : 函數執行失敗;
*              0   : 查找指定的設備描述符失敗;
*              1   : 查找指定的設備描述符成功;
*              2   : 查找指定的設備描述符成功但設備為停用狀態;
*              3   : 查找指定的設備描述符成功但設備為黃卡狀態;
*              4   : 查找指定的設備描述符成功但獲取Driver日期及版本失敗;
*              5   : 查找指定的設備描述符成功但系統提示需要重新啟動電腦;
*              6   : 查找指定的設備描述符成功但系統提示安裝Driver失敗;
*============================================================================*/
FOXCONN_CFT_API int     __stdcall FindDUT(
    const char * szDevDes,
    bool bIfGetDriverInfo,
    char * szDriverVer,
    char * szDriverDate);

//---------------------------------------------------------------------------
/*= CheckMACRule ============================================================
* Description: 檢查MACID的規則
*
* Inputs:      szMAC  -- 需要檢查的MACID
*             
* Returns:     true  : MACID 符合規則;
*              false : MACID 不符合規則;
*============================================================================*/
FOXCONN_CFT_API bool    __stdcall CheckMACRule  (const char *szMAC);


//---------------------------------------------------------------------------
/*= GetPCName ===============================================================
* Description: 獲取本地電腦的名稱
*
* Inputs:      szComputerName -- 返回12長度的電腦名稱
*
* Returns:     0  : 函數執行成功;
============================================================================*/
FOXCONN_CFT_API DWORD   __stdcall GetPCName (char *szComputerName);

//---------------------------------------------------------------------------
/*= GetMACID ================================================================
* Description: 獲取MACID
*
* Inputs:      szAdapterName  -- 需要獲取MACID的描述符
*              szMACID        -- 返回MACID
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
*============================================================================*/

FOXCONN_CFT_API bool    __stdcall GetMACID  (const char *szAdapterName, char * szMACID);


//---------------------------------------------------------------------------
/*= SyanServerTime ==========================================================
* Description: 映射網絡路徑到本地盤符，功能同MapNetSourceToLocalDriver
*
* Inputs:      szLocalName  -- 本地盤符
*              szRemoteName -- 網絡路徑
*              szPASSWORD   -- 用戶口令
*              szUSERNAME   -- 用戶名
*
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
*============================================================================*/
FOXCONN_CFT_API bool    __stdcall SyanServerTime    (
    char *szLocalName,
    char *szRemoteName,
    char *szPASSWORD,
    char *szUSERNAME);

//---------------------------------------------------------------------------
/*= MapNetSourceToLocalDriver ===============================================
* Description: 映射網絡路徑到本地盤符，功能同SyanServerTime
*
* Inputs:      szLocalName  -- 本地盤符
*              szRemoteName -- 網絡路徑
*              szPASSWORD   -- 用戶口令
*              szUSERNAME   -- 用戶名
*
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
*============================================================================*/
FOXCONN_CFT_API bool    __stdcall MapNetSourceToLocalDriver(
    char *szLocalName,
    char *szRemoteName,
    char *szPASSWORD,
    char *szUSERNAME);


//---------------------------------------------------------------------------
/*= ChangeDeviceState =======================================================
* Description: 改變設備裝置的狀態
*
* Inputs:      szDevDesc -- 需要改變狀態的描述符
*              bState    -- true:啟用設備; false:停用設備
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
============================================================================*/
FOXCONN_CFT_API bool    __stdcall ChangeDeviceState	(const char * szDevDesc, bool bState);

FOXCONN_CFT_API bool    __stdcall CheckFileSize (const char * szFileName, const DWORD  dwFileLowSize, const DWORD dwFileHighSize);

//---------------------------------------------------------------------------
/*= QueryLocalNetAdpaters ===================================================
* Description: Query 本地電腦的網卡描述符
*
* Inputs:      tempAdpters -- 返回保存本地電腦的網絡卡的描述符
*
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
============================================================================*/
FOXCONN_CFT_API bool    __stdcall QueryLocalNetAdpaters (TStringList * tempAdpters);

//---------------------------------------------------------------------------
/*= GetAdapterIPAddress =====================================================
* Description: Query 指定的Adpater名的IP地址
*
* Inputs:      szAdapterName -- 網絡卡的描述符號
*              szIPAddress   -- 網路卡的IP地址
* Returns:     true  : 函數執行成功;
*              false : 函數執行失敗;
============================================================================*/

FOXCONN_CFT_API bool    __stdcall GetAdapterIPAddress	(const char *szAdapterName, char * szIPAddress);


//---------------------------------------------------------------------------
/*= ReadStringFromRegistry ==================================================
* Description: Read keyname sring value to keyvalue
*
* Inputs:      keyroot   -- HKEY_LOCAL_MACHINE路徑下的keyroot目錄
*              keyname   -- keyroot目錄下的keyname項目
*              keyvalue  -- 返回keyname 的字符串值
*              errorcode -- 返回錯誤信息
* Returns:     -1: 函數執行失敗;
*              0 : keyname不存在;
*              1 : 成功的讀取到值;
*              2 : 讀出值成功但返回值為空.
============================================================================*/
FOXCONN_CFT_API int     __stdcall ReadStringFromRegistry(
        const char * keyroot,
        const char * keyname,
        char * keyvalue,
        char * errorcode);

//---------------------------------------------------------------------------
/*= ReadIntegerFromRegistry =================================================
* Description: Read keyname Integer value to nkeyvalue
*
* Inputs:      keyroot   -- HKEY_LOCAL_MACHINE路徑下的keyroot目錄
*              keyname   -- keyroot目錄下的keyname項目
*              keyvalue  -- 返回keyname 的整數值
*              errorcode -- 返回錯誤信息
* Returns:     -1: 函數執行失敗;
*              0 : keyname不存在;
*              1 : 成功的讀取到值;
============================================================================*/
FOXCONN_CFT_API int     __stdcall ReadIntegerFromRegistry(
        const char * keyroot,
        const char * keyname,
        int * nkeyvalue,
        char * errorcode);

//---------------------------------------------------------------------------
/*= WriteIntegerToRegistry ==================================================
* Description: Write  Integer keyvalue to  HKEY_LOCAL_MACHINE\keyroot\keyname.
*
* Inputs:      keyroot   -- HKEY_LOCAL_MACHINE路徑下的keyroot目錄
*              keyname   -- keyroot目錄下的keyname項目
*              keyvalue  -- keyname 的值
*              errorcode -- 返回錯誤信息
* Returns:     true: 寫Registry成功 ; false : 函數執行失敗
============================================================================*/
FOXCONN_CFT_API bool    __stdcall WriteIntegerToRegistry(
        const char * keyroot,
        const char * keyname,
        const int nKeyValue,
        char * errorcode);

//---------------------------------------------------------------------------
/*= WriteStringToRegistry ===================================================
* Description: Write  keyvalue to  HKEY_LOCAL_MACHINE\keyroot\keyname.
*
* Inputs:      keyroot   -- HKEY_LOCAL_MACHINE路徑下的keyroot目錄
*              keyname   -- keyroot目錄下的keyname項目
*              keyvalue  -- keyname 的值
*              errorcode -- 返回錯誤信息
* Returns:     true: 寫Registry成功 ; false : 函數執行失敗
============================================================================*/
FOXCONN_CFT_API bool    __stdcall WriteStringToRegistry(
        const char * keyroot,
        const char * keyname,
        char * keyvalue,
        char * errorcode);
//---------------------------------------------------------------------------
/*= ExecuteConsoleFileAndWaitReturn =========================================
* Description: Execute console or  file and wait the console finished.
*
* Inputs:      szConsoleFile   --  The file that will executed
*              nDelayTime_s    --  The time that will waited this console return?
* Returns:       0 : Execute the console file success
*               -1 : The console file is not exists
*               -2 : CreateProcess return false
*               -3 : Time out when wait the console file returned.
============================================================================*/


FOXCONN_CFT_API int __stdcall ExecuteConsoleFileAndWaitReturn(
        const char * szConsoleFile,
        int nDelayTime_s);
//---------------------------------------------------------------------------
/*= runShellExecute =========================================================
* Description: Execute console or  file and wait the console finished.
*
* Inputs:      szConsoleFile   --  The file that will executed
*              ShellMode       --  The window show mode
* Returns:       0 : Execute the console file success
*               -1 : Execute the console file fail
============================================================================*/
FOXCONN_CFT_API int __stdcall ExecuteConsoleFile(const char* szConsoleFile, int ShellMode);



//---------------------------------------------------------------------------
/*= CalcRetest ==============================================================
* Description: Calc Retest.
*
* Inputs:       nPass -- pass times; nFail -- Fail times;
*
* Returns:      Retest
============================================================================*/
FOXCONN_CFT_API float __stdcall CalcRetest(const int nPass, const int nFail);



/*******************************************************************************
*                                                                              *
* Routine Name : EnumExtensionFilesInDirectory                                 *
* Description  : 列舉指定路徑下某括展名的文件.                                 *
* Inputs       :                                                               *
*                strDirectory -- 待查找的路徑;                                 *
*                strExt       -- 待查找的後綴名格式為  "*.dll"                 *
*                myFileLists  -- 保存文件名的StringList指針,調用時先申請空間   *
* Returns      :                                                               *
*                0            -- 執行成功                                      *
*                1            -- 參數strDirectory不是合法的路徑                *
*                2            -- 參數myFileLists 未申請空間                    *
*                                                                              *
********************************************************************************/
FOXCONN_CFT_API int __stdcall EnumExtensionFilesInDirectory(
    const AnsiString strDirectory,
    const AnsiString strExt,
    TStringList *myFileLists
);

/*******************************************************************************
*                                                                              *
* Routine Name : DriveExists                                                   *
* Description  : 檢查輸入的Drive是否存在.                                      *
* Inputs       :                                                               *
*                strDrive -- 待查找的驅動器;                                   *
* Returns      :                                                               *
*                true     -- 指定的驅動器存在.                                 *
*                false    -- 指定的驅動器不存在.                               *
*                                                                              *
********************************************************************************/
bool FOXCONN_CFT_API __stdcall DriveExists(AnsiString strDrive);


#ifdef __cplusplus
}
#endif

#endif
