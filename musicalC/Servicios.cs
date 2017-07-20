using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace musicalC
{
    class Servicios
    {
        public Servicios() {  //Constructor 

        }
        

        public async Task<string> MakePostRequest(string url, string serializedDataString, bool isJson = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (isJson)
                request.ContentType = "application/json";
            else
                request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";
            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(serializedDataString);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();

            using (StreamReader sr = new StreamReader(respStream))
            {
                return sr.ReadToEnd();
            }
        }

        public static async Task<string> MakeGetRequest(string url, string cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/html";
            request.Method = "GET";
            request.Headers["Cookie"] = cookie;

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();
            respStream.Flush();

            using (StreamReader sr = new StreamReader(respStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                respStream = null;
                return strContent;
            }
        }


    }

}