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
    class Genero : EventArgs
    {
        public string descripcion { get; set; }
        public string id { get; set; }
        public string name { get; set; }


        public Genero(string descripcion, string id, string name)
        {
            this.descripcion = descripcion;
            this.id = id;
            this.name = name;
        }


    }
}