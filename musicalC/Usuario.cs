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
    class Usuario
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string id { get; set; }
        public string is_admin { get; set; }
        public string last_name { get; set; }
        public string password_hash { get; set; }
        public string username { get; set; }

    }
}