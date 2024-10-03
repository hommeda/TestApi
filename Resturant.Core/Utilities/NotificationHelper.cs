using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Resturant.Core.Utilities
{
    public class NotificationHelper
    {
        private readonly IConfiguration _config;

        public NotificationHelper(IConfiguration config)
        {
            _config = config;
        }
        public string SendToUsers(List<string> regIds, string title, string message, Dictionary<string, string> payload)
        {
            string result;
            var request = WebRequest.Create(_config.GetSection("AppSettings:FCMURL").Value);

            request.Method = "post";
            request.ContentType = "application/json";
            request.Headers.Add(string.Format("Authorization: key={0}", _config.GetSection("AppSettings:ServerKey").Value));
            request.Headers.Add(string.Format("Sender: id={0}", _config.GetSection("AppSettings:SenderID").Value));


            var notification = new JObject(
                  new JProperty("notification",
                      new JObject(
                          new JProperty("title", title),
                          new JProperty("body", ""),
                          new JProperty("click_action", "FCM_PLUGIN_ACTIVITY"),
                          new JProperty("sound", "default")
                          )),
                  new JProperty("registration_ids", JArray.FromObject(regIds)),
                  new JProperty("priority", "high")
                );

            var data = new JObject();
            data.Add(new JProperty("title", title));
            data.Add(new JProperty("message", message));
            if (payload != null)
            {
                foreach (var item in payload)
                {
                    data.Add(new JProperty(item.Key, item.Value));
                }
            }
            notification.Add(new JProperty("data", data));

            var byteArray = Encoding.UTF8.GetBytes(notification.ToString());
            request.ContentLength = byteArray.Length;

            using var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            using var tResponse = request.GetResponse();
            using var dataStreamResponse = tResponse.GetResponseStream();
            using var reader = new StreamReader(dataStreamResponse!);
            var responseFromServer = reader.ReadToEnd();
            result = responseFromServer;


            return result;
        }

    }
}
