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
using iitDataWeb;
using iitLogWeb;
using System.Linq.Dynamic.Core;
using WebAPITest6.Models;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public interface INameable
    {
        string Name { get; }
    }

    public interface IRepository<T> : INameable
    {
        T Select( string KeyValue ){ return default(T); }
        T Select( string KeyValue1, string KeyValue2 ){ return default(T); }
        T Select( string KeyValue1, string KeyValue2, string KeyValue3 ){ return default(T); }
        void Insert( T objData ){;}
        void Update( T objData ){;}
        void Delete( string KeyValue ){;}
    } // end of public interface IRepository<T> : INameable

    public interface IWebAPIRepository<T> : INameable
    {
        T Select( string TableName, string con1, string para1 ){ return default(T); }
        T Select( string KeyValue1, string KeyValue2 ){ return default(T); }
        void Insert( string TableName, T objData ){;}
        void Update( string TableName, string KeyCondition, string Key, T objData ){;}
        void Delete( string KeyValue ){;}
    } // end of public interface IRepository<T> : INameable
} // end of namespace WebAPITest6
//===================================================================================================
// end of IWebAPIRepository.cs
//===================================================================================================
