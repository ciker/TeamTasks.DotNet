using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLibrary.ResourceServer
{
    public class ResourceServerMiddleware
    {
        private RequestDelegate _next;
        private ICrypter _crypter;
        private ResourceServerOptions _resourceServerOptions;

        public ResourceServerMiddleware(RequestDelegate next, ICrypter crypter, ResourceServerOptions resourceServerOptions)
        {
            _next = next;
            _crypter = crypter;
            _resourceServerOptions = resourceServerOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string[] values = context.Request.Headers["Authorization"].ToString().Split(' ');

                if (values.Length != 2)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }

                if (values.First() != "Bearer")
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }

                try
                {

                    string webTokenJsonString = _crypter.Decrypt(values[1], _resourceServerOptions.CryptionKey);
                    WebToken webToken = JsonConvert.DeserializeObject<WebToken>(webTokenJsonString);

                    if (webToken.Issuer != _resourceServerOptions.Issuer)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("issuers-do-not-match");
                    }

                    DateTime tokenExpiration = webToken.CreatedDate.AddDays(_resourceServerOptions?.TokenDurationInDays ?? 14);

                    if (DateTime.Now > tokenExpiration)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("token-expired");
                    }

                    // Now, we have to write the claims to the ClaimsPrincipal!
                    context.User = webToken.ConvertToClaimsPrincipal();
                }
                catch (Exception e)
                {
                    var dummy2 = 45;
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync($"{e.Message} {e.InnerException?.Message ?? "no inner exception"}");
                }

            }

            await _next.Invoke(context);
        }
    }
}
