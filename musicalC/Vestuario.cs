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
    class Vestuario
    {
        public string sex { get; set; }
        public string photo { get; set; }
        public string description { get; set; }
        public string user { get; set; }
        public string genre { get; set; }



        public Vestuario(string sex, string photo, string description, string user, string genre)
        {

            this.sex = sex;
            this.photo = photo;
            this.description = description;
            this.user = user;
            this.genre = genre;
        }
    }
}