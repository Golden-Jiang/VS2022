namespace WebAPITest5.Service
{
    public interface ILog1
    {
        string Message { get;set; }
        void WriteLog(string message);
    }
    public class Logger : ILog1
    { 
        public string Message { get; set; }
        public Logger( string msg )
        { 
            this.Message = msg;
        }

        public void WriteLog(string msg)
        {
            Console.WriteLine( this.Message + msg);
        }
    }
}
