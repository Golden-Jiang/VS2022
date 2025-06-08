//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : IAccountService.cs
// Description   : Interface of AccountService
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:00 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using iitLogWeb;
using WebAPITest6.Models;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public interface IAccountService
    {
        public string GetAccountFromTeleNo( string TeleNo );
        public string GetForexAccountFromTeleNo( string TeleNo );
    } // end of public interface IAccountService
} // end of namespace WebAPITest6
//===================================================================================================
// end of IAccountService.cs
//===================================================================================================
