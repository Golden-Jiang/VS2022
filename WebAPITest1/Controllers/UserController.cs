//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : WeatherForecastController.cs
// Description   : 系統啟動進入程式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/19 16:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Extensions;
//
// iit SDK 
//
using iitSystemWeb;
using iitMSGWeb;
using iitDataWeb;
using iitLogWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly    ILogger<UserController> _logger;
//
        public UserController(ILogger<UserController> logger)
        {
//
            Static.httpContextAccessor.HttpContext.Items[ "ClientIP" ] =   Utility.GetClientIP(Static.httpContextAccessor);
            _logger     =   logger;
        } // end of public public UserController(ILogger<UserController> logger)
//
        [HttpGet]
        public string Get()
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            { 
                iLog.WriteLog(Static.httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG);
                Utility.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], "" );
                iLog.WriteLog( iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                iLog.Log.except =   except;
                iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                //
                Utility.SetResponseResult<string>( APIResult, "8501", APIError.E8501, except.Message );
            }
//
            var response = JsonConvert.SerializeObject( APIResult );
            return response;
        } // end of public IEnumerable<WeatherForecast> Get()
    } // end of public class WeatherForecastController : ControllerBase
} // end of namespace WebAPI_Test_1.Controllers
//===================================================================================================
// end of User.cs
//===================================================================================================
