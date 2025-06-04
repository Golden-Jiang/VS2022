using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Test_5.Models;
using WebAPITest5.Interface;
using iitLogWeb;
using iitSystemWeb;
using static WebAPITest5.Service.WebAPIService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_Test_5.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IWebAPI.IBranch _BranchService;
        private readonly DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IiitLog _Log;

        public BranchController( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IWebAPI.IBranch BranchService, IiitLog Log )
        {
            _BranchService  = BranchService;
            _Log = Log; 
            Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext, _Log );
        }

        // GET: <BranchController>
        [HttpGet]
        public IEnumerable<Branch> Get()
        {
            //_Log.WriteLog( "aaa", iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString() );
            return _BranchService.GetMany();
        }

        // GET <BranchController>/5
        [HttpGet( "{BranchID}" )]
        public IEnumerable<Branch> Get( string BranchID )
        {
            return _BranchService.GetOne( BranchID );
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
