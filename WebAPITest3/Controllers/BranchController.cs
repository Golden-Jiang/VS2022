using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Test_3.Models;
using iitLogWeb;
using iitSystemWeb;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_Test_3.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class BranchController : ControllerBase
    {
       public readonly IiitLog _Log;
       public readonly DBContext _DBContext;
       public readonly IHttpContextAccessor _httpContextAccessor;

       public BranchController( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IiitLog Log )
       {
            _Log = Log; 
            Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext, _Log );
        }

        // GET: <BranchController>
        [HttpGet]
        public Branch Get()
        {
            _Log.WriteLog( "aaa", iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString() );
            return _DBContext.Branch.FirstOrDefault();
        }

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
    }
}
