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

    public class CreateContactEventArgs : EventArgs
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string is_admin { get; set; }
        public string last_name { get; set; }
        public string password_hash { get; set; }
        public string username { get; set; }

        public CreateContactEventArgs(string email, string first_name, string last_name, string password_hash, string username)
        {

            this.email = email;
            this.first_name = first_name;
            this.is_admin = "true";
            this.last_name = last_name;
            this.password_hash = password_hash;
            this.username = username;
        }
    }  /*Mandar a una clase*/

    class CreateContactDialog : DialogFragment
    {
        private Button mButtonCreateContact;
        /*Registrar usuario*/
        private TextView txtEmail;
        private TextView txtUsername;
        private TextView txtFirstname;
        private TextView txtLastname;
        private TextView txtPassword;
        private TextView txtConfirmPassword;

        public event EventHandler<CreateContactEventArgs> OnCreateContact;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.registro, container, false);

            txtEmail = view.FindViewById<TextView>(Resource.Id.txtEmail);
            txtUsername = view.FindViewById<TextView>(Resource.Id.txtUsername);
            txtFirstname = view.FindViewById<TextView>(Resource.Id.txtFirstname);
            txtLastname = view.FindViewById<TextView>(Resource.Id.txtLastname);
            txtPassword = view.FindViewById<TextView>(Resource.Id.txtPassword);
            txtConfirmPassword = view.FindViewById<TextView>(Resource.Id.txtConfirmPassword);
            //mProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            mButtonCreateContact = view.FindViewById<Button>(Resource.Id.btnRegistrar1);

            mButtonCreateContact.Click += mButtonCreateContact_Click;
            return view;

        }

        void mButtonCreateContact_Click(object sender, EventArgs e)
        {
           // mProgressBar.Visibility = ViewStates.Visible;


            WebClient client = new WebClient();
            Uri uri = new Uri("http://didacdedm.pythonanywhere.com/api/users/add");
            NameValueCollection parameters = new NameValueCollection();
            

            parameters.Add("email", txtEmail.Text);
            parameters.Add("first_name", txtFirstname.Text);
            // parameters.Add("is_admin", txtEmail.Text);
            parameters.Add("last_name", txtLastname.Text);
            parameters.Add("password_hash", txtPassword.Text);
            parameters.Add("username", txtUsername.Text);

            //HttpClient oHttpClient = new HttpClient();

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameters);

        }

        void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Activity.RunOnUiThread(async () =>
            {
                //string id = Encoding.UTF8.GetString(e.Result); //Get the data echo backed from PHP
                //int newID = 0;
                //int.TryParse(id, out newID); //Cast the id to an integer

                if (OnCreateContact != null)
                {
                    //Broadcast event

                    //OnCreateContact.Invoke(this, new CreateContactEventArgs(txtEmail.Text, txtFirstname.Text, txtLastname.Text, txtPassword.Text, txtUsername.Text));
                    var person = new Usuario(txtEmail.Text, txtFirstname.Text, txtLastname.Text, txtPassword.Text, txtUsername.Text);
                    var json = JsonConvert.SerializeObject(person);
                    // var content = new StringContent(json, Encoding.UTF8, "application/json");

                    await MakePostRequest("http://didacdedm.pythonanywhere.com/api/users/add", json);


                }

                //mProgressBar.Visibility = ViewStates.Invisible;
                this.Dismiss();
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

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
           // Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}