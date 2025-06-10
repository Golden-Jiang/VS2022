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
using iitMSGWeb;
using iitSystemWeb;
using iitToolsWeb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using WebAPITest6.Models;
//---------------------------------------------------------------------------------------------------
// Program Area
//---------------------------------------------------------------------------------------------------
namespace WebAPITest6.Filter
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync( AuthorizationFilterContext context )
        {
            //await context.HttpContext.Response.WriteAsync( $"{GetType().Name} in \r\n" );
        }
    } // end of public class AuthorizationFilter : IAsyncAuthorizationFilter
    public class ResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync( ResourceExecutingContext context, ResourceExecutionDelegate next )
        {
            //await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");

            await next();

            //await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        } // end of public async Task OnResourceExecutionAsync( ... )
    } // end of public class ResourceFilter : IAsyncResourceFilter

    public class ActionFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IiitLog _Log;
        private readonly string _ClientIP;

        public ActionFilter( IHttpContextAccessor httpContextAccessor, IiitLog Log ) 
        { 
            _httpContextAccessor    =   httpContextAccessor;
            _Log                    =   Log; 
            _ClientIP               =   iitSystemTools.SetClientIP( httpContextAccessor );

            //iitSystemTools.SetClientEnvironment( httpContextAccessor, _Log,  _ClientIP );
        } // end of public ActionFilter( IHttpContextAccessor httpContextAccessor, IiitLog Log ) 

        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            iitSystemTools.SetClientEnvironment( _httpContextAccessor, _Log,  _ClientIP );

            await next();

            //await context.HttpContext.Response.WriteAsync( $"{GetType().Name} out. \r\n" );
        } // end of public async Task OnActionExecutionAsync( ... )
    } // end of public class ActionFilter : IAsyncActionFilter  

    public class ResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync( ResultExecutingContext context, ResultExecutionDelegate next )
        {
            //await context.HttpContext.Response.WriteAsync( $"{GetType().Name} in \r\n" );
            await next();

            //await context.HttpContext.Response.WriteAsync( $"{GetType().Name} out \r\n" );
        } // end of public async Task OnResultExecutionAsync( ... )
    } // end of public class ResultFilter : IAsyncResultFilter

    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IiitLog _Log;

        public ExceptionFilter( IiitLog Log )
        {
            _Log    =   Log;
        }

        public void OnException( ExceptionContext context )
        {
            string              ResponseMessage = "", _ClientIP = "::1";
            iitAPIResultClass   APIResult = new iitAPIResultClass();

            if( context.HttpContext.Connection.RemoteIpAddress != null ) 
                _ClientIP   = context.HttpContext.Connection.RemoteIpAddress.ToString();
            // 獲取異常信息
            _Log.except =   context.Exception;
            _Log.WriteLog( "", iitConst.LOG.ERROR, iitConst.LOG.LEVEL_HIGHEST, _ClientIP );

            APIResult = JsonConvert.DeserializeObject<iitAPIResultClass>( context.Exception.Message );
            ResponseMessage =   JsonConvert.SerializeObject( APIResult );

            context.HttpContext.Response.WriteAsync( ResponseMessage );

//            context.Result = result; // 設置結果
            context.ExceptionHandled = true; // 標記異常已處理
            // 可以選擇繼續執行其他邏輯
            // 例如，記錄日誌或執行清理操作
        } // end of public void OnException( ... )
    } // end of public class ExceptionFilter : Attribute, IExceptionFilter
} // end of namespace WebAPITest6
//===================================================================================================
// end of IWebAPIRepository.cs
//===================================================================================================
