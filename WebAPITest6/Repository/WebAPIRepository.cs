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
using WebAPITest6.DTO;

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
                //var result1 =   from a in _DBContext.WebTeleNo
                //                where a.TeleNo == TeleNo
                //                select a;
                //{ 
                //    a.TeleNo,  
                //    a.RecordControl,
                //    a.RecordControlDateTime,
                //    a.LastAccessTime,
                //    a.Ip,
                //    a.AccountNo, 
                //    a.TotalGetCallNo, 
                //    a.TotalForm 
                //}).FirstOrDefault();
                //var result1 = _DBContext.WebTeleNo.FirstOrDefault<WebTeleNo>( p => p.TeleNo == TeleNo );  

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

        public void Update<T>( string TableName, string KeyCondition, string Key1, string Key2, T objData )
        {
            string  SQLCommand;

            try
            {
                var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
                if( dbSetProperty != null )
                {
                    var dbSet   =   dbSetProperty.GetValue( _DBContext );
                    var entity  =   ( ( IQueryable<T> )dbSet ).Where<T>( $"{KeyCondition}", Key1, Key2 );

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <param name="KeyCondition"></param>
        /// <param name="Key"></param>
        public void Delete<T>( string TableName, string KeyCondition, string Key ) where T : class
        {
            string SQLCommand = "";

            var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
            if( dbSetProperty != null )
            {
                var dbSet   =   dbSetProperty.GetValue( _DBContext ) as IQueryable<T>;
                var results =   dbSet.Where<T>( $"{KeyCondition}", Key ).FirstOrDefault();

                if( results != null ) 
                { 
                    _DBContext.Set<T>().Remove( results );
                    _DBContext.SaveChanges(); // 儲存變更
                }

                SQLCommand = $"DELETE FROM {TableName} WHERE {KeyCondition}='{Key}'";
                _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );
            } // end of if( dbSetProperty != null )
        } // end of public void Delete<T>

        public void Delete<T>( string TableName, string KeyCondition, string Key1, string Key2 ) where T : class
        {
            string SQLCommand = "";

            var dbSetProperty   =   typeof( DBContext ).GetProperty( TableName, BindingFlags.Public | BindingFlags.Instance );
            if( dbSetProperty != null )
            {
                var dbSet   =   dbSetProperty.GetValue( _DBContext ) as IQueryable<T>;
                var results =   dbSet.Where<T>( $"{KeyCondition}", Key1, Key2 ).FirstOrDefault();

                if( results != null ) 
                { 
                    _DBContext.Set<T>().Remove( results );
                    _DBContext.SaveChanges(); // 儲存變更
                }

                SQLCommand = $"DELETE FROM {TableName} WHERE {KeyCondition}='{Key1}-{Key2}'";
                _Log.WriteLog( $"SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );
            } // end of if( dbSetProperty != null )
        } // end of public void Delete<T>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TeleNo"></param>
        /// <returns></returns>
        public WebAPIDTO.WebTeleNoJoinQRCodeDTO WebTeleNoJoinQRCode( string TeleNo )
        {
            string SQLCommand = "";

            // 判斷新電話號碼是否存在
            //var result3 = (from a in _DBContext.WebTeleNo
            //              join b in _DBContext.QRCode
            //              on a.QRCode equals b.QRCode
            //              where a.TeleNo == TeleNo
            //              select new
            //              {
            //                a.CustID, a.QRCode, b.QRCodeStratDate, b.QRCodeEndDate, b.ServiceStatus
            //              }).ToList();

            WebAPIDTO.WebTeleNoJoinQRCodeDTO Result =
                    ( from a in _DBContext.WebTeleNo
                    join b in _DBContext.QRCodes
                    on a.QRCode equals b.QRCode
                    where a.TeleNo == TeleNo
                    select new WebAPIDTO.WebTeleNoJoinQRCodeDTO
                    {
                        TeleNo = a.TeleNo,
                        CustID = a.CustID,
                        QRCode = a.QRCode,
                        QRCodeStratDate = b.QRCodeStratDate,
                        QRCodeEndDate = b.QRCodeEndDate,
                        ServiceStatus = b.ServiceStatus
                    }).FirstOrDefault();

            SQLCommand = $"SELECT a.TeleNo, a.CustID, a.QRCode, b.QRCodeStratDate, b.QRCodeEndDate, b.ServiceStatus FROM WebTeleNo a LEFT JOIN QRCode b ON b.QRCode=a.QRCode " +
                            $"WHERE a.TeleNo='{TeleNo}'";
            _Log.WriteLog( "SQLCommand={SQLCommand}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

            return Result;
        } // end of public WebAPIDTO.WebTeleNoJoinQRCodeDTO WebTeleNoJoinQRCode( string TeleNo )

        /// <summary>
        /// 取得當日變更電話號碼次數
        /// </summary>
        /// <returns></returns>
        public int CaculateTodayChangeCustIDTeleNoCount( string CustID )
        {
            int ReturnValue =   0; 
            string SQLCommand = "";
            var today       =   DateTime.Parse( DateTime.Now.ToString( "yyyy/MM/dd" ) );
            var tomorrow    =   today.AddDays( 1 );

            ReturnValue =   ( from a in _DBContext.ChangeCustIDTeleNo
                              where a.CustID == CustID && ( a.CreateTime >= today && a.CreateTime < tomorrow )
                              select a ).Count();

            SQLCommand  =   $"SELECT * FROM ChangeCustIDTeleNo WHERE CustID='{CustID}' AND CAST( CreateTime AS Date )=CAST( GETDATE() AS Date ) " +
                            $"AND Process=2 AND Result=1";
            _Log.WriteLog( $"SQLCommand={SQLCommand}, ChangeCount={ReturnValue}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, _ClientIP );

            return ReturnValue;
        } // end of CaculateTodayChangeCustIDTeleNoCount()
    } // end of public calss WebAPIRepository
} // end of namespace WebAPITest6
//===================================================================================================
// end of WebAPIRepository.cs
//===================================================================================================
