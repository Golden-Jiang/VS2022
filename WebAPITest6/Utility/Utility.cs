//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/27 09:30 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http.Extensions;
using WebAPITest6.Models;
 
using iitSystemWeb;
using iitLogWeb;
using iitToolsWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6
{
    public class Utility
    {
        public static void SetClientEnvironment1( IHttpContextAccessor httpContextAccessor, ref IHttpContextAccessor _httpContextAccessor, 
                                                 DBContext dBContext, ref DBContext _DBContext, IiitLog Log, ref IiitLog _Log, ref string _ClientIP )
        {
            _httpContextAccessor    =   httpContextAccessor;
            _DBContext              =   dBContext;
            _Log                    =   Log;
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );  // 一定要先執行
             
            if( ! Static.SystemStartLog )
            { 
                _Log.WriteLog( Static.SystemStartMesage, iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString());
                Static.SystemStartLog = true;
            } // end of if( ! Static.SystemStartLog )

            _Log.WriteLog( _httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), 
                           iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
        } // end of SetClientEnvironment( ... )
    } // end of public class Utility
} // end of namespace WebAPITest6
//===================================================================================================
// end of Utility.cs
//===================================================================================================
