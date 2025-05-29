//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : WeatherForecastController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/28 17:30 建立於 D:\Golden\Project\VS2022\WebAPITest2 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
namespace WebAPITest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public class Person 
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        public              Person person;
        private readonly    ILogger<WeatherForecastController> _logger;
//
        public WeatherForecastController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<WeatherForecastController> logger)
        {
            SystemTools.SetClientIP(httpContextAccessor);
//
            _logger     =   logger;
        } // end of public WeatherForecastController(IHttpContextAccessor httpContextAccessor, ... )

        [HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        public string Get()
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            { 
                iLog.WriteLog( Static.httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, null );
                person = new Person( "gg", 18 );
                DataTools.SetResponseResult<Person>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], person );
                iLog.WriteLog( iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                iLog.Log.except = except;
                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                //
                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
                //return OK(APIResult);
            }
//
            return JsonConvert.SerializeObject( APIResult );
        } // end of public IEnumerable<WeatherForecast> Get()
    } // end of public class WeatherForecastController : ControllerBase
} // end of namespace WebAPITest2.Controllers
//===================================================================================================
// end of WeatherForecastController.cs
//===================================================================================================
