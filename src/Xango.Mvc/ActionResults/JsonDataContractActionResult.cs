using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;

namespace Xango.Mvc.ActionResults
{
    public class JsonDataContractActionResult : ActionResult
    {
        public JsonDataContractActionResult(Object data)
        {
            this.Data = data;
        }

        public Object Data { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new DataContractJsonSerializer(this.Data.GetType());
            String output = String.Empty;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, this.Data);
                output = Encoding.Default.GetString(ms.ToArray());
            }
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(output);
        }
    }
}