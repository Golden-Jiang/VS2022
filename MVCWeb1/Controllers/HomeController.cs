using MVCWeb1.Interface;
using MVCWeb1.Models;

namespace MVCWeb1.Controllers
{
    public class HomeController
    {
        private readonly IStudentRepository _studentsRepository;
        public HomeController( IStudentRepository studentsRepository )
        {
            _studentsRepository = studentsRepository;
        }

        public string Index()
        {
            return _studentsRepository.GeStudent( 1 ).Name;
        }
    }
}
