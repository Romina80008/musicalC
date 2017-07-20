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

namespace musicalC
{
    class Usuario : EventArgs
    {
        public string email { get; set; }
        public string first_name { get; set; }
       // public string id { get; set; }
        public bool is_admin { get; set; }
        public string last_name { get; set; }
        public string password_hash { get; set; }
        public string username { get; set; }


        public Usuario(string email, string first_name, string last_name, string password_hash, string username ) {

            this.email = email;
            this.first_name = first_name;
            this.is_admin = true;
            this.last_name = last_name;
            this.password_hash = password_hash;
            this.username = username;
        }

    }
}