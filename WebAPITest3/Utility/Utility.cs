//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/27 09:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_3 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using System.Net;
using WebAPI_Test_3.Models;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitToolsWeb;
using iitDataWeb;
using iitMSGWeb;
using Microsoft.AspNetCore.Http.Extensions;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_3
{
    public class Utility
    {
        /// <summary>
        /// 系統啟動時, 設定 iitSDKWeb 相關全域資料與環境
        /// </summary>
        /// <param name="config"></param>
        public static void Start( IConfiguration config )
        {
            // 系統啟動
            iitSystemTools.SystemStart( config );

            // 初始化系統公用訊息
            iitMSG.Start();

            Static.SystemStartMesage = $"WebAPI System Start at {DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}";
        } // end of public static void InitStatic( IConfiguration config, ...)

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
} // end of namespace WebAPI_Test_3
//===================================================================================================
// end of Utility.cs
//===================================================================================================
