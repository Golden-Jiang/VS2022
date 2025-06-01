//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/27 09:30 建立於 D:\Golden\Project\VS2022\WebAPITest1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
//using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using WebAPITest1.Models;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitMSGWeb;
using iitToolsWeb;
using iitDataWeb;
using Microsoft.AspNetCore.Http;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest1
{ 
    public class Utility
    {
        public static void SetClientEnvironment( IHttpContextAccessor httpContextAccessor, ref IHttpContextAccessor _httpContextAccessor, 
                                                 DBContext dBContext, ref DBContext _DBContext, IiitLog _Log )
        {
            string  Message     =   httpContextAccessor.HttpContext?.Request?.GetEncodedUrl();

            iitSystemTools.SetClientIP( httpContextAccessor );  // 一定要先執行
             
            _DBContext              =   dBContext;
            _httpContextAccessor    =   httpContextAccessor;
             
            if( ! Static.SystemStartLog )
            { 
                _Log.WriteLog( Static.SystemStartMesage, iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString());
                Static.SystemStartLog = true;
            }

            _Log.WriteLog( Message, iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST, _httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString());
        } // end of SetClientEnvironment(IHttpContextAccessor httpContextAccessor, DBContext dBContext)
    } // end of public class Utility
} // end of namespace WebAPITest1
//===================================================================================================
// end of Utility.cs
//===================================================================================================
