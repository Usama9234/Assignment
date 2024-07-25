using System.Diagnostics;

namespace School_Management_System.Data
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            var userRole = context.Session.GetString("UserEmail");
            var path=context.Request.Path;
            // Allow POST requests to the login endpoint
            if (context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/Authentication/Login")||context.Request.Path.StartsWithSegments("/Authentication/Register"))
            {
                await _next(context);
                return;
            }

            // If user role is not set and the request is not for the login page, redirect to login
            if (string.IsNullOrEmpty(userRole) && !context.Request.Path.StartsWithSegments("/Authentication/Login"))
            {
                context.Response.Redirect("/Authentication/Login");
                return;
            }
            await _next(context);
        }
    }

}
