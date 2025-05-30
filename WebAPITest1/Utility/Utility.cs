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
        /// <summary>
        /// 系統啟動時, 設定 iitSDKWeb 相關全域資料與環境
        /// </summary>
        /// <param name="config"></param>
        public static void Start(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            ILog iLog =   new ILog( null );
            
            // 系統啟動
            iitSystemTools.SystemStart(config);

            // 初始化系統公用訊息
            iitMSG.Start();

            iLog.WriteLog( $"WebAPI System Start at {DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST );
        } // end of public static void InitStatic( IConfiguration config, ...)

        public static void SetClientEnvironment( IHttpContextAccessor httpContextAccessor, ref IHttpContextAccessor _httpContextAccessor, 
                                                 DBContext dBContext, ref DBContext _DBContext )
        {
            iitSystemTools.SetClientIP( httpContextAccessor );  // 一定要先執行
             
            ILog iLog =   new ILog( httpContextAccessor );
             
            _DBContext              =   dBContext;
            _httpContextAccessor    =   httpContextAccessor;
             
            iLog.WriteLog( _httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, null );
        } // end of SetClientEnvironment(IHttpContextAccessor httpContextAccessor, DBContext dBContext)
    } // end of public class Utility
} // end of namespace WebAPITest1
//===================================================================================================
// end of Utility.cs
//===================================================================================================
