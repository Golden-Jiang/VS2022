
using iitLogWeb;
using System.Linq.Dynamic.Core;
using WebAPI_Test_5;
using WebAPI_Test_5.Models;
using WebAPITest5.Interface;

namespace WebAPITest5.Service
{
    public partial class WebAPIService
    { 
        public class BranchService : IWebAPI.IBranch
        {
           public readonly IiitLog _Log;
           public readonly DBContext _DBContext;
           public readonly IHttpContextAccessor _httpContextAccessor;

           public BranchService( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IiitLog Log )
           {
               _httpContextAccessor = httpContextAccessor;
               _DBContext = dBContext;
               _Log = Log; 
                //Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext, _Log );
            }

            public IEnumerable<Branch> GetOne( string BranchID )
            {
                var result1 = _DBContext.Branch.Where<Branch>( "BranchID = @0", BranchID );
                return result1.ToList();
            }

            public IEnumerable<Branch> GetMany()
            {
                return _DBContext.Branch;
            }
        }
    }
}
