using System.Linq;
using System.Text;

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Java.Security;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;
using System.Collections.Generic;
using Android.Graphics;
using Java.Net;
using Android.Provider;
using Xamarin.Facebook.Share.Model;
using System.IO;
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Share;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using Java.Lang;
using Org.Json;


using System.Threading.Tasks;

namespace musicalC
{
    class Servicios
    {
        private WebClient mClient;
        private Uri mUri;
        private List<Usuario> usuarios;
        private List<Vestuario> vestuarios;
        private List<Genero> generos;

        public Servicios() {  //Constructor 

        }

        public void ConsumirServicioGet(string url)
        {
            // Consumiendo servicio get 
            mClient = new WebClient();
            mUri = new Uri(url);

            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted_Generos;
        }

        /*Consumiendo servicio*/
        public void mClient_DownloadDataCompleted_Usuarios(object sender, DownloadDataCompletedEventArgs e)
        {  //Cargando la informacion 

            Activity a = new Activity();
            a.RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
               // Toast.MakeText(this, "Informacion cargada ", ToastLength.Long).Show();
                // Console.ReadKey();
            });
        }

        public void mClient_DownloadDataCompleted_Vestuarios(object sender, DownloadDataCompletedEventArgs e)
        {  //Cargando la informacion 

            Activity a = new Activity();
            a.RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                vestuarios = JsonConvert.DeserializeObject<List<Vestuario>>(json);
                // Toast.MakeText(this, "Informacion cargada ", ToastLength.Long).Show();
                // Console.ReadKey();
            });

   
        }

        public void mClient_DownloadDataCompleted_Generos(object sender, DownloadDataCompletedEventArgs e)
        {  //Cargando la informacion 

            Activity a = new Activity();
            a.RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                generos = JsonConvert.DeserializeObject<List<Genero>>(json);
                // Toast.MakeText(this, "Informacion cargada ", ToastLength.Long).Show();
                // Console.ReadKey();
            });
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