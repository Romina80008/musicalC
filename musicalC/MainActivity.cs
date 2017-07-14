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


namespace musicalC
{
    [Activity(Label = "musicalC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
    {
        private ICallbackManager mCallBackManager;
        private MyProfileTracker mProfileTracker;

        private TextView mTxtFirstName;
        private TextView mTxtLastName;
        private TextView mTxtName;
        private ProfilePictureView mProfilePic;


        /*Web Service*/
        private ProgressBar mProgressBar; 
        private WebClient mClient;
        private Uri mUri;
        private List<Usuario> usuarios; 

        /*Registro*/
        private Button btnRegistro;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

/*
            FacebookSdk.SdkInitialize(this.ApplicationContext);


                mProfileTracker = new MyProfileTracker();
                mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
                mProfileTracker.StartTracking();
    
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

           




            mTxtFirstName = FindViewById<TextView>(Resource.Id.txtFirstName);
            mTxtLastName = FindViewById<TextView>(Resource.Id.txtLastName);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mProfilePic = FindViewById<ProfilePictureView>(Resource.Id.profilePic);




            LoginButton button = FindViewById<LoginButton>(Resource.Id.login_button);

            button.SetReadPermissions(new List<string> { "public_profile", "user_friends", "email" });

            mCallBackManager = CallbackManagerFactory.Create();

            
            Toast.MakeText(this, "Comprobando inicio ", ToastLength.Long).Show();


            button.RegisterCallback(mCallBackManager, this);
*/
             SetContentView(Resource.Layout.principal);

            btnRegistro = FindViewById<Button>(Resource.Id.button1);

            mClient = new WebClient();
            mUri = new Uri("http://didacdedm.pythonanywhere.com/api/users");

            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;


            btnRegistro.Click += btn_registrar;

        }//Fin del main 

        public void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {

            RunOnUiThread(() =>
            {
                string json = System.Text.Encoding.UTF8.GetString(e.Result);
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                Toast.MakeText(this, "Informacion cargada ", ToastLength.Long).Show();



            });


        }



        /*Metodo de registro*/
        public void btn_registrar(object sender, EventArgs e) {
            //controlar q ningun string sea nulo 


        }


        public void OnCompleted(Org.Json.JSONObject json, GraphResponse response)
        {
            string data = json.ToString();
            FacebookResult result = JsonConvert.DeserializeObject<FacebookResult>(data);
        }

        void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        
        void mProfileTracker_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
        {
            if (e.mProfile != null)
            {                               
                try
                {
                    mTxtFirstName.Text = e.mProfile.FirstName;
                    mTxtLastName.Text = e.mProfile.LastName;
                    mTxtName.Text = e.mProfile.Name;
                    mProfilePic.ProfileId = e.mProfile.Id;
                }

                catch (System.Exception ex)     /*Corregir o es java.exception */
                {
                    //
                }
            }
            
            else
            {
                //el usuario debe estar ingresado
                mTxtFirstName.Text = "First Name";
                mTxtLastName.Text = "Last Name";
                mTxtName.Text = "Name";
                mProfilePic.ProfileId = null;
            }
        }
        
        public void OnCancel()
        {
            Toast.MakeText(this, "Cancelado ", ToastLength.Long).Show();
           
        }

        public void OnError(FacebookException error)
        {
            Toast.MakeText(this, "Error ", ToastLength.Long).Show();
            //throw new NotImplementedException();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Toast.MakeText(this, "Success ", ToastLength.Long).Show();
            LoginResult loginResult = result as LoginResult;
            Console.WriteLine(AccessToken.CurrentAccessToken.UserId);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
        
          protected override void OnDestroy()
          {
                    mProfileTracker.StopTracking();
                    base.OnDestroy();
          }       
    }

            public class MyProfileTracker : ProfileTracker
            {
                public event EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;

                protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
                {
                    if (mOnProfileChanged != null)
                    {
                        mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(newProfile));
                    }
                }
            }

            public class OnProfileChangedEventArgs : EventArgs
            {
                public Profile mProfile;

                public OnProfileChangedEventArgs(Profile profile) { mProfile = profile; }


            }
}

