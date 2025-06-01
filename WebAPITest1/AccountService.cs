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
        public static string GetAccountFromTeleNo( string TeleNo, DBContext _DBContext, IHttpContextAccessor httpContextAccessor )
        {
            string                  SQLCommand = "";
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
            ILog                    iLog = new ILog( httpContextAccessor );
 
            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );
 
                    var result1 =   from a in _DBContext.WebTeleNo
                                    where a.TeleNo == TeleNo
                                    select a;
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

                    SQLCommand  =   $"SELECT * FROM WebTeleNo WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                    iLog.WriteLog( $"{SQLCommand}, result rows={result1.Count()}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );

                    //var result11 = result1.ToList();
                    //if( result11.Count != 0 )
                    //{
                        //CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.TotalGetCallNo = result11 [ 0 ].TotalGetCallNo.ToString();
                    //    CustomerClass.TeleAccount.TotalForm = result11 [ 0 ].TotalForm.ToString();

                    if( result1.Count() != 0 )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.FirstOrDefault().TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.FirstOrDefault().AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   result1.FirstOrDefault().TotalGetCallNo.ToString();
                        CustomerClass.TeleAccount.TotalForm         =   result1.FirstOrDefault().TotalForm.ToString();

                        foreach( var std in result1 )
                        {
                            std.RecordControl           =   2;
                            std.RecordControlDateTime   =   TmpDateTime1;
                            std.LastAccessTime          =   TmpDateTime1;
                            std.IP                      =   httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
                        } // end of foreach( var std in result1 )

                        _DBContext.SaveChanges();

                        SQLCommand  =   String.Format( "Update WebTeleNo SET RecordControl=2, RecordControlDateTime='{0}', LastAccessTime='{1}', IP='{2}' WHERE TeleNo='{3}'", 
                                        TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), 
                                        httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString(), TeleNo );
                        iLog.WriteLog( "SQLCommand=" + SQLCommand, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );

                        var result2 =   from a in _DBContext.SystemParameter
                                        where a.FuncParamID == "WEBAPI" && a.ParameterCode == "0002"
                                        select a;

                        if( result2.Count() != 0 )
                            CustomerClass.TeleAccount.MaxForm   =   result2.FirstOrDefault().ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";

                        SQLCommand  =   "SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0002' ORDER BY FuncParamID, ParameterCode"; 
                        iLog.WriteLog( $"SQLCommand={SQLCommand} result rows={result2.Count()}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
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
                            IP = httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString(),
                            TotalForm = 0
                        };

                        _DBContext.WebTeleNo.Add( sp );
                        _DBContext.SaveChanges();

                        SQLCommand  =   String.Format( "INSERT INTO WebTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountNo, " + 
                                        " TotalGetCallNo, LastGetCallNoTime, IP, TotalForm ) VALUES ( 1, '{0}', 1, '{1}', '{2}', '{3}', '', 0, '{4}', '{5}', 0 )", 
                                        TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ),
                                        TeleNo, TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString() );
                        iLog.WriteLog( "SQLCommand=" + SQLCommand, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
 
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
                    iLog.Log.except =   except;
                    iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
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
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public static string GetAccountFromTeleNoNetBank( string TeleNo, string CustID, DBContext _DBContext, IHttpContextAccessor httpContextAccessor )
        {
            string                  TmpString1 = "", TmpString2 = "";
            int                     Result = 9; // API 作業錯誤
            int                     AddTeleNo = 0, Exist = 9;
            string                  SQLCommand = "";
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
            ILog                    iLog = new ILog( httpContextAccessor );

            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );
 
                    // 以網銀帳號讀取對應的電話號碼
                    var result1 =   from a in _DBContext.WebTeleNo
                                    where a.CustID == CustID
                                    select a;

                    SQLCommand  =   $"SELECT * FROM WebTeleNo WHERE CustID='{HttpUtility.HtmlDecode( CustID )}' AND TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'";
                    ds          =   iitDB.GetDataSet( SQLCommand, out RecordCount, out SQLError, ref APIResult.RespCode, ref APIResult.RespDesc );
                    iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", RecordCount=" + RecordCount.ToString(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                    if( SQLError == string.Empty && RecordCount > 0 )
                    {
                        // 帳號與電話號碼對應相同
                        APIResult.TeleAccount.AccountNo         =   ds.Tables[ 0 ].Rows[ 0 ][ "AccountNo" ].ToString().Trim();
                        APIResult.TeleAccount.TotalGetCallNo    =   ds.Tables[ 0 ].Rows[ 0 ][ "TotalGetCallNo" ].ToString().Trim();
                        APIResult.TeleAccount.TotalForm         =   ds.Tables[ 0 ].Rows[ 0 ][ "TotalForm" ].ToString().Trim();
 
                        SQLCommand  =   $"UPDATE WebTeleNo SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', IP='{HttpContext.Current.Request.UserHostAddress}' " +
                                        $"WHERE TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'"; 
                        iitDB.ExecuteNonQuery( SQLCommand, out SQLError, ref APIResult.RespCode, ref APIResult.RespDesc );
                        iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", SQLError=" + SQLError, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                        Utility.CheckSQLResult( SQLError );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    { 
                        Utility.CheckSQLResult( SQLError );
 
                        // 綁定的電話號碼
                        AddTeleNo   =   1;

                        // 檢查 TeleNo 是否為可綁定的電話號碼
                        ProcessResult   =   AvailableTeleNo( TeleNo, CustID, APIResult );
                        switch( ProcessResult )
                        {
                            case    1   :   // 電話號碼不存在
                                SQLCommand  =   $"INSERT INTO WebTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountNo, " + 
                                                $" TotalGetCallNo, LastGetCallNoTime, IP, TotalForm, CustID ) VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                $"1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                $"'{HttpUtility.HtmlDecode( TeleNo )}', '', 0, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                $"'{HttpContext.Current.Request.UserHostAddress}', 0, '{CustID}' )"; 
                                iitDB.ExecuteNonQuery( SQLCommand, out SQLError, ref APIResult.RespCode, ref APIResult.RespDesc );
                                iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", SQLError=" + SQLError, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                                Utility.CheckSQLResult( SQLError );

                                Exist   =   0;
                                Result  =   1;
                               break;
                            case    2   :   // 電話號碼已存在並可綁定
                                if( ! ChangeTeleNoDB( ProcessResult, "", TeleNo, CustID, "", "", APIResult ) )
                                    break;

                                SQLCommand  =   $"UPDATE WebTeleNo SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                $"QRCode='', AccountNo='', LastGetCallNoTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TotalGetCallNo=0, " +
                                                $"IP='{HttpContext.Current.Request.UserHostAddress}', QRCodeStartTime='{TmpDateTime1.ToString( "yyyy/MM/dd" )}', " + 
                                                $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                                                $"TotalForm=0, CustID='{CustID}' WHERE TeleNo='{HttpUtility.HtmlDecode( TeleNo )}'"; 
                                iitDB.ExecuteNonQuery( SQLCommand, out SQLError, ref APIResult.RespCode, ref APIResult.RespDesc );
                                iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", SQLError=" + SQLError, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                                Utility.CheckSQLResult( SQLError );

                                Exist   =   1;
                                Result  =   1;
                                break;
                            case    -1  :   // 電話號碼已被其他網銀帳號綁定
                                Result  =   0;
                                break;
                            case    -2  :   // 新綁定電話號碼與舊綁定電話號碼相同
                                Result  =   8;
                                break;
                            case    -97 :   // 仍有QRCode尚未使用
                                Result  =   6;
                                break;
                            case    -98 :   // 變更電話號碼超過當日最大次數
                                Result  =   7;
                                break;
                            case    -99 :   // API 作業錯誤
                                Result  =   9;
                                break;
                        } // end of switch( ProcessResult )

                        if( ProcessResult < 0 )
                            break;

                        APIResult.TeleAccount.AccountNo         =   "";
                        APIResult.TeleAccount.TotalGetCallNo    =   "";
                        APIResult.TeleAccount.TotalForm         =   "";

                        SQLCommand  =   "SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0002' ORDER BY FuncParamID, ParameterCode"; 
                        ds          =   iitDB.GetDataSet( SQLCommand, out RecordCount, out SQLError, ref APIResult.RespCode, ref APIResult.RespDesc );
                        iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", RecordCount=" + RecordCount.ToString(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                        if( SQLError == string.Empty && RecordCount > 0 )
                            APIResult.TeleAccount.MaxForm   =   ds.Tables[ 0 ].Rows[ 0 ][ "ParameterValue" ].ToString().Trim();
                        else
                        {
                            Utility.CheckSQLResult( SQLError );
                            APIResult.TeleAccount.MaxForm   =   "10";
                        } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 ) 
 
                    Utility.SetResponseResult( APIResult, "0000", "交易成功" );
 
                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    Result  =   9;
                    iLog.except =   except;
                    iitLog.WriteLog( iLog, "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                    Utility.SetResponseResult( APIResult, "8000", "WebAPI 作業錯誤" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            // Insert into ChangeCustIDTeleNo
            if( AddTeleNo == 1 && SQLError.Length == 0 ) 
            {
                SQLCommand  =   $"INSERT INTO ChangeCustIDTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, CustID, OldTeleNo, " + 
                                $"NewTeleNo, Process, Exist, Result, RFU ) VALUES ( 1, '{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', 1, " +
                                $"'{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', '{CustID}', '', '{TeleNo}', 1, {Exist}, {Result}, '' )";
                iitDB.ExecuteNonQuery( SQLCommand, out SQLError, ref TmpString1, ref TmpString2 );
                iitLog.WriteLog( iLog, "SQLCommand=" + SQLCommand + ", SQLError=" + SQLError, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            } // end of if( AddTeleNo == 1 && SQLError.Length == 0 )

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNoNetBank( ... )
        /// <summary>
        /// // 依據電話號碼讀取外幣綁定常用帳號
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public static string GetForexAccountFromTeleNo( string TeleNo, DBContext _DBContext, IHttpContextAccessor httpContextAccessor  )
        {
            string                  SQLCommand = "";
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
            ILog                    iLog = new ILog( httpContextAccessor );

            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );

                    var result1 =   from a in _DBContext.CommonAccount
                                    where a.TeleNo == TeleNo
                                    select a;

                    SQLCommand  =   $"SELECT * FROM CommonAccount WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                    iLog.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
//
                    if( result1.Count() != 0 )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.FirstOrDefault().TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.FirstOrDefault().AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";

                        foreach( var std in result1 )
                        {
                            std.RecordControl           =   2;
                            std.RecordControlDateTime   =   TmpDateTime1;
                            std.LastAccessTime          =   TmpDateTime1;
                        } // end of foreach( var std in result1 )

                        _DBContext.SaveChanges();

                        SQLCommand  =   $"UPDATE CommonAccount SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}, " +
                                        $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}' WHERE TeleNo='{TeleNo}";
                        iLog.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
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
                        iLog.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );

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
                    iLog.Log.except =   except;
                    iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
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
