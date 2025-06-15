using MVCWeb1.Models;

namespace MVCWeb1.Interface
{
    public interface IStudentRepository
    {
        Student GeStudent( int id );
    }
}
