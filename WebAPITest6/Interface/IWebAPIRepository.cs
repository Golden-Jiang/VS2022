//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : IWebAPIRepository.cs
// Description   : Interface of IWebAPIRepository
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:00 建立於 D:\Golden\Project\VS2022\WebAPITest6 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using iitLogWeb;
using System.Linq.Dynamic.Core;
using WebAPITest6.Models;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public interface IWebAPIRepository
    {
        T GetUsePara1<T>( string TableName, string con1, string para1 );
        void Insert<T>( string TableName, T objData );
        void Update<T>( string TableName, string KeyCondition, string Key, T objData );
    } // end of public interface IWebAPIRepository
} // end of namespace WebAPITest6
//===================================================================================================
// end of IWebAPIRepository.cs
//===================================================================================================
