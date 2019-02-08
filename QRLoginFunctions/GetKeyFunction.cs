using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using QRCoder;

namespace QRLoginFunctions
{
    public static class GetKeyFunction
    {
        [FunctionName("GetKey")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "key/new")]
            HttpRequestMessage req,
            TraceWriter log)
        {
            var nc = req.GetQueryNameValuePairs().ToArray();
            var length = nc.FirstOrDefault<int>("length", StringComparer.OrdinalIgnoreCase);
            length = Math.Min(128, Math.Max(8, length));
            var pixelsPerModule = nc.FirstOrDefault<int>("pixelsPerModule", StringComparer.OrdinalIgnoreCase);
            pixelsPerModule = Math.Min(50, Math.Max(5, pixelsPerModule));

            var buffer = new byte[length];
            new Random().NextBytes(buffer);
            var secret = Convert.ToBase64String(buffer);

            using (var generator = new QRCodeGenerator())
            {
                var qrData = generator.CreateQrCode(secret, QRCodeGenerator.ECCLevel.Q, true);
                var qrCode = new QRCode(qrData);
                using (var img = qrCode.GetGraphic(pixelsPerModule))
                {
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, ImageFormat.Png);
                        var imageDataString = Convert.ToBase64String(ms.ToArray());
                        return req.CreateResponse(
                            HttpStatusCode.OK,
                            new QrCodeResult {ImageData = imageDataString},
                            "application/json"
                        );
                    }
                }
            }
        }
    }
}
