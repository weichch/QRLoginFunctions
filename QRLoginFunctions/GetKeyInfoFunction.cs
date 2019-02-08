using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace QRLoginFunctions
{
    public static class GetKeyInfoFunction
    {
        [FunctionName("GetKeyInfoFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "key")]
            HttpRequestMessage req,
            TraceWriter log)
        {
            var key = req.GetQueryNameValuePairs()
                .FirstOrDefault<string>("key", StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(key))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid key.");
            }

            var session = Sessions.GetOrCreateSession(key);
            try
            {
                await session.MoveNextAsync();
                return req.CreateResponse(HttpStatusCode.OK, session.KeyInfo, "application/json");
            }
            catch (TaskCanceledException)
            {
                return req.CreateResponse(HttpStatusCode.RequestTimeout);
            }
        }
    }
}
