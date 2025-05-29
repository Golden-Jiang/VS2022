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
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
//
// iit SDK 
//
using WebAPITest1.Models;
using iitLogWeb;
using iitDataWeb;
using iitMSGWeb;
using iitSystemWeb;
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
       private readonly     DBContext _DBContext;
       private readonly     IHttpContextAccessor _httpContextAccessor;

       public BranchController(IHttpContextAccessor httpContextAccessor, DBContext dBContext)
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
        // GET: <BranchController>
        [HttpGet]
        public string GetRecord([FromQuery(Name = "TxCode")]string TxCode)
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            { 
                var result = (from a in _DBContext.Branches
                              select a).FirstOrDefault();

                DataTools.SetResponseResult<Branch>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                iLog.WriteLog( $"TcCode={TxCode}-{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _httpContextAccessor );
            }
            catch( Exception except )
            {
                iLog.Log.except = except;
                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor );
                //
                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
                //return OK(APIResult);
            }
//
            return JsonConvert.SerializeObject( APIResult );
        } // end of public Branch GetRecord()

        // GET <BranchController>/5
        [HttpGet( "{id}" )]
        public string Get( int id )
        {
            return "value";
        }

        // POST api/<BranchController>
        public class Tel 
        {
            [Required]
            public string TeleNo { get; set;} 
            public string Name { get; set;}
        }
        [HttpPost]
        public void Post( [FromQuery(Name = "TxCode")]string TxCode, [FromBody] Tel ss )
        {
            ILog iLog =   new ILog();
            //
            iLog.WriteLog( $"TxCode={TxCode}-{ss}-{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _httpContextAccessor );
        } // end of public void Post( string TxCode, [FromBody] string value )

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
