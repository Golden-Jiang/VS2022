using iitDataWeb;
using iitLogWeb;
using iitSystemWeb;
using iitToolsWeb;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

//using System.Linq.Dynamic.Core;
using System.Reflection;
using WebAPITest6.Models;

namespace WebAPITest6.Repository
{
    public partial class WebTeleNoRepository : IRepository<WebTeleNo>
    {
        public string Name => "WebTeleNo";

        public readonly IHttpContextAccessor    _httpContextAccessor;
        public readonly DBContext               _DBContext;
        public readonly IiitLog                 _Log;
        private readonly string                 _ClientIP;

        // 類別建構子
        public WebTeleNoRepository( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IiitLog Log ) 
        { 
            _httpContextAccessor    =   httpContextAccessor;
            _DBContext              =   dBContext;
            _Log                    =   Log; 
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );
        }

        public WebTeleNo Select( string TeleNo )
        {
            string  SQLCommand;

            try
            {
                var result  =  _DBContext.WebTeleNo.FirstOrDefault( p => p.TeleNo == TeleNo );
                SQLCommand  =   $"SELECT * FROM WebTeleNo WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

                return result;
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
            } // end of catch

            return default(WebTeleNo);
        } // end of IEnumerable<WebTeleNo> GetSingle<WebTeleNo>( ... )

        public void Insert( WebTeleNo objData )
        {
            _DBContext.WebTeleNo.Add( objData );
            _DBContext.SaveChanges();
        }
        public void Update( WebTeleNo objData )
        {
            _DBContext.WebTeleNo.Update( objData );
            _DBContext.SaveChanges();
        }
    } // end of public class WebTeleNoRepository 
} // end of namespace WebAPITest6.Repository
