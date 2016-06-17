using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using System.Threading.Tasks;
using KaliidoAPI;
using KaliidoAPI.Models;

namespace Kaliido.Resources.layout
{
    [Activity(Label = "ListOfPeopleTest")]
    public class ListOfPeopleTest : Activity
    {

        public static KaliidoServiceFactory serviceFactory;
        ListView myContent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.ListOfPeopleTest);

            myContent = FindViewById<ListView>(Resource.Id.listView1);
            IListAdapter myListAdapter;

            Task.Factory.StartNew(async()=> {
                    /// new thread
                        serviceFactory = new KaliidoServiceFactory().Instance;
                        
                        TokenResponse response = await serviceFactory.loginSession("robbie@kaliido.com", "Matt2105*");
                        if (response.access_token != null)
                        {
                        List<KaliidoUser> usersCLosest = await serviceFactory.getUsersClosestByDistance(2000);

                        List<string> myList = new List<string>();

                        foreach(KaliidoUser user in usersCLosest)
                        {
                            myList.Add(user.fullName);
                        }

                        Log.Debug("DEBUG", "usersCLosest executed");

                            RunOnUiThread(() => {
                                // this is on ui thread
                                //Toast.MakeText(this, "almost there", ToastLength.Long).Show();
                                myListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, myList);
                                myContent.SetAdapter(myListAdapter);

                                int a = 1;
                            });
                    }

                        
                });









            // playing with files and directories

            string appDirectory = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

            var myDirectory = Path.Combine(appDirectory, "myDirectory");
            Directory.CreateDirectory(myDirectory); // only if it doesn't exist

            var textFilePath = Path.Combine(appDirectory, "mytextfile.txt"); 
            File.CreateText(textFilePath);

            var entries = Directory.EnumerateDirectories(appDirectory);
            foreach(var e in entries)
            {
                Log.Debug("debug", e);
            }

            var myString = Intent.GetStringExtra("myString");
            Log.Debug("Drbug", "Message: " + myString);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("debug", "DESTROY");
            SaveState();
        }

        void SaveState()
        {

        }
    }
}