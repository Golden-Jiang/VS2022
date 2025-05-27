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
namespace WebAPI_Test_3
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
        //
        //public static string GetSystemName( IConfiguration _config )
        //{
        //    return _config.GetValue<string>( "System:SystemName" );
        //} // end of GetSystemName()
        ////
        //public static void SetClientIP( IHttpContextAccessor httpContextAccessor )
        //{
        //    Static.httpContextAccessor.HttpContext.Items [ "ClientIP" ] = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        //} // end of GetClientIP( ... )
        ////
        //public static string GetHostIP()
        //{
        //    string ReturnValue = "";
        //    //
        //    try
        //    {
        //        IPHostEntry iphostentry = Dns.GetHostEntry(Dns.GetHostName());    // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
        //                                                                          //
        //        foreach( IPAddress ipaddress in iphostentry.AddressList )          // 檢查所有 IP 位址
        //        {
        //            if( ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )  // 只取得IP V4的Address
        //            {
        //                ReturnValue = ipaddress.ToString();
        //                break;
        //            } // end of if( ipaddress.AddressFamily ... )
        //        } // end of foreach( IPAddress ipaddress in iphostentry.AddressList )   
        //    } // end of try
        //    catch
        //    {
        //    } // end of catch
        //      //
        //    return ReturnValue;
        //} // end of GetHostIP( ... )
        ////
        //public static void SetResponseResult<T>( iitAPIResultClass APIResult, string RespCode, string RespDesc, T RespData )
        //{
        //    APIResult.RespCode = RespCode;
        //    APIResult.RespDesc = RespDesc;
        //    APIResult.RespData = RespData;
        //    //APIResult.RespData       =   JsonConvert.SerializeObject(RespData);
        //    APIResult.RespDateTime = DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" );
        //} // end of SetResponseResult( ... )
    } // end of public class Utility
} // end of namespace WebAPI_Test_2
//===================================================================================================
// end of Utility.cs
//===================================================================================================
