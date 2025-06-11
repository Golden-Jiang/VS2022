//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountService.cs
// Description   : 帳號處理服務
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:00 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using WebAPITest6.Models;
using WebAPITest6.DTO;

using iitSystemWeb;
using iitDataWeb;
using iitLogWeb;
using iitMSGWeb;
using iitToolsWeb;
using WebAPITest6.Repository;
using Microsoft.EntityFrameworkCore;
using System.Web;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public class AccountService : IAccountService
    {
        private readonly WebAPIRepository       _repository;
        private readonly DBContext              _DBContext;
        private readonly IiitLog                _Log;
        private readonly string                 _ClientIP;

        public AccountService(
            IHttpContextAccessor httpContextAccessor,  
            WebAPIRepository repository, 
            DBContext dBContext, 
            IiitLog Log )
        {
            _repository             =   repository;
            _DBContext              =   dBContext;
            _Log                    =   Log;
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );
        } // end of public AccountService( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public string GetAccountFromTeleNo( string TeleNo )
        {
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();

            try
            {
                while( true )
                {
                    if( ! TaiwanIdValidator.isMobilePhoneNumber( TeleNo ) )
                        throw new iitException( "電話號碼錯誤" );

                    var result1 = _repository.Select<WebTeleNo>( "WebTeleNo", "TeleNo = @0", TeleNo );
                    if( result1 != null )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   result1.TotalGetCallNo.ToString();
                        CustomerClass.TeleAccount.TotalForm         =   result1.TotalForm.ToString();

                        result1.RecordControl = 2;
                        result1.RecordControlDateTime = TmpDateTime1;
                        result1.LastAccessTime = TmpDateTime1;
                        result1.IP = _ClientIP;

                        _repository.Update<WebTeleNo>( "WebTeleNo", "TeleNo = @0", TeleNo, result1 );

                        var result2 = _repository.Select<SystemParameter>( "SystemParameter", "FuncParamID = @0 && ParameterCode = @1", "WEBAPI", "0002" );

                        if( result2 != null )
                            CustomerClass.TeleAccount.MaxForm   =   result2.ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";
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
                            IP = _ClientIP,
                            TotalForm = 0
                        };

                        _repository.Insert<WebTeleNo>( "WebTeleNo", sp );
 
                        CustomerClass.TeleAccount.AccountNo         =   "";
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 )

                    iitDataTools.SetResponseResult<AccountData.Customer>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], CustomerClass );

                    break;
                } // end of while( true )
            } // end of try
            catch(Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

                if(except.GetType() == typeof( iitException ) )
                    iitDataTools.SetResponseResult<string>( APIResult, "2000", except.Message, "" );
                else
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNo( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public string GetForexAccountFromTeleNo( string TeleNo )
        {
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();

            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );

                    var result1 =   _repository.Select<CommonAccount>( "CommonAccount", "TeleNo = @0", TeleNo );

                    if( result1 != null )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";

                        result1.RecordControl           =   2;
                        result1.RecordControlDateTime   =   TmpDateTime1;
                        result1.LastAccessTime          =   TmpDateTime1;

                        _repository.Update<CommonAccount>( "CommonAccount", "TeleNo = @0", TeleNo, result1 );
                    } // end of if( result1 != null )
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

                        _repository.Insert<CommonAccount>( "CommonAccount", sp );

                        CustomerClass.TeleAccount.AccountNo         =   "";
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";
                    } // end of else if( result1 != null )

                    iitDataTools.SetResponseResult<AccountData.Customer>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], CustomerClass );

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

                if(except.GetType() == typeof( iitException ) )
                    iitDataTools.SetResponseResult<string>( APIResult, "2000", except.Message, "" );
                else
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetForexAccountFromTeleNo( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="CustID"></param>
        /// <returns></returns>
        /// <exception cref="iitException"></exception>
        public string GetAccountFromTeleNoNetBank( string TeleNo, string CustID )
        {
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
                    if( ! TaiwanIdValidator.isMobilePhoneNumber( TeleNo ) )
                        throw new iitException( "電話號碼錯誤" );

                    if( ! TaiwanIdValidator.IsNationalIdentificationNumberValid( CustID ) )
                        throw new iitException( "身分證號碼錯誤" );

                    // 以網銀帳號讀取對應的電話號碼
                    var result1 =   _repository.Select<WebTeleNo>( "WebTeleNo", "TeleNo = @0 && CustID = @1", TeleNo, CustID );
 
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
                        result1.IP                      =   _ClientIP;

                        _repository.Update<WebTeleNo>( "WebTeleNo", "TeleNo = @0 && CustID = @1", TeleNo, CustID, result1 );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    {
                        // 綁定的電話號碼
                        AddTeleNo = 1;

                        // 檢查 TeleNo 是否為可綁定的電話號碼
                        ProcessResult = AvailableTeleNo( TeleNo, CustID, APIResult );
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
                                    IP = _ClientIP,
                                    TotalForm = 0,
                                    CustID = CustID
                                };

                                _repository.Insert<WebTeleNo>( "WebTeleNo", result1 );

                                Exist = 0;
                                Result = 1;
                                break;
                            case 2:   // 電話號碼已存在並可綁定
                                if( ! ChangeTeleNoDB( ProcessResult, "", TeleNo, CustID, "", "", APIResult ) )
                                    break;

                                var result2 =   _repository.Select<WebTeleNo>( "WebTeleNo", "TeleNo = @0", TeleNo );

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
                                    result2.IP                      =   _ClientIP;
                                    result2.QRCodeStartTime         =   TmpDateTime1;
                                    result2.TotalForm               =   0;
                                    result2.CustID                  =   CustID;

                                    _repository.Update<WebTeleNo>( "WebTeleNo", "TeleNo = @0", TeleNo, result2 );
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

                        var result3 =   _repository.Select<SystemParameter>( "SystemParameter", "FuncParamID = @0 && ParameterCode = @1", "WEBAPI", "0002" );

                        if( result3 != null )
                            CustomerClass.TeleAccount.MaxForm   =   result3.ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 ) 

                    iitDataTools.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], "" );

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

                if(except.GetType() == typeof( iitException ) )
                    iitDataTools.SetResponseResult<string>( APIResult, "2000", except.Message, "" );
                else
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
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

                _repository.Insert<ChangeCustIDTeleNo>( "ChangeCustIDTeleNo", sp );
            } // end of if( AddTeleNo == 1 && SQLError.Length == 0 )

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNoNetBank( ... )

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <param name="CustID"></param>
        /// <param name="APIResult"></param>
        /// <returns></returns>
        private int AvailableTeleNo( string TeleNo, string CustID, iitAPIResultClass APIResult )
        {
            int             ReturnValue = 1; // 電話號碼不存在
            string          TmpString1 = "";
            int             ChangeCount = 0, MaxChangeCount = 1;
 
            try
            {
                while( true )
                { 
                    // 取得變更電話號碼最大次數
                    var result1 =   _repository.Select<SystemParameter>( "SystemParameter", "FuncParamID = @0 && ParameterCode = @1", "WEBAPI", "0007" );
                    if( result1 != null ) 
                        MaxChangeCount  =   Convert.ToInt32( result1.ParameterValue );

                    // 取得當日變更電話號碼次數
                    ChangeCount =   _repository.CaculateTodayChangeCustIDTeleNoCount( CustID );

                    if( ChangeCount >= MaxChangeCount )
                    {
                        ReturnValue =   -98;    // 變更電話號碼超過當日最大次數
                        iitDataTools.SetResponseResult<string>( APIResult, "9000", "今日已曾變更電話號碼%br%請隔日再變更", "" );
                        break;
                    } // end of if( ChangeCount >= MaxChangeCount )

                    // 判斷新電話號碼是否存在
                    var result3 = _repository.WebTeleNoJoinQRCode( TeleNo );

                    if( result3 != null )
                    {
                        TmpString1  =   result3.CustID;
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
                            } // end of if( CustID.CompareTo( TmpString1 ) == 0 )
                        } // end of if( TmpString1.Length > 0 )
                        else
                        {
                            TmpString1  =   DateTime.Now.ToString( "yyyy/MM/dd" );
                            if( TmpString1.CompareTo( result3.QRCodeStratDate.ToString() ) >= 0 &&
                                TmpString1.CompareTo( result3.QRCodeEndDate.ToString() ) <= 0 &&
                                result3.ServiceStatus.ToString() == "0" )
                            {
                                ReturnValue = -97;    // 仍有QRCode尚未使用
                                iitDataTools.SetResponseResult<string>( APIResult, "9000", "此電話號碼仍有預填資料尚未使用%br%請確認使用後再綁定", "" );
                            }
                            else
                                ReturnValue = 2;  // 電話號碼已存在並可綁定
                        } // end of else if( TmpString1.Length > 0 )
                    } // end of if( result3 == null )
                    else
                    {
                        var result4 =   _repository.Select<WebTeleNo>( "WebTeleNo", "TeleNo = @0", TeleNo );
                        if( result4 != null )
                            ReturnValue = 2;  // 電話號碼已存在並可綁定
                    } // end of  // end of if( result3 == null )

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

                if(except.GetType() == typeof( iitException ) )
                    iitDataTools.SetResponseResult<string>( APIResult, "2000", except.Message, "" );
                else
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
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
        /// <param name="APIResult"></param>
        /// <returns></returns>
        private bool ChangeTeleNoDB( int ProcessResult, string OldTeleNo, string NewTeleNo, string CustID, string OldQRCode, 
                                     string NewQRCode, iitAPIResultClass APIResult )
        {
            bool            ReturnValue = false; 
            DateTime        TmpDateTime1 = DateTime.Now;
            //string []       arrTable = [ "CommonAccount", "CommonForexTransaction", "CommonTWTransaction" ];
 
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
                        _repository.Delete<CommonAccount>( "CommonAccount", "TeleNo = @0", NewTeleNo );
                        //_DBContext.CommonAccount.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                        //SQLCommand  =   $"DELETE FROM {arrTable[ 0 ]} WHERE TeleNo='{NewTeleNo}'";
                        //_Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                         _repository.Delete<CommonForexTransaction>( "CommonForexTransaction", "TeleNo = @0", NewTeleNo );
                       //_DBContext.CommonForexTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                       // SQLCommand  =   $"DELETE FROM {arrTable[ 1 ]} WHERE TeleNo='{NewTeleNo}'";
                       // _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        _repository.Delete<CommonTWTransaction>( "CommonTWTransaction", "TeleNo = @0", NewTeleNo );
                       //_DBContext.CommonTWTransaction.Where( p => p.TeleNo == NewTeleNo ).ExecuteDelete();
                       // SQLCommand  =   $"DELETE FROM {arrTable[ 2 ]} WHERE TeleNo='{NewTeleNo}'";
                       // _Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( NewTeleNo.Length > 0 )
 
                    if( OldQRCode.Length > 0 )  // 電話號碼已存在但未綁定, 需先將 NewTeleNo 對應的資料與 QRCode 對應的資料刪除
                    {
                        var result1 = _repository.Select<QRCodes>( "QRCodes", "QRCode = @0", OldQRCode );
                        if( result1 != null ) 
                        {
                            result1.RecordControl = 2;
                            result1.RecordControlDateTime = TmpDateTime1;
                            result1.LastAccessTime = TmpDateTime1;
                            result1.TeleNo = NewTeleNo;

                            _repository.Update<QRCodes>( "QRCodes", "QRCode = @0", OldQRCode, result1 );
                        } // end of if( result1 != null )
                        //_DBContext.QRCodes.Where( p => p.QRCode == OldQRCode ).ExecuteUpdate( s =>
                        //               s.SetProperty( b => b.RecordControl, b => 2 ) 
                        //               .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        //SQLCommand  =   $"UPDATE QRCode SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                        //                $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE QRCode='{OldQRCode}'";
                        //_Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( OldQRCode.Length > 0 )
    
                    if( OldTeleNo.Length > 0 )
                    { 
                        var result1 = _repository.Select<CommonAccount>( "CommonAccount", "TeleNo = @0", OldTeleNo );
                        if( result1 != null ) 
                        {
                            result1.RecordControl = 2;
                            result1.RecordControlDateTime = TmpDateTime1;
                            result1.LastAccessTime = TmpDateTime1;
                            result1.TeleNo = NewTeleNo;

                            _repository.Update<CommonAccount>( "CommonAccount", "TeleNo = @0", OldTeleNo, result1 );
                        } // end of if( result1 != null )
                        //_DBContext.CommonAccount.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                        //               s.SetProperty( b => b.RecordControl, b => 2 ) 
                        //               .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        //SQLCommand  =   $"UPDATE {arrTable[ 0 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                        //                $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        //_Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        var result2 = _repository.Select<CommonForexTransaction>( "CommonForexTransaction", "TeleNo = @0", OldTeleNo );
                        if( result2 != null ) 
                        {
                            result2.RecordControl = 2;
                            result2.RecordControlDateTime = TmpDateTime1;
                            result2.LastAccessTime = TmpDateTime1;
                            result2.TeleNo = NewTeleNo;

                            _repository.Update<CommonForexTransaction>( "CommonForexTransaction", "TeleNo = @0", OldTeleNo, result2 );
                        } // end of if( result1 != null )
                        //_DBContext.CommonForexTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                        //               s.SetProperty( b => b.RecordControl, b => 2 ) 
                        //               .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        //SQLCommand  =   $"UPDATE {arrTable[ 1 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                        //                $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        //_Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                        
                        
                        var result3 = _repository.Select<CommonTWTransaction>( "CommonTWTransaction", "TeleNo = @0", OldTeleNo );
                        if( result2 != null ) 
                        {
                            result3.RecordControl = 2;
                            result3.RecordControlDateTime = TmpDateTime1;
                            result3.LastAccessTime = TmpDateTime1;
                            result3.TeleNo = NewTeleNo;

                            _repository.Update<CommonTWTransaction>( "CommonTWTransaction", "TeleNo = @0", OldTeleNo, result3 );
                        } // end of if( result1 != null )
                        //_DBContext.CommonTWTransaction.Where( p => p.TeleNo == OldTeleNo ).ExecuteUpdate( s =>
                        //               s.SetProperty( b => b.RecordControl, b => 2 ) 
                        //               .SetProperty( b => b.RecordControlDateTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.LastAccessTime, b => TmpDateTime1 )
                        //               .SetProperty( b => b.TeleNo, b => NewTeleNo ) );
                        //SQLCommand  =   $"UPDATE {arrTable[ 2 ]} SET RecordControl=2, RecordControlDateTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', " +
                        //                $"LastAccessTime='{TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}', TeleNo='{NewTeleNo}' WHERE TeleNo='{OldTeleNo}'";
                        //_Log.WriteLog( $"SQLCommand={SQLCommand }", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, ClientIP );
                    } // end of if( OldTeleNo.Length > 0 )

                    iitDataTools.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], "" );

                    ReturnValue =   true;

                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

                if(except.GetType() == typeof( iitException ) )
                    iitDataTools.SetResponseResult<string>( APIResult, "2000", except.Message, "" );
                else
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
            } // end of catch

            return ReturnValue;
        } // end of ChangeTeleNoDB( ... )
    } // end of public class AccountService : IAccountService
} // end of namespace WebAPITest6
//===================================================================================================
// end of AccountService.cs
//===================================================================================================
