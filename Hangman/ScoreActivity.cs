using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hangman
{
    [Activity(Label = "ScoreActivity")]
    public class ScoreActivity : Activity
    {
        private Button button_next;
        private Button button_exit;
        private ImageView home_button;
        private TextView good_text;
        private TextView word_text;
        private TextView score_text;
        private TextView hint_text;
        
        public static int flag_save = 0;

        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_score);

            db = new Database();
            db.createDatabase();

            button_next = FindViewById<Button>(Resource.Id.btn_next);
            button_exit = FindViewById<Button>(Resource.Id.btn_exit);

            home_button = FindViewById<ImageView>(Resource.Id.button_home);

            good_text = FindViewById<TextView>(Resource.Id.txt_good);
            word_text = FindViewById<TextView>(Resource.Id.txt_word);
            score_text = FindViewById<TextView>(Resource.Id.txt_solvednumber);
            hint_text = FindViewById<TextView>(Resource.Id.txt_hintvalue);

            Show_components();

            button_next.Click += delegate
            {
                if (MainActivity.flag_succes == 2)
                {
                    StartActivity(new Intent(Application.Context, typeof(StartActivity)));
                    Finish();
                    
                }
                else
                {
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    Finish();
                }
            };

            home_button.Click += delegate
            {
                GoTo_start();
            };

            button_exit.Click += delegate
            {
                Exit();
            };

            // Create your application here
        }

        public void GoTo_start()
        {
            MainActivity.flag_resume = 2;
            StartActivity(new Intent(Application.Context, typeof(StartActivity)));
            //Finish();
        }

        public void Show_components()
        {
            word_text.Text = "WORD: " + MainActivity.target_str;
            if(MainActivity.flag_succes == 0)
            {
                good_text.Text = "Wrong";
                score_text.Text = "SOLVED: " + MainActivity.solved_value.ToString();
                hint_text.Text = "HINT:" + MainActivity.hint_value.ToString();
            } else if ( MainActivity.flag_succes == 1) {
                String good;
                good = "GOOD!";
                good_text.Text = good;
                score_text.Text = "SOLVED: " + MainActivity.solved_value.ToString();
                hint_text.Text = "HINT:" + MainActivity.hint_value.ToString() + "(+1)";
            }
            else
            {
                score_text.Text = "SOLVED: " + MainActivity.solved_value.ToString();
                hint_text.Text = "HINT:" + MainActivity.hint_value.ToString();
               
                if (MainActivity.top_score < MainActivity.solved_value)
                {
                    good_text.Text = "Congratlation!";
                    Save_score();
                    flag_save = 1;
                    button_next.Text = "New Game";
                    button_next.TextSize = 20;
                }
                else
                {
                    good_text.Text = "Time is UP!";
                    flag_save = 0;
                    button_next.Text = "Retry";
                }
                 
            }
        }

        public void Save_score()
        {
            try
            {
                Userscore userscore = new Userscore
                {
                    Name = MainActivity.username,
                    Score = MainActivity.solved_value,
                    Level = MainActivity.levelname
                };
                db.insertIntoTable(userscore);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        public void Exit()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Confirm EXIT");

            alert.SetMessage("Do you want to Exit?");

            alert.SetPositiveButton("Yes", (senderAlert, args) => {

                StartActivity(new Intent(Application.Context, typeof(StartActivity)));
                Finish();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();

            dialog.Show();
        }
    }
}