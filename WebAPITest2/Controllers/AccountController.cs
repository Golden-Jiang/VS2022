//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/28 20:00 建立於 D:\Golden\Project\VS2022\WebAPITest2 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using iitDataWeb;
using iitLogWeb;
using iitSystemWeb;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using static iitLogWeb.ILog;
using static iitMSGWeb.iitMSG;
//
// iit SDK 
//
using WebAPITest2.Models;
using iitLogWeb;
using iitDataWeb;
using iitMSGWeb;
using iitSystemWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest2.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class AccountController : ControllerBase
    {
       private readonly DBContext _DBContext;

       public AccountController(IHttpContextAccessor httpContextAccessor, DBContext dBContext)
       {
            ILog iLog =   new ILog();
            //
            _DBContext = dBContext;
            //
            SystemTools.SetClientIP(httpContextAccessor);
            //
            iLog.WriteLog( Static.httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, null );
        } // end of public BranchController(IHttpContextAccessor
        //
        // GET: api/<ValuesController>
        [HttpGet]
        public void Get()
        {
        } // end of Get()

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
} // end of namespace WebAPITest2.Controllers
//===================================================================================================
// end of AccountController.cx
//===================================================================================================
