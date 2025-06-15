using MVCWeb1.Interface;

namespace MVCWeb1.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private readonly IList<Student> _studentsList;

        // 建構式
        public MockStudentRepository()
        {
            _studentsList   =   new List<Student>()
            { 
                new Student() { ID = 1, Name = "張三", ClassName = "一年級", EMail = "a@gmail.com" },
                new Student() { ID = 2, Name = "李四", ClassName = "二年級", EMail = "b@gmail.com" },
                new Student() { ID = 3, Name = "王二", ClassName = "三年級", EMail = "c@gmail.com" },
            };
        }

        public Student GeStudent( int id )
        {
            return _studentsList.FirstOrDefault( p => p.ID == id );
        }
    }
}
