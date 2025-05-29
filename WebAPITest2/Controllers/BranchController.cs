//===================================================================================================
// Project Name  : TSB2.0 WebAPI
// Program Name  : BranchController.cs
// Description   :  
// Version		 : Ver 1.0.0.0
// Create Author : Golden Jiang 2025/05/28 17:30 建立於 D:\Golden\Project\VS2022\WebAPITest2 目錄 
// Update Record :
// Note          :
//===================================================================================================
//---------------------------------------------------------------------------------------------------
// declare package
//---------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WebAPITest2.Models;
//
// iit SDK 
//
using iitDataWeb;
using iitMSGWeb;
using iitSystemWeb;
using iitLogWeb;
using WebAPITest2.Dtos;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebAPITest2.Controllers
{
    [Route( "[controller]" )]
    [ApiController]
    public class BranchController : ControllerBase
    {
       private readonly DBContext _DBContext;

       public BranchController(IHttpContextAccessor httpContextAccessor, DBContext dBContext)
       {
            ILog iLog =   new ILog();
            //
            _DBContext = dBContext;
            //
            SystemTools.SetClientIP(httpContextAccessor);
            //
            iLog.WriteLog( Static.httpContextAccessor.HttpContext?.Request?.GetEncodedUrl(), iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG, null );
        } // end of public BranchController(IHttpContextAccessor
        //
        // GET: <BranchController>
//        [HttpGet]
//        public string GetRecord([FromQuery(Name = "TxCode")]string TxCode)
//        {
//            iitAPIResultClass APIResult = new iitAPIResultClass();
//            ILog iLog =   new ILog();
//            //
//            try
//            { 
//                var result = (from a in _DBContext.Branch
//                              select a).FirstOrDefault();

//                DataTools.SetResponseResult<Branch>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
//                iLog.WriteLog( $"TcCode={TxCode}-{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
//            }
//            catch( Exception except )
//            {
//                iLog.Log.except = except;
//                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
//                //
//                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
//                //return OK(APIResult);
//            }
////
//            return JsonConvert.SerializeObject( APIResult );
//        } // end of public Branch GetRecord()

        // GET https://url/Branch/id
        [HttpGet( "{id}" )]
        public string Get( string id )
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            {
                var result  =   ( from a in _DBContext.Branch
                                  where a.BranchID == id
                                  select new BranchDtoGroup.BranchSelect
                                  {
                                      Name = a.Name,
                                      TeleNo = a.TeleNo,
                                      Address = a.Address
                                  }
                                ).SingleOrDefault();

                //var result  =     _DBContext.Branch.Where( x => x.BranchID == id ).FirstOrDefault();

                //DataTools.SetResponseResult<dynamic>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                //DataTools.SetResponseResult<object>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                DataTools.SetResponseResult<BranchDtoGroup.BranchSelect>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                iLog.WriteLog( $"BranchID={id}-{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                iLog.Log.except = except;
                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                //
                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
                //return OK(APIResult);
            }
//
            return JsonConvert.SerializeObject( APIResult );
        } // end of public string Get( string id )

        // GET https://url/Branch?TeleNo=xxx
        [HttpGet]
        public string Select( [FromQuery(Name = "Address")] string Address )
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            {
                var result  =   ( from a in _DBContext.Branch
                                  where a.Address.Contains(Address)
                                  select new BranchDtoGroup.BranchSelect
                                  {
                                      Name = a.Name,
                                      TeleNo = a.TeleNo,
                                      Address = a.Address
                                  }
                                ).ToList();

                //var result  =     _DBContext.Branch.Where( x => x.BranchID == id ).FirstOrDefault();

                //DataTools.SetResponseResult<dynamic>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                //DataTools.SetResponseResult<object>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                DataTools.SetResponseResult<List<BranchDtoGroup.BranchSelect>>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                iLog.WriteLog( $"{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                iLog.Log.except = except;
                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                //
                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
                //return OK(APIResult);
            }
//
            return JsonConvert.SerializeObject( APIResult );
        } // end of public string Get( [FromQuery(Name = "BranchID")] string BranchID )

        // POST api/<BranchController>
        public class Tel 
        {
            [Required]
            public string TeleNo { get; set;} 
            public string Name { get; set;}
        }
        [HttpPost]
        public string Post( [FromQuery(Name = "TxCode")]string TxCode, [FromBody] Tel ss )
        {
            iitAPIResultClass APIResult = new iitAPIResultClass();
            ILog iLog =   new ILog();
            //
            try
            { 
                var result  =   ( from a in _DBContext.Branch
                                  select new BranchDtoGroup.BranchSelect
                                  {
                                    Name = a.Name,
                                    TeleNo = a.TeleNo,
                                    Address = a.Address
                                  }
                                ).FirstOrDefault();

                //DataTools.SetResponseResult<dynamic>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                //DataTools.SetResponseResult<object>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                DataTools.SetResponseResult<BranchDtoGroup.BranchSelect>( APIResult, "0000", iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS], result );
                iLog.WriteLog( $"TcCode={TxCode}-{iitMSG.HTTPMSG[iitMSG.CODE.HTTP.SUCCESS]}", iitConst.LOG.INFO, iitConst.LOG.LEVEL_DEBUG );
            }
            catch( Exception except )
            {
                iLog.Log.except = except;
                iLog.WriteLog( "Error", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST );
                //
                DataTools.SetResponseResult<string>( APIResult, "8501", iitMSG.APIError.E8501, except.Message );
                //return OK(APIResult);
            }
//
            return JsonConvert.SerializeObject( APIResult );
        } // end of public void Post( string TxCode, [FromBody] string value )

        // PUT api/<BranchController>/5
        [HttpPut( "{id}" )]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE api/<BranchController>/5
        [HttpDelete( "{id}" )]
        public void Delete( int id )
        {
        }
    } // end of public class BranchController : ControllerBase
} // end of WebAPITest2.Controllers
//===================================================================================================
// end of BranchController.cs
//===================================================================================================
