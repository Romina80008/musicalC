using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Collections.Specialized;


namespace musicalC
{
    public class CreateContactEventArgs : EventArgs
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public CreateContactEventArgs(int id, string name, string number)
        {
            ID = id;
            Name = name;
            Number = number;
        }
    }
}


