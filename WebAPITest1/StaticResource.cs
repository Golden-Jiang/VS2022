//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : StaticResource.cs
// Description   : 定義程式執行時所有公共靜態資源
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/19 16:55 建立於 D:\Golden\Project\VS2022\WebAPI_Test_1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
//
// iit SDK 
//
using iitSystemWeb;
using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPI_Test_1
{
    public class Static
    {
        public static readonly object _lock = new object(); 
        public static string SystemName = "";
        public static string HostIP = "";
        public static IConfiguration config = null;
        public static IHttpContextAccessor httpContextAccessor = null;
    } // end of public class Static
    public class APIError
    {
        public const string E1000   =   "未定義的 API";       
        public const string E1001   =   "遺失 GET 參數";        
        public const string E1002   =   "GET 參數錯誤";        
        public const string E2000   =   "資料不存在";        
        public const string E2001   =   "已超過當天取號次數";        
        public const string E2002   =   "取號失敗";        
        public const string E2003   =   "分行營業狀態為已休息中";        
        public const string E2004   =   "本日無取號記錄";        
        public const string E2005   =   "帳號不存在";        
        public const string E2006   =   "已超過每次預填單數";        
        public const string E2007   =   "分行目前為非營業中";        
        public const string E2008   =   "新增重複資料錯誤";        
        public const string E2009   =   "資料錯誤";        
        public const string E20091  =   "外幣帳號";        
        public const string E20092  =   "虛擬帳號";        
        public const string E2101   =   "超過次數";        
        public const string E3001   =   "登入失敗";        
        public const string E3002   =   "登入分行失敗";        
        public const string E3003   =   "登入分行失敗,不在人力支援表中";        
        public const string E8500   =   "API 作業愈時";        
        public const string E8501   =   "API 作業錯誤";        
    } // end of APIError
//
    [Serializable]
    public class iitAPIResultClass
    {
        public string   RespCode;
        public string   RespDesc;
        public object   RespData;
        public string   RequestDateTime;
        public string   RespDateTime;
//
        public iitAPIResultClass()
        {
            RespCode        =   iitMSG.CODE.HTTP.UNKNOW_ERROR;
            RespDesc        =   "交易失敗";
            RespData        =   null;
            RequestDateTime =   DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" );
            RespDateTime    =   "";
        } // end of public APIResultClass()
    } // end of APIResultClass
} // end of namespace WebAPI_Test_1
//===================================================================================================
// end of StaticResource.cs
//===================================================================================================
