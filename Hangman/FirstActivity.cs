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
using System.Timers;

namespace Hangman
{
    [Activity(Label = "FirstActivity", NoHistory = true)]
    public class FirstActivity : Activity
    {
        Userdatabase db;
        Timer timer;
        ProgressBar mProgress;
        TextView loading_text;
        int sec = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_first);

            db = new Userdatabase();
            db.createDatabase();

            MainActivity.username = "Player";
            loading_text = FindViewById<TextView>(Resource.Id.txt_loading);
            mProgress = (ProgressBar)FindViewById(Resource.Id.progress_bar);

            if (db.confirmTable(MainActivity.username) == 0)
            {
                try
                {
                    Users users = new Users
                    {
                        Name = MainActivity.username,
                    };
                    db.insertIntoTable(users);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
            }

            timer = new Timer
            {
                Interval = 100 // 1 second  
            };
            timer.Elapsed += Timer_Elapsed1;
            timer.Start();
        }

        private void Timer_Elapsed1(object sender, ElapsedEventArgs e)
        {
            sec++;
            mProgress.Progress = sec;
            
            if (sec <= 100)
            {
                loading_text.Text = "Loading...  " + sec.ToString() + "%";
            }
            else
            {
                timer.Dispose();
                timer.Stop();
                StartActivity(new Intent(Application.Context, typeof(StartActivity)));
                Finish();
            }
        }
    }
}