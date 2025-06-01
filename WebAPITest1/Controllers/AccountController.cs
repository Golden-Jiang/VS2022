//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/28 20:00 建立於 D:\Golden\Project\VS2022\WebAPITest1 目錄 
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
using iitMSGWeb;
using iitToolsWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest1.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class AccountController : ControllerBase
    {
       public readonly DBContext _DBContext;
       public readonly IHttpContextAccessor _httpContextAccessor;

       public AccountController( IHttpContextAccessor httpContextAccessor, DBContext dBContext )
       {
            Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext);
        } // end of public BranchController(IHttpContextAccessor
        
        // GET: http://url/Branch?Query1=.....
        [HttpGet]
        public string Get()
        {
            string              ReturnValue = "";
            string              TxCode = "";
            string              TmpString1 = "", TmpString2 = "", TmpString3 = "", TmpString4 = "";
            iitAPIResultClass   APIResult = new iitAPIResultClass();
            ILog                iLog =   new ILog( _httpContextAccessor );
 
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
                                ReturnValue =   AccountService.GetAccountFromTeleNo( TmpString1, _DBContext, _httpContextAccessor );
                            else
                                ReturnValue = AccountService.GetAccountFromTeleNoNetBank( TmpString1, TmpString2, _DBContext, _httpContextAccessor  );
                            break;
                        case    "SA013001F"      :   // 依據電話號碼讀取外幣綁定常用帳號
                            if( ( ReturnValue = iitDataTools.CheckQuery( _httpContextAccessor, APIResult, "TeleNo", ref TmpString1 ) ) != "" )
                                break;
                            
                            ReturnValue = AccountService.GetForexAccountFromTeleNo( TmpString1, _DBContext, _httpContextAccessor  );
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
//
                            iLog.WriteLog( APIResult.RespDesc, iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                            break;
                    } // end of switch( TxCode )
//
                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                iLog.Log.except  =   except;
                iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
//
                iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                ReturnValue =   JsonConvert.SerializeObject( APIResult );
            } // end of catch
//
            return ReturnValue;
        } // end of public string Get()

        // GET api/<ValuesController>/5
        [HttpGet( "{id}" )]
        public string Get( int id )
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post( [FromBody] string value )
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut( "{id}" )]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete( "{id}" )]
        public void Delete( int id )
        {
        }
    } // end of public class ValuesController : ControllerBase
} // end of namespace WebAPITest1.Controllers
//===================================================================================================
// end of AccountController.cx
//===================================================================================================
