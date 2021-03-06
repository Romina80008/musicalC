﻿using System;
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

using System.Text;

using Android.Support.V7.App;



namespace musicalC
{
    
    [Activity(Label = "musicalC", MainLauncher = true, Icon = "@drawable/icon", Theme ="@style/MyTheme")]
    public class MainActivity : ActionBarActivity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback //Activity
    {

        /*Facebook Service*/
        private ICallbackManager mCallBackManager;
        private MyProfileTracker mProfileTracker;

        private TextView mTxtFirstName;
        private TextView mTxtLastName;
        private TextView mTxtName;
        private ProfilePictureView mProfilePic;


        /*Web Service*/
      //  private ProgressBar mProgressBar; 
      //  private WebClient mClient;
      //  private Uri mUri;
        private List<Usuario> usuarios; 

        /*Botones*/
        private Button btnRegistrar;
        private Button btnEmpezar;
        private Button btnIniciarSesion;

        LoginButton btnFacebook;


        /*Menu*/
        private Android.Support.V7.Widget.Toolbar mToolbar;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            /*
            FacebookSdk.SdkInitialize(this.ApplicationContext);

            mProfileTracker = new MyProfileTracker();
            mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
            mProfileTracker.StartTracking();
            */

            // Renderizando primerapantalla 

            SetContentView(Resource.Layout.empezar);

            btnEmpezar = FindViewById<Button>(Resource.Id.btnEmpezar);
            btnEmpezar.Click += btn_empezar;

            //ConsumirServicioGet();



        }//Fin del main 



        /*Metodo de registro*/
        public void btn_registrar(object sender, EventArgs e)
        {
           // SetContentView(Resource.Layout.registro);
            CreateContactDialog dialog = new CreateContactDialog();
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

          //Subscribe to event
            dialog.OnCreateContact += dialog_OnCreateContact;
            dialog.Show(transaction, "create contact! RRRR");


        }

        void dialog_OnCreateContact(object sender, Usuario e)
        {
            //Create a new contact and update the UI
            // mContacts.Add(new Contact() { ID = e.ID, Name = e.Name, Number = e.Number });
            // mAdapter.NotifyDataSetChanged();
            Toast.MakeText(this, "ENTRO ACA ", ToastLength.Long).Show();
        }



        /*Metodo de registro*/
        public void btn_empezar(object sender, EventArgs e)
        {
            //pantalla para logearse
            FacebookSdk.SdkInitialize(this.ApplicationContext);


            SetContentView(Resource.Layout.Main);

            mProfileTracker = new MyProfileTracker();
            mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
            mProfileTracker.StartTracking();


            /*Informacion extraida de facebook*/
            mTxtFirstName = FindViewById<TextView>(Resource.Id.txtFirstName);
            mTxtLastName = FindViewById<TextView>(Resource.Id.txtLastName);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mProfilePic = FindViewById<ProfilePictureView>(Resource.Id.profilePic);

            // LoginButton
            btnFacebook = FindViewById<LoginButton>(Resource.Id.login_button);
            btnFacebook.Click += botonFB; 


            
            btnRegistrar = FindViewById<Button>(Resource.Id.btnRegistrar);
            btnRegistrar.Click += btn_registrar;

            btnIniciarSesion = FindViewById<Button>(Resource.Id.btnIniciarSesion);
            btnIniciarSesion.Click += btn_iniciar_sesion;

        }

        private void botonFB(object sender, EventArgs e)
        {
            btnFacebook.SetReadPermissions(new List<string> { "public_profile", "user_friends", "email" });
            mCallBackManager = CallbackManagerFactory.Create();
            btnFacebook.RegisterCallback(mCallBackManager, this);
        }

        /*Metodo de registro*/
        public void btn_iniciar_sesion(object sender, EventArgs e)
        {
            //si esque el usuario se encuentra en la base 
            SetContentView(Resource.Layout.Principal);



            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Bienvenido @Nombre";
            spin();


            //else -> presenta mensaje de error no pasa de pagina

        }

        public void spin() {
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinner2 = FindViewById<Spinner>(Resource.Id.spinner2);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.generos, Android.Resource.Layout.SimpleSpinnerItem); //generos esta declarado en strings.xml 

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleDropDownItem1Line);
            spinner.Adapter = adapter;

            /*Otro spinner */
            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
           /* var adapter2 = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.musicales, Android.Resource.Layout.SimpleSpinnerItem); //generos esta declarado en strings.xml 

            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleDropDownItem1Line);
            spinner2.Adapter = adapter2;
            */

        }

        /*Metodo para el spinner o combobox */
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

           // string toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
           // Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        /*MEnu de opciones*/
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            int id = item.ItemId;
            if (id == Resource.Id.action_retro)
            {
                SetContentView(Resource.Layout.Principal);
                spin();
                return true;

            }
            else if (id == Resource.Id.action_vestuarios) {
                Toast.MakeText(this, "Desplegar Vest ", ToastLength.Short).Show();
                return true;

            }
            else if (id == Resource.Id.action_generos) {
                Toast.MakeText(this, "Desplegar Genres ", ToastLength.Short).Show();
                return true;

            }
            else if (id == Resource.Id.action_perfil) {
                Toast.MakeText(this, "Mi Perfil Romi ", ToastLength.Short).Show();
                SetContentView(Resource.Layout.Pantalla_Perfil);

                return true;

            }
            return base.OnOptionsItemSelected(item);
        }


        /*Consumiendo servicio*/
        /*
        public void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {  //Cargando la informacion 

            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                Toast.MakeText(this, "Informacion cargada ", ToastLength.Long).Show();
               // Console.ReadKey();
            });

        }
        */

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
                    /* Insanciar en mi base tambien ??
                    mTxtFirstName.Text = e.mProfile.FirstName; //e.mProfile.FirstName;
                    mTxtLastName.Text = e.mProfile.LastName;
                    mTxtName.Text = e.mProfile.Name;
                    mProfilePic.ProfileId = e.mProfile.Id;

                    */

                    SetContentView(Resource.Layout.Principal);

                    mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                    SetSupportActionBar(mToolbar);
                    SupportActionBar.Title = "Hi,  " + e.mProfile.FirstName + " ! ";
                    spin();

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

