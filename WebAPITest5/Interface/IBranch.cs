using WebAPI_Test_5.Models;

namespace WebAPITest5.Interface
{
    public partial class IWebAPI
    {
        public interface IBranch
        {
            IEnumerable<Branch> GetOne( string Filter );
            IEnumerable<Branch> GetMany();
        }
    }
}
