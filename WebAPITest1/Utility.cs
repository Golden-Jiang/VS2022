//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : Utility.cs
// Description   : 系統所有公共函式
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/19 16:30 建立於 D:\Golden\Project\VS2022\WebAPI_Test_1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Configuration;
using System.Net;
//
// iit SDK 
//
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_1
{
    public class Utility
    {
        public static void InitStatic( IConfiguration config, IHttpContextAccessor httpContextAccessor )
        {
            if (Static.config == null)
            {
                lock (Static._lock)
                {
                    Static.config = config;
                }
            } // end of if( Static.config == null )
//
            if (Static.httpContextAccessor == null)
            {
                lock (Static._lock)
                {
                    Static.httpContextAccessor = httpContextAccessor;
                }
            } // end of if( Static.httpContextAccessor == null )
//
            if (Static.SystemName == "")
            {
                lock (Static._lock)
                {
                    Static.SystemName = Utility.GetSystemName(Static.config);
                }
            } // end of if( Static.SystemName == "" )
              //           
            if (Static.HostIP == "")
            {
                lock (Static._lock)
                {
                    Static.HostIP = Utility.GetHostIP();
                }
            }
        } // end of GetSystemName()
//
        public static string GetSystemName(IConfiguration _config)
        {
            return _config.GetValue<string>("System:SystemName");
        } // end of GetSystemName()
          //
        public static string GetClientIP(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        } // end of GetClientIP( ... )
        //
        public static string GetHostIP()
        {
            string ReturnValue = "";
            //
            try
            {
                IPHostEntry iphostentry = Dns.GetHostEntry(Dns.GetHostName());    // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
                                                                                  //
                foreach (IPAddress ipaddress in iphostentry.AddressList)          // 檢查所有 IP 位址
                {
                    if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)  // 只取得IP V4的Address
                    {
                        ReturnValue = ipaddress.ToString();
                        break;
                    } // end of if( ipaddress.AddressFamily ... )
                } // end of foreach( IPAddress ipaddress in iphostentry.AddressList )   
            } // end of try
            catch  
            {
            } // end of catch
              //
            return ReturnValue;
        } // end of GetHostIP( ... )
//
        public static void SetResponseResult<T>( iitAPIResultClass APIResult, string RespCode, string RespDesc, T RespData )
        {
            APIResult.RespCode       =   RespCode;
            APIResult.RespDesc       =   RespDesc;
            APIResult.RespData       =   RespData;
            //APIResult.RespData       =   JsonConvert.SerializeObject(RespData);
            APIResult.RespDateTime   =   DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" ); 
        } // end of SetResponseResult( ... )
    } // end of public class Utility
} // end of namespace WebAPI_Test_1
//===================================================================================================
// end of Utility.cs
//===================================================================================================
