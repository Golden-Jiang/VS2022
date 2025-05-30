//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : AccountService.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/29 12:40 建立於 D:\Golden\Project\VS2022\WebAPITest1 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPITest1.Models;
//
// iit SDK 
//
using iitSystemWeb;
using iitLogWeb;
using iitDataWeb;
using iitToolsWeb;
using System.Data;
using System.IO;
using System.Web;
using static iit.Data.iitData;
using iitMSGWeb;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest1
{
    public class AccountService
    {
       public static string GetAccountFromTeleNo( string TeleNo, DBContext _DBContext, IHttpContextAccessor httpContextAccessor )
        {
            string                  SQLCommand = "";
            DateTime                TmpDateTime1 = DateTime.Now;
            iitAPIResultClass       APIResult = new iitAPIResultClass();
            AccountData.Customer    CustomerClass = new AccountData.Customer();
            ILog                    iLog = new ILog( httpContextAccessor );
 
            try
            {
                while( true )
                { 
                    if( ! iitCheckTools.CheckTeleNo( TeleNo, APIResult ) )
                        throw new iitException( "" );
 
                    var result1 =   from a in _DBContext.WebTeleNo
                                    where a.TeleNo == TeleNo
                                    select a;
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

                    SQLCommand  =   $"SELECT * FROM WebTeleNo WHERE TeleNo='{TeleNo}' ORDER BY TeleNo";
                    iLog.WriteLog( $"{SQLCommand}, result rows={result1.Count()}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );

                    //var result11 = result1.ToList();
                    //if( result11.Count != 0 )
                    //{
                        //CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.AccountNo = result11 [ 0 ].AccountNo;
                    //    CustomerClass.TeleAccount.TotalGetCallNo = result11 [ 0 ].TotalGetCallNo.ToString();
                    //    CustomerClass.TeleAccount.TotalForm = result11 [ 0 ].TotalForm.ToString();

                    if( result1.Count() != 0 )
                    {
                        CustomerClass.TeleAccount.TeleNo            =   result1.FirstOrDefault().TeleNo;
                        CustomerClass.TeleAccount.AccountNo         =   result1.FirstOrDefault().AccountNo;
                        CustomerClass.TeleAccount.TotalGetCallNo    =   result1.FirstOrDefault().TotalGetCallNo.ToString();
                        CustomerClass.TeleAccount.TotalForm         =   result1.FirstOrDefault().TotalForm.ToString();

                        foreach( var std in result1 )
                        {
                            std.RecordControl           =   2;
                            std.RecordControlDateTime   =   TmpDateTime1;
                            std.LastAccessTime          =   TmpDateTime1;
                            std.IP                      =   httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString();
                        } // end of foreach( var std in result1 )

                        _DBContext.SaveChanges();

                        SQLCommand  =   String.Format( "Update WebTeleNo SET RecordControl=2, RecordControlDateTime='{0}', LastAccessTime='{1}', IP='{2}' WHERE TeleNo='{3}'", 
                                        TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), 
                                        httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString(), TeleNo );
                        iLog.WriteLog( "SQLCommand=" + SQLCommand, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );

                        var result2 =   from a in _DBContext.SystemParameter
                                        where a.FuncParamID == "WEBAPI" && a.ParameterCode == "0002"
                                        select a;

                        if( result2.Count() != 0 )
                            CustomerClass.TeleAccount.MaxForm   =   result2.FirstOrDefault().ParameterValue;
                        else
                            CustomerClass.TeleAccount.MaxForm    =   "10";

                        SQLCommand  =   "SELECT * FROM SystemParameter WHERE FuncParamID='WEBAPI' AND ParameterCode='0002' ORDER BY FuncParamID, ParameterCode"; 
                        iLog.WriteLog( $"SQLCommand={SQLCommand} result rows={result2.Count()}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
                    } // end of if( SQLError == string.Empty && RecordCount > 0 )
                    else
                    { 
                        WebTeleNo sp = new WebTeleNo
                        { 
                            RecordControl = 1,
                            RecordControlDateTime = TmpDateTime1,
                            Enabled = 1,
                            CreateTime = TmpDateTime1,
                            LastAccessTime = TmpDateTime1,
                            TeleNo = TeleNo,
                            AccountNo = "",
                            TotalGetCallNo = 0,
                            LastGetCallNoTime = TmpDateTime1,
                            IP = httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString(),
                            TotalForm = 0
                        };

                        _DBContext.WebTeleNo.Add( sp );
                        _DBContext.SaveChanges();

                        SQLCommand  =   String.Format( "INSERT INTO WebTeleNo ( RecordControl, RecordControlDateTime, Enabled, CreateTime, LastAccessTime, TeleNo, AccountNo, " + 
                                        " TotalGetCallNo, LastGetCallNoTime, IP, TotalForm ) VALUES ( 1, '{0}', 1, '{1}', '{2}', '{3}', '', 0, '{4}', '{5}', 0 )", 
                                        TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ),
                                        TeleNo, TmpDateTime1.ToString( "yyyy/MM/dd HH:mm:ss.fff" ), httpContextAccessor.HttpContext.Items[ "ClientIP" ].ToString() );
                        iLog.WriteLog( "SQLCommand=" + SQLCommand, iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
 
                        CustomerClass.TeleAccount.AccountNo         =   "";
                        CustomerClass.TeleAccount.TotalGetCallNo    =   "";
                        CustomerClass.TeleAccount.TotalForm         =   "";
                    } // end of else if( SQLError == string.Empty && RecordCount > 0 )
 
                    iitDataTools.SetResponseResult<AccountData.Customer>( APIResult, "0000", iitMSG.HTTPMSG[ iitMSG.CODE.HTTP.SUCCESS ], CustomerClass );
 
                    break;
                } // end of while( true )
            } // end of try
            catch( Exception except )
            {
                if( except.GetType() != typeof( iitException ) )
                {
                    iLog.Log.except =   except;
                    iLog.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                    iitDataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, "" );
                } // end of if( except.GetType() != typeof( iitException ) )
            } // end of catch

            return JsonConvert.SerializeObject( APIResult );
        } // end of GetAccountFromTeleNo( ... )
    } // end of public class AccountService
} // end of namespace WebAPITest1
//===================================================================================================
// end of AccountService.cs
//===================================================================================================
