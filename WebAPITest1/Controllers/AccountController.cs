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
//
// iit SDK 
//
//using iitSystemWeb;
//using iitLogWeb;
//using iitDataWeb;
//using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest1.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string [] { "value1", "value2" };
        }

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
