using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using KaliidoAPI;
using System.Threading.Tasks;
using System.Threading;
using Android.Util;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using KaliidoAPI.Models;


namespace Kaliido
{
    [Activity(Label = "Kaliido", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyListUserButton);
            

            button.Click += delegate {

                var myIntent = new Intent(this, typeof(Resources.layout.ListOfPeopleTest));
                myIntent.PutExtra("myString", "valueOfMyString");
                StartActivity(myIntent);

            };

        }
    }
}

