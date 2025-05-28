//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : BranchController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/28 17:30 建立於 D:\Golden\Project\VS2022\WebAPITest1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using WebAPITest1.Models;
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
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebAPITest1.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class BranchController : ControllerBase
    {
       private readonly DBContext _DBContext;

       public BranchController(DBContext dBContext)
       {
            _DBContext = dBContext;
        }

        // GET: <BranchController>
        //[HttpGet]
        //public Branch Get()
        //{
        //    return _DBContext.Branch.FirstOrDefault();
        //}

        // GET <BranchController>/5
        [HttpGet( "{id}" )]
        public string Get( int id )
        {
            return "value";
        }

        // POST api/<BranchController>
        [HttpPost]
        public void Post( [FromBody] string value )
        {
        }

        // PUT api/<BranchController>/5
        [HttpPut( "{id}" )]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE api/<BranchController>/5
        [HttpDelete( "{id}" )]
        public void Delete( int id )
        {
        }
    } // end of public class BranchController : ControllerBase
} // end of WebAPITest1.Controllers
//===================================================================================================
// end of BranchController.cs
//===================================================================================================
