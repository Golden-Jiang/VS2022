//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountService.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/29 12:40 建立於 D:\Golden\Project\VS2022\WebAPITest1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPITest1.Models;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitDataWeb;
using iitToolsWeb;
using System.Data;
using System.IO;
using System.Web;
using static iit.Data.iitData;
using iitMSGWeb;
using iit.Data;
using static iitLogWeb.ILog;
using static iitMSGWeb.iitMSG.CODE;
using Microsoft.EntityFrameworkCore;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest1
{
    public class AccountService
    {
        /// <summary>
        /// 依據電話號碼讀取台幣綁定常用帳號
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="_DBContext"></param>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public static string GetAccountFromTeleNo( string TeleNo, DBContext _DBContext, IiitLog _Log, IHttpContextAccessor _httpContextAccessor )
        {
            string                  SQLCommand = "", ClientIP = _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
 
            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );
 
                    //var result1 =   from a in _DBContext.WebTeleNo
                    //                where a.TeleNo == TeleNo
                    //                select a;
                                    //{ 
                                    //    a.TeleNo,  
                                    //    a.RecordControl,
                                    //    a.RecordControlDateTime,
                                    //    a.LastAccessTime,
                                    //    a.Ip,
                                    //    a.AccountNo, 
                                    //    a.TotalGetCallNo, 
                                    //    a.TotalForm 
                                    //}).FirstOrDefault();
                    var result1 = _DBContext.WebTeleNo.FirstOrDefault<WebTeleNo>( p => p.TeleNo == TeleNo );  

                    SQLCommand  =   $"SELECT * FROM WebTeleNo WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                    _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                    //var result11 = result1.ToList();
                    //if( result11.Count != 0 )
                    //{
                        //CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.TotalGetCallNo = result11 [ 0 ].TotalGetCallNo.ToString();
                    //    CustomerClass.TeleAccount.TotalForm = result11 [ 0 ].TotalForm.ToString();

                    if( result1 != null )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   result1.TotalGetCallNo.ToString();
                        CustomerClass.TeleAccount.TotalForm         =   result1.TotalForm.ToString();

                        //foreach( var std in result1 )
                        //{
                        //    std.RecordControl       =   2;
                        //    std.RecordControlDateTime   =   TmpDateTime1;
                        //    std.LastAccessTime          =   TmpDateTime1;
                        //    std.IP                      =   ClientIP;
                        //} // end of foreach( var std in result1 )
                        result1.RecordControl = 2;
                        result1.RecordControlDateTime = TmpDateTime1;
                        result1.LastAccessTime = TmpDateTime1;
                        result1.IP = ClientIP;

                        _DBContext.WebTeleNo.Update( result1 );
                        _DBContext.SaveChanges();

                        SQLCommand  =   $"Update WebTeleNo SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', IP='{ClientIP}' WHERE TeleNo='{TeleNo}'"; 
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                        var result2 =   _DBContext.SystemParameter.FirstOrDefault<SystemParameter>( p => p.FuncParamID == "WEBAPI" && p.ParameterCode == "0002" );  

                        if( result2 != null )
                            CustomerClass.TeleAccount.MaxForm   =   result2.ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";

                        SQLCommand  =   "SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0002' ORDER BY FuncParamID, ParameterCode"; 
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    { 
                        WebTeleNo sp = new WebTeleNo
                        { 
                            RecordControl = 1,
                            RecordControlDateTime = TmpDateTime1,
                            Enabled = 1,
                            CreateTime = TmpDateTime1,
                            LastAccessTime = TmpDateTime1,
                            TeleNo = TeleNo,
                            AccountNo = "",
                            TotalGetCallNo = 0,
                            LastGetCallNoTime = TmpDateTime1,
                            IP = ClientIP,
                            TotalForm = 0
                        };

                        _DBContext.WebTeleNo.Add( sp );
                        _DBContext.SaveChanges();

                        SQLCommand  =   $"INSERT INTO WebTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountNo, " + 
                                        $" TotalGetCallNo, LastGetCallNoTime, IP, TotalForm ) VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', 1, " +
                                        $"'{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{TeleNo}', '', 0, " +
                                        $"'{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{ClientIP}', 0 )"; 
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
 
                        CustomerClass.TeleAccount.AccountNo         =   "";
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 )
 
                    iitDataTools.SetResponseResult<AccountData.Customer>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], CustomerClass );
 
                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNo( ... )

        /// <summary>
        /// 依據電話號碼讀取台幣綁定常用帳號
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="CustID"></param>
        /// <param name="_DBContext"></param>
        /// <param name="_Log"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public static string GetAccountFromTeleNoNetBank( string TeleNo, string CustID, DBContext _DBContext, IiitLog _Log, IHttpContextAccessor _httpContextAccessor )
        {
            string                  SQLCommand = "", ClientIP = _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
            int                     ProcessResult = 0;
            int                     Result = 9; // API 作業錯誤
            int                     AddTeleNo = 0, Exist = 9;

            try
            {
                while( true )
                {
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );

                    // 以網銀帳號讀取對應的電話號碼
                    var result1 =   _DBContext.WebTeleNo.FirstOrDefault( p => p.CustID == CustID && p.TeleNo == TeleNo );
 
                    SQLCommand = $"SELECT * FROM WebTeleNo WHERE CustID='{HttpUtility.HtmlDecode( CustID )}' AND TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'";
                    _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                    if( result1 != null )
                    {
                        // 帳號與電話號碼對應相同
                        CustomerClass.TeleAccount.TeleNo            = result1.TeleNo;
                        CustomerClass.TeleAccount.AccountNo         = result1.AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    = result1.TotalGetCallNo.ToString();
                        CustomerClass.TeleAccount.TotalForm         = result1.TotalForm.ToString();

                        result1.RecordControl           =    2;
                        result1.RecordControlDateTime   =   TmpDateTime1;
                        result1.LastAccessTime          =   TmpDateTime1;
                        result1.IP                      =   ClientIP;

                        _DBContext.WebTeleNo.Update( result1 );
                        _DBContext.SaveChanges();

                        SQLCommand =    $"UPDATE WebTeleNo SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', IP='{ClientIP}' " +
                                        $"WHERE TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    {
                        // 綁定的電話號碼
                        AddTeleNo = 1;

                        // 檢查 TeleNo 是否為可綁定的電話號碼
                        ProcessResult = AvailableTeleNo( TeleNo, CustID, _DBContext, APIResult, _Log, ClientIP );
                        switch( ProcessResult )
                        {
                            case 1:   // 電話號碼不存在
                                WebTeleNo sp = new WebTeleNo
                                { 
                                    RecordControl = 1,
                                    RecordControlDateTime = TmpDateTime1,
                                    Enabled = 1,
                                    CreateTime = TmpDateTime1,
                                    LastAccessTime = TmpDateTime1,
                                    TeleNo = TeleNo,
                                    AccountNo = "",
                                    TotalGetCallNo = 0,
                                    LastGetCallNoTime = TmpDateTime1,
                                    IP = ClientIP,
                                    TotalForm = 0
                                };

                                _DBContext.WebTeleNo.Add( sp );
                                _DBContext.SaveChanges();

                                SQLCommand = $"INSERT INTO WebTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountNo, " +
                                             $" TotalGetCallNo, LastGetCallNoTime, IP, TotalForm, CustID ) VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                             $"1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                             $"'{HttpUtility.HtmlDecode( TeleNo )}', '', 0, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                             $"'{ClientIP}', 0, '{CustID}' )";
                                _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                                Exist = 0;
                                Result = 1;
                                break;
                            case 2:   // 電話號碼已存在並可綁定
                                if( ! ChangeTeleNoDB( ProcessResult, "", TeleNo, CustID, "", "", _DBContext, APIResult, _Log, ClientIP ) )
                                    break;

                                var result2 =   _DBContext.WebTeleNo.FirstOrDefault( p => p.TeleNo == TeleNo );
 
                                SQLCommand = $"SELECT * FROM WebTeleNo WHERE TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'";
                                _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                                if( result2 != null )
                                {
                                    CustomerClass.TeleAccount.TeleNo            = result2.TeleNo;
                                    CustomerClass.TeleAccount.AccountNo         = result2.AccountNo;
                                    CustomerClass.TeleAccount.TotalGetCallNo    = result2.TotalGetCallNo.ToString();
                                    CustomerClass.TeleAccount.TotalForm         = result2.TotalForm.ToString();

                                    result2.RecordControl           =    2;
                                    result2.RecordControlDateTime   =   TmpDateTime1;
                                    result2.LastAccessTime          =   TmpDateTime1;
                                    result2.QRCode                  =   "";
                                    result2.AccountNo               =   "";
                                    result2.LastGetCallNoTime       =   TmpDateTime1;
                                    result2.TotalGetCallNo          =   0;
                                    result2.IP                      =   ClientIP;
                                    result2.QRCodeStartTime         =   TmpDateTime1;
                                    result2.TotalForm               =   0;
                                    result2.CustID                  =   CustID;

                                    _DBContext.WebTeleNo.Update( result1 );
                                    _DBContext.SaveChanges();

                                    SQLCommand =    $"UPDATE WebTeleNo SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                    $"QRCode='', AccountNo='', LastGetCallNoTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TotalGetCallNo=0, " +
                                                    $"IP='{ClientIP}', QRCodeStartTime='{TmpDateTime1.ToString( "yyyy/MM/dd" )}', " +
                                                    $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                    $"TotalForm=0, CustID='{CustID}' WHERE TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'";
                                    _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                                } // end of if( result1!= null )

                                Exist = 1;
                                Result = 1;
                                break;
                            case -1:   // 電話號碼已被其他網銀帳號綁定
                                Result = 0;
                                break;
                            case -2:   // 新綁定電話號碼與舊綁定電話號碼相同
                                Result = 8;
                                break;
                            case -97:   // 仍有QRCode尚未使用
                                Result = 6;
                                break;
                            case -98:   // 變更電話號碼超過當日最大次數
                                Result = 7;
                                break;
                            case -99:   // API 作業錯誤
                                Result = 9;
                                break;
                        } // end of switch( ProcessResult )

                        if( ProcessResult < 0 )
                            break;

                        CustomerClass.TeleAccount.AccountNo         = "";
                        CustomerClass.TeleAccount.TotalGetCallNo    = "";
                        CustomerClass.TeleAccount.TotalForm         = "";

                        var result3 =   _DBContext.SystemParameter.FirstOrDefault<SystemParameter>( p => p.FuncParamID == "WEBAPI" && p.ParameterCode == "0002" );  

                        if( result3 != null )
                            CustomerClass.TeleAccount.MaxForm   =   result3.ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";

                        SQLCommand  =   "SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0002' ORDER BY FuncParamID, ParameterCode"; 
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 ) 

                    iitDataTools.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], "" );

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    Result = 9;
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            // Insert into ChangeCustIDTeleNo
            if( AddTeleNo == 1 )
            {
                ChangeCustIDTeleNo sp = new ChangeCustIDTeleNo
                { 
                    RecordControl = 1,
                    RecordControlDateTime = TmpDateTime1,
                    Enabled = 1,
                    CreateTime = TmpDateTime1,
                    CustID = CustID,
                    OldTeleNo = "",
                    NewTeleNo = TeleNo,
                    Process = 1,
                    Exist = Exist,
                    Result = Result,
                    RFU = "",
                };

                _DBContext.ChangeCustIDTeleNo.Add( sp );
                _DBContext.SaveChanges();

                SQLCommand =    $"INSERT INTO ChangeCustIDTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, CustID, OldTeleNo, " +
                                $"NewTeleNo, Process, Exist, Result, RFU ) VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', 1, " +
                                $"'{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{CustID}', '', '{TeleNo}', 1, {Exist}, {Result}, '' )";
                _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
            } // end of if( AddTeleNo == 1 && SQLError.Length == 0 )

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNoNetBank( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="CustID"></param>
        /// <param name="_DBContext"></param>
        /// <param name="APIResult"></param>
        /// <param name="_Log"></param>
        /// <param name="ClientIP"></param>
        /// <returns></returns>
        public static int AvailableTeleNo( string TeleNo, string CustID, DBContext _DBContext, iitAPIResultClass APIResult, IiitLog _Log, string ClientIP )
        {
            int             ReturnValue = 1; // 電話號碼不存在
            string          SQLCommand = "", TmpString1 = "";
            int             ChangeCount = 0, MaxChangeCount = 1;
 
            try
            {
                while( true )
                { 
 
                    // 取得變更電話號碼最大次數
                    var result1 =_DBContext.SystemParameter.FirstOrDefault( p => p.FuncParamID == "WEBAPI" && p.ParameterCode == "0007" );
                    if( result1 == null ) 
                        MaxChangeCount  =   Convert.ToInt32( result1.ParameterValue );
                    SQLCommand  =   $"SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0007' ORDER BY FuncParamID, ParameterCode"; 
                    _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                    // 取得當日變更電話號碼次數
                    SQLCommand  =   $"SELECT Count(*) FROM ChangeCustIDTeleNo WHERE CustID='{CustID}' AND CAST( CreateTime AS Date )=CAST( GETDATE() AS Date ) " +
                                    $"AND Process=2 AND Result=1";
                    var result2 =   _DBContext.ChangeCustIDTeleNo.FromSqlRaw<ChangeCustIDTeleNo>( SQLCommand );
                    if( result2 == null )
                        ChangeCount     =   result2.Count();

                    _Log.WriteLog( "SQLCommand={SQLCommand}, ChangeCount={ChangeCount}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                    if( ChangeCount >= MaxChangeCount )
                    {
                        ReturnValue =   -98;    // 變更電話號碼超過當日最大次數
                        iitDataTools.SetResponseResult<string>( APIResult, "9000", "今日已曾變更電話號碼%br%請隔日再變更", "" );
                        break;
                    } // end of if( ChangeCount >= MaxChangeCount )

                    // 判斷新電話號碼是否存在
                    var result3 = from a in _DBContext.WebTeleNo
                                  join b in _DBContext.QRCode
                                  on a.QRCode equals b.QRCode1
                                  where a.TeleNo == TeleNo
                                  select new
                                  {
                                    a.CustID, a.QRCode, b.QRCodeStratDate, b.QRCodeEndDate, b.ServiceStatus
                                  };

                    if( result3 == null )
                    {
                        TmpString1  =   result3.FirstOrDefault().CustID;
                        if( TmpString1.Length > 0 )
                        {
                            if( CustID.CompareTo( TmpString1 ) == 0 )
                            {
                                ReturnValue =   -2;  // 新綁定電話號碼與舊綁定電話號碼相同
                                iitDataTools.SetResponseResult<string>( APIResult, "9000", "新綁定電話號碼不得與舊綁定電話號碼相同%br%請輸入正確的電話號碼", "" );
                            } // end of if( CustID.CompareTo( TmpString1 ) == 0 )
                            else
                            {
                                ReturnValue =   -1;  // 已被其他網銀帳號綁定
                                iitDataTools.SetResponseResult<string>( APIResult, "9000", "此電話號碼已被其他網銀帳號綁定%br%請輸入正確的電話號碼", "" );
                            }
                        } // end of if( TmpString1.Length > 0 )
                        else
                        {
                            TmpString1  =   DateTime.Now.ToString( "yyyy/MM/dd" );
                            if( TmpString1.CompareTo( result3.FirstOrDefault().QRCodeStratDate.ToString() ) >= 0 &&
                                TmpString1.CompareTo( result3.FirstOrDefault().QRCodeEndDate.ToString() ) <= 0 && 
                                result3.FirstOrDefault().ServiceStatus.ToString() == "0" )
                            {
                                ReturnValue =   -97;    // 仍有QRCode尚未使用
                                iitDataTools.SetResponseResult<string>( APIResult, "9000", "此電話號碼仍有預填資料尚未使用%br%請確認使用後再綁定", "" );
                            }
                            else
                                ReturnValue =   2;  // 電話號碼已存在並可綁定
                        } // end of else if( TmpString1.Length > 0 )
                    } // end of if( result3 == null )

                    SQLCommand  =   $"SELECT a.CustID, a.QRCode, b.QRCodeStratDate, b.QRCodeEndDate, b.ServiceStatus FROM WebTeleNo a LEFT JOIN QRCode b ON b.QRCode=a.QRCode " +
                                    $"WHERE a.TeleNo='{TeleNo}'";
                    _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return ReturnValue;
        } // end of AvailableTeleNo( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProcessResult"></param>
        /// <param name="OldTeleNo"></param>
        /// <param name="NewTeleNo"></param>
        /// <param name="CustID"></param>
        /// <param name="OldQRCode"></param>
        /// <param name="NewQRCode"></param>
        /// <param name="_DBContext"></param>
        /// <param name="APIResult"></param>
        /// <param name="_Log"></param>
        /// <param name="ClientIP"></param>
        /// <returns></returns>
        public static bool ChangeTeleNoDB( int ProcessResult, string OldTeleNo, string NewTeleNo, string CustID, string OldQRCode, 
                                           string NewQRCode, DBContext _DBContext, iitAPIResultClass APIResult, IiitLog _Log, string ClientIP )
        {
            bool            ReturnValue = false; 
            string          SQLCommand = "";
            string []       arrTable = [ "CommonAccount", "CommonForexTransaction", "CommonTWTransaction" ];
            DateTime        TmpDateTime1 = DateTime.Now;
 
            try
            {
                while( true )
                { 
                    //if( NewTeleNo.Length > 0 )
                    //{ 
                    //    if( ( AffectedRows = _DBContext.CommonAccount.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete() ) == 0 )
                    //        throw new Exception( $"{NewTeleNo} not found in {arrTable[ 0 ]}" );
                    //    SQLCommand  =   $"DELETE FROM {arrTable[ 0 ]} WHERE TeleNo='{NewTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                    //    if( ( AffectedRows = _DBContext.CommonForexTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete() ) == 0 )
                    //        throw new Exception( $"{NewTeleNo} not found in {arrTable[ 1 ]}" );
                    //    SQLCommand  =   $"DELETE FROM {arrTable[ 1 ]} WHERE TeleNo='{NewTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                    //    if( ( AffectedRows = _DBContext.CommonTWTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete() ) == 0 )
                    //        throw new Exception( $"{NewTeleNo} not found in {arrTable[ 2 ]}" );
                    //    SQLCommand  =   $"DELETE FROM {arrTable[ 2 ]} WHERE TeleNo='{NewTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    //} // end of if( NewTeleNo.Length > 0 )
 
                    //if( OldQRCode.Length > 0 )  // 電話號碼已存在但未綁定, 需先將 NewTeleNo 對應的資料與 QRCode 對應的資料刪除
                    //{
                    //    AffectedRows = _DBContext.QRCode.Where( p => p.QRCode1 == OldQRCode ).ExecuteUpdate( s =>
                    //                   s.SetProperty( b => b.RecordControl, b => 2 ) 
                    //                   .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                    //    if( AffectedRows == 0 )
                    //        throw new Exception( $"{OldQRCode} not found in QRCode" );

                    //    SQLCommand  =   $"UPDATE QRCode SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                    //                    $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE QRCode='{OldQRCode}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    //} // end of if( OldQRCode.Length > 0 )
    
                    //if( OldTeleNo.Length > 0 )
                    //{ 
                    //    AffectedRows = _DBContext.CommonAccount.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                    //                   s.SetProperty( b => b.RecordControl, b => 2 ) 
                    //                   .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                    //    if( AffectedRows == 0 )
                    //        throw new Exception( $"{OldTeleNo} not found in {arrTable[ 0 ]}" );
                    //    SQLCommand  =   $"UPDATE {arrTable[ 0 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                    //                    $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                    //    AffectedRows = _DBContext.CommonForexTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                    //                   s.SetProperty( b => b.RecordControl, b => 2 ) 
                    //                   .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                    //    if( AffectedRows == 0 )
                    //        throw new Exception( $"{OldTeleNo} not found in {arrTable[ 1 ]}" );
                    //    SQLCommand  =   $"UPDATE {arrTable[ 1 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                    //                    $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                    //    AffectedRows = _DBContext.CommonTWTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                    //                   s.SetProperty( b => b.RecordControl, b => 2 ) 
                    //                   .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                    //                   .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                    //    if( AffectedRows == 0 )
                    //        throw new Exception( $"{OldTeleNo} not found in {arrTable[ 2 ]}" );
                    //    SQLCommand  =   $"UPDATE {arrTable[ 2 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                    //                    $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                    //    _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    //} // end of if( OldTeleNo.Length > 0 )
                    if( NewTeleNo.Length > 0 )
                    { 
                        _DBContext.CommonAccount.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                        SQLCommand  =   $"DELETE FROM {arrTable[ 0 ]} WHERE TeleNo='{NewTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        _DBContext.CommonForexTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                        SQLCommand  =   $"DELETE FROM {arrTable[ 1 ]} WHERE TeleNo='{NewTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        _DBContext.CommonTWTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                        SQLCommand  =   $"DELETE FROM {arrTable[ 2 ]} WHERE TeleNo='{NewTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( NewTeleNo.Length > 0 )
 
                    if( OldQRCode.Length > 0 )  // 電話號碼已存在但未綁定, 需先將 NewTeleNo 對應的資料與 QRCode 對應的資料刪除
                    {
                        _DBContext.QRCode.Where( p => p.QRCode1 == OldQRCode ).ExecuteUpdate( s =>
                                       s.SetProperty( b => b.RecordControl, b => 2 ) 
                                       .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        SQLCommand  =   $"UPDATE QRCode SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE QRCode='{OldQRCode}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( OldQRCode.Length > 0 )
    
                    if( OldTeleNo.Length > 0 )
                    { 
                        _DBContext.CommonAccount.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                                       s.SetProperty( b => b.RecordControl, b => 2 ) 
                                       .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        SQLCommand  =   $"UPDATE {arrTable[ 0 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        _DBContext.CommonForexTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                                       s.SetProperty( b => b.RecordControl, b => 2 ) 
                                       .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        SQLCommand  =   $"UPDATE {arrTable[ 1 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        _DBContext.CommonTWTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                                       s.SetProperty( b => b.RecordControl, b => 2 ) 
                                       .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                                       .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        SQLCommand  =   $"UPDATE {arrTable[ 2 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( OldTeleNo.Length > 0 )

                    iitDataTools.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], "" );

                    ReturnValue =   true;

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return ReturnValue;
        } // end of ChangeTeleNoDB( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="_DBContext"></param>
        /// <param name="_Log"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public static string GetForexAccountFromTeleNo( string TeleNo, DBContext _DBContext, IiitLog _Log, IHttpContextAccessor _httpContextAccessor  )
        {
            string                  SQLCommand = "", ClientIP = _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();

            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );

                    var result1 =   _DBContext.CommonAccount.FirstOrDefault<CommonAccount>( p => p.TeleNo == TeleNo );

                    SQLCommand  =   $"SELECT * FROM CommonAccount WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                    _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
//
                    if( result1 != null )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";

                        result1.RecordControl           =   2;
                        result1.RecordControlDateTime   =   TmpDateTime1;
                        result1.LastAccessTime          =   TmpDateTime1;

                        _DBContext.CommonAccount.Update( result1 );
                        _DBContext.SaveChanges();

                        SQLCommand  =   $"UPDATE CommonAccount SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}, " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}' WHERE TeleNo='{TeleNo}";
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    { 
                        CommonAccount sp = new CommonAccount
                        { 
                            RecordControl = 1,
                            RecordControlDateTime = TmpDateTime1,
                            Enabled = 1,
                            CreateTime = TmpDateTime1,
                            LastAccessTime = TmpDateTime1,
                            TeleNo = TeleNo,
                            AccountType = "F",
                            AccountNo = "",
                        };

                        _DBContext.CommonAccount.Add( sp );
                        _DBContext.SaveChanges();

                        SQLCommand  =   $"INSERT INTO CommonAccount ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountType, AccountNo, RFU ) " + 
                                        $"VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}, " + 
                                        $"'{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{TeleNo}', 'F', '', '' )"; 
                        _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );

                        CustomerClass.TeleAccount.AccountNo         =   "";
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 )

                    iitDataTools.SetResponseResult<AccountData.Customer>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], CustomerClass );

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetForexAccountFromTeleNo( ... )
    } // end of public class AccountService
} // end of namespace WebAPITest1
//===================================================================================================
// end of AccountService.cs
//===================================================================================================
