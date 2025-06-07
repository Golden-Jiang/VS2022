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

using iitSystemWeb;
using iitDataWeb;
using iitLogWeb;
using iitMSGWeb;
using iitToolsWeb;
using WebAPITest6.Repository;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public class AccountService : IAccountService
    {
        private readonly WebAPIRepository       _repository;
        //private readonly IRepository<WebTeleNo> _repository;
        private readonly IHttpContextAccessor   _httpContextAccessor;
        private readonly DBContext              _DBContext;
        private readonly IiitLog                _Log;
        private readonly string                 _ClientIP;

        public AccountService( 
            IHttpContextAccessor httpContextAccessor,  
            WebAPIRepository repository, 
            DBContext dBContext, 
            IiitLog Log )
        {
            _httpContextAccessor    =   httpContextAccessor;
            _repository             =   repository;
            _DBContext              =   dBContext;
            _Log                    =   Log;
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );
            //_ClientIP               =   httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
            //Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext, _Log );
        }

        public string GetAccountFromTeleNo( string TeleNo )
        {
            string                  SQLCommand = "";
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
                    //var result1 = _DBContext.WebTeleNo.FirstOrDefault<WebTeleNo>( p => p.TeleNo == TeleNo );  

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
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    _Log.except =   except;
                    _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNo( ... )
    } // end of public class AccountService : IAccountService
} // end of namespace WebAPITest6
//===================================================================================================
// end of AccountService.cs
//===================================================================================================
