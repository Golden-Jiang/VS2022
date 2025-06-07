//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : WebAPIRepository.cs
// Description   : Repository of WebAPIRepository
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/06/05 11:30 建立於 D:\Golden\Project\VS2022\WebAPITest6\Repository 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using WebAPITest6.Models;

using iitSystemWeb;
using iitLogWeb;
using iitMSGWeb;
using iitToolsWeb;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6 
{
    public class WebAPIRepository 
    {
        public readonly IHttpContextAccessor    _httpContextAccessor;
        public readonly DBContext               _DBContext;
        public readonly IiitLog                 _Log;
        private readonly string                 _ClientIP;

        public WebAPIRepository( IHttpContextAccessor httpContextAccessor, DBContext dBContext, IiitLog Log )
        {
            _httpContextAccessor    =   httpContextAccessor;
            _DBContext              =   dBContext;
            _Log                    =   Log; 
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );
        } // end of public WebAPIRepository

        public T Select<T>( string TableName, string con1, string para1 )
        {
            string  SQLCommand;
            try
            { 
                var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
                if( dbSetProperty != null )
                {
                    var dbSet   =   dbSetProperty.GetValue( _DBContext ) as IQueryable<T>;
                    var results =   dbSet.Where<T>( $"{con1}", para1 );

                    SQLCommand = $"SELECT FROM {TableName} condition={con1}-{para1}";
                    _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

                    return results.FirstOrDefault();
                } // end of if( dbSetProperty != null )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
            } // end of catch

            return default(T);
        } // end of public WebTeleNo GetUseTeleNo ... )

        public T Select<T>( string TableName, string con1, string para1, string para2 )
        {
            string  SQLCommand;
            try
            { 
                var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
                if( dbSetProperty != null )
                {
                    var dbSet   =   dbSetProperty.GetValue( _DBContext ) as IQueryable<T>;
                    var results =   dbSet.Where<T>( $"{con1}", para1, para2 );

                    SQLCommand = $"SELECT FROM {TableName} condition={con1}-{para1}-{para2}";
                    _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

                    return results.FirstOrDefault();
                } // end of if( dbSetProperty != null )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
            } // end of catch

            return default(T);
        } // end of public WebTeleNo GetUseTeleNo ... )

        public void Insert<T>( string TableName, T objData ) where T : class
        {
            string  SQLCommand;

            try
            {
                _DBContext.Set<T>().Add( objData );
                _DBContext.SaveChanges(); // 儲存變更

                SQLCommand = $"INSERT {TableName} objData={JsonConvert.SerializeObject(objData)}";
                _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

                _DBContext.SaveChanges();
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
            } // end of catch
        } // end of public void Insert<T>( ... )

        public void Update<T>( string TableName, string KeyCondition, string Key, T objData )
        {
            string  SQLCommand;

            try
            {
                var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
                if( dbSetProperty != null )
                {
                    var dbSet   =   dbSetProperty.GetValue( _DBContext );
                    var entity  =   ( ( IQueryable<T> )dbSet ).Where<T>( $"{KeyCondition}", Key );

                    if( entity != null )
                    {
                        // 更新屬性
                        foreach( var prop in objData.GetType().GetProperties() )
                        {
                            var entityProp = entity.GetType().GetProperty( prop.Name );
                            if( entityProp != null && entityProp.CanWrite )
                            {
                                entityProp.SetValue( entity, prop.GetValue( objData ) );
                            }
                        } // end of foreach( var prop in objData.GetType().GetProperties() )

                        SQLCommand = $"UPDATE {TableName} objData={JsonConvert.SerializeObject(objData)}";
                        _Log.WriteLog( $"{SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

                        _DBContext.SaveChanges();
                    } // end of if( entity != null )
                } // end of if( dbSetProperty != null )
            } // end of try
            catch( Exception except )
            {
                _Log.except =   except;
                _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );
            } // end of catch
        } // end of public void Update<T>( ... )
    } // end of public calss WebAPIRepository
} // end of namespace WebAPITest6
//===================================================================================================
// end of WebAPIRepository.cs
//===================================================================================================
