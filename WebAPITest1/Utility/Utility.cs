//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/27 09:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using System.Net;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitDataWeb;
using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_1
{ 
    public class Utility
    {
        public static void Start(IConfiguration config)
        {
            ILog iLog =   new ILog();
            
            // 系統啟動
            SystemTools.SystemStart(config);

            // 初始化系統公用訊息
            iitMSG.Start();

            iLog.WriteLog( $"WebAPI System Start at {DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" )}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_HIGHEST );
        } // end of public static void InitStatic( IConfiguration config, ...)
    } // end of public class Utility
} // end of namespace WebAPI_Test_1
//===================================================================================================
// end of Utility.cs
//===================================================================================================
