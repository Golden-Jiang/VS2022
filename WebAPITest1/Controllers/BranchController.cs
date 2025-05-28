using Microsoft.AspNetCore.Mvc;
using WebAPI_Test_1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_Test_1.Controllers
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
    }
}
