//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : UserController.cs
// Description   : 使用者服務控制器
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/19 16:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_2 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitDataWeb;
using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly    ILogger<UserController> _logger;
        // UserController 建構函式
        public UserController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<UserController> logger)
        {
            // 檢查是否第一次呼叫 API, 若第一次則設定 API 系統用公用變數
            Utility.InitStatic( configuration, httpContextAccessor );
            //
            //Static.httpContextAccessor.HttpContext.Items[ "ClientIP" ] =   Utility.GetClientIP(Static.httpContextAccessor);
            //_logger     =   logger;
        } // end of public public UserController(ILogger<UserController> logger)
        //
        // http://localhost/WebAPI/user 使用 http GET 協定
        // 函數名自訂, 只要函數名上面有 [HttpGet] 標籤
        [HttpGet]
        public string Get()
        {
            // 公用 API 返回 JSON 物件
            iitAPIResultClass APIResult = new iitAPIResultClass();
            // 公用 Log 物件
            ILog iLog =   new ILog();
            //
            try
            { 
                // 若 iLogDebug = "1" 時, 會記錄呼叫 API 的完整 url
                iLog.WriteLog( Static.httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG);
                // 設定 API 回傳的相關資料, 此函數為泛型, APIResult.RespData 會放入回傳指定型別的資料
                Utility.SetResponseResult<string>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], "" );
                // 若 iLogDebug = "1" 時, 會記錄呼叫 API 交易成功
                iLog.WriteLog( iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                // 此函式發生任何錯誤, 均會執行此段程式碼
                iLog.Log.except = except;
                // 一定會寫入程式記錄, 並會記錄完整的 Exception
                iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                // 設定 API 回傳的相關資料, 此函數固定為 string 泛型,
                // APIResult.RespCode = "8501", APIResult.RespDesc = iitMSG.APIError.E8501, APIResult.RespData = except.Message
                Utility.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
            }
            // 回傳 APIResult 轉換為 JSON 格式
            return JsonConvert.SerializeObject( APIResult );
        } // end of public IEnumerable<WeatherForecast> Get()
    } // end of public class WeatherForecastController : ControllerBase
} // end of namespace WebAPI_Test_2.Controllers
//===================================================================================================
// end of User.cs
//===================================================================================================
