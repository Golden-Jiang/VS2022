//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 15:30 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPITest6.Models;

using iitLogWeb;
using iitToolsWeb;
using iitDataWeb;
using iitMSGWeb;
using iitSystemWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService        _AccountService;
        private readonly IHttpContextAccessor   _httpContextAccessor;
        private readonly IiitLog                _Log;
        private readonly string                 _ClientIP;

        public AccountController( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IAccountService AccountService, IiitLog Log )
        {
            _AccountService         =   AccountService;
            _httpContextAccessor    =   httpContextAccessor;
            _Log                    =   Log; 
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );

            //iitSystemTools.SetClientEnvironment( httpContextAccessor, _Log,  _ClientIP );
        } // end of public AccountController

        // GET: http://url/Account?Query1=.....
        [HttpGet]
        public string Get()
        {
            string              ReturnValue = "";
            string              TxCode = "";
            string              TmpString1 = "", TmpString2 = "";
            iitAPIResultClass   APIResult = new iitAPIResultClass();

            try
            {
                while( true )
                {
                    if( ( ReturnValue = iitDataTools.CheckQuery( _httpContextAccessor, APIResult, "TxCode", ref TxCode ) ) != "" )
                        break;

                    switch( TxCode )
                    {
                        case    "SA013001"      :   // 依據電話號碼讀取台幣綁定常用帳號
                            if( ( ReturnValue = iitDataTools.CheckQuery( _httpContextAccessor, APIResult, "TeleNo", ref TmpString1 ) ) != "" )
                                break;

                            if( ( ReturnValue = iitDataTools.CheckQuery( _httpContextAccessor, APIResult, "ND", ref TmpString2 ) ) != "" )
                                TmpString2  =   "";

                            if( TmpString2.Length == 0 )
                                ReturnValue =   _AccountService.GetAccountFromTeleNo( TmpString1 );
                            else
                                ReturnValue =   _AccountService.GetAccountFromTeleNoNetBank( TmpString1, TmpString2 );
                            break;
                        case    "SA013001F"      :   // 依據電話號碼讀取外幣綁定常用帳號
                            if( ( ReturnValue = iitDataTools.CheckQuery( _httpContextAccessor, APIResult, "TeleNo", ref TmpString1 ) ) != "" )
                                break;

                            ReturnValue =   _AccountService.GetForexAccountFromTeleNo( TmpString1 );
                            break;
                        //                        case    "SA013101"      :   // 變更綁定電話號碼
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "ND", ref TmpString1 ) ) != "" )
                        //                                break;

                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "OldTeleNo", ref TmpString2 ) ) != "" )
                        //                                break;

                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "NewTeleNo", ref TmpString3 ) ) != "" )
                        //                                break;

                        //                            lock( Global.AccountMutex )
                        //                            {
                        //                                ReturnValue =   AccountService.ChangeTeleNoNetBank( TmpString1, TmpString2, TmpString3 );
                        //                            } // end of lock( Global.AccountMutex )
                        //                            break;
                        //                        case    "SA013002"      :   // 儲存電話號碼的台幣綁定常用帳號
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "TeleNo", ref TmpString1 ) ) != "" )
                        //                                break;
                        ////
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString2, false ) ) != "" )
                        //                                break;
                        ////
                        //                            ReturnValue =   AccountService.SaveAccountNo( TmpString1, TmpString2 );
                        //                            break;
                        //                        case    "SA013002F"     :   // 儲存電話號碼的外幣綁定常用帳號
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "TeleNo", ref TmpString1 ) ) != "" )
                        //                                break;
                        ////
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString2, false ) ) != "" )
                        //                                break;
                        ////
                        //                            ReturnValue =   AccountService.SaveAccountNoF( TmpString1, TmpString2 );
                        //                            break;
                        //                        case    "SA013003"      :   // 帳號驗證
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString1, true ) ) != "" )
                        //                                break;

                        //                            ReturnValue =   AccountService.CheckAccountNo( TmpString1 );
                        //                            break;
                        //                        case    "SA013003A"     :   // 帳號驗證
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString1, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "Checkforeign", ref TmpString2, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "CheckVirtual", ref TmpString3, true ) ) != "" )
                        //                                break;

                        //                            ReturnValue =   AccountService.CheckAccountNoA( TmpString1, TmpString2, TmpString3 );
                        //                            break;
                        //                        case    "SA013003B"     :   // 帳號驗證
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString1, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "ID", ref TmpString2, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "Checkforeign", ref TmpString3, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "CheckVirtual", ref TmpString4, true ) ) != "" )
                        //                                break;

                        //                            ReturnValue =   AccountService.CheckAccountNoB( TmpString1, TmpString2, TmpString3, TmpString4 );
                        //                            break;
                        //                        case    "SA013003C"     :   // 帳號驗證, 返回 AccountType 與 CustID 
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString1, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "Checkforeign", ref TmpString2, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "CheckVirtual", ref TmpString3, true ) ) != "" )
                        //                                break;

                        //                            ReturnValue =   AccountService.CheckAccountNoC( TmpString1, TmpString2, TmpString3 );
                        //                            break;
                        //                        case    "SA013004"      :   // 台灣解款行與分支機構列表
                        //                            ReturnValue =   AccountService.GetTaiwanBankList();
                        //                            break;
                        //                        case    "SA013005"     :    // 帳號驗證, 返回 CustID 與 姓名(中文或英文)
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "AccountNo", ref TmpString1, true ) ) != "" )
                        //                                break;
                        //                            if( ( ReturnValue = Utility.CheckGetParameter( ControllerContext, APIResult, "Language", ref TmpString2, true ) ) != "" )
                        //                                break;

                        //                            ReturnValue =   AccountService.GetCustomerIDName( TmpString1, TmpString2 );
                        //                            break;
                        default:   // Undefine API
                            iitDataTools.SetResponseResult<string>( APIResult, "1000", $"{iitMSG.APIError.E1000} {TxCode}", "" );
                            ReturnValue =   JsonConvert.SerializeObject( APIResult );

                            _Log.WriteLog( APIResult.RespDesc, iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
                            break;
                    } // end of switch( TxCode )

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

                ReturnValue = JsonConvert.SerializeObject( APIResult );
            } // end of catch

            return ReturnValue;
        } // end of public string Get()
    } // end of public class AccountController : ControllerBase
} // end of namespace namespace WebAPITest6.Controllers
//===================================================================================================
// end of AccountController.cs
//===================================================================================================
