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

namespace Hangman
{
    [Activity(Label = "ScoreviewActivity")]
    public class ScoreviewActivity : Activity
    {
        private Button button_resetscore;
        private Button button_endgame;
        private Button button_restartgame;
        private TextView currentusername_text;
        private TextView currentscore_text;
        private TextView highestscore_text;

        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_scoreview);

            db = new Database();
            db.createDatabase();

            button_resetscore = FindViewById<Button>(Resource.Id.btn_resetscore);
            button_endgame = FindViewById<Button>(Resource.Id.btn_resumegame);
            button_restartgame = FindViewById<Button>(Resource.Id.btn_endgame);

            currentusername_text = FindViewById<TextView>(Resource.Id.txt_currentusername);
            currentscore_text = FindViewById<TextView>(Resource.Id.txt_currentscore);
            highestscore_text = FindViewById<TextView>(Resource.Id.txt_highestscore);

            Display_allscoreandusername();

            if( MainActivity.solved_value > MainActivity.top_score )
            {
                Update_topscore();
            }

            button_resetscore.Click += delegate
            {
                Reset_score();
            };

            button_endgame.Click += delegate
            {
                End_game();
            };

            button_restartgame.Click += delegate
            {
                Restart_game();
            };
        }

        public void Display_allscoreandusername()
        {
            currentusername_text.Text = "Congratulations " + MainActivity.username;
            currentscore_text.Text = MainActivity.solved_value.ToString();
            highestscore_text.Text = "Highest Score : " + MainActivity.top_score.ToString();
        }

        public void Update_topscore()
        {
            if (db.getUser(MainActivity.username))
            {
                try
                {
                    Userscore userscore = new Userscore
                    {
                        Name = MainActivity.username,
                        Score = MainActivity.solved_value,
                        Level = MainActivity.levelname
                    };
                    db.updateTable(userscore);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
            }
            else
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
        }

        public void End_game()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Confirm EXIT");

            alert.SetMessage("Do you want to Exit?");

            alert.SetPositiveButton("Yes", (senderAlert, args) => {

                var activity = (Activity)this;
                activity.FinishAffinity();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();

            dialog.Show();
        }

        public void Restart_game()
        {
            StartActivity(new Intent(Application.Context, typeof(StartActivity)));
            Finish();
        }

        public void Reset_score()
        {
            db.removeTable();
            highestscore_text.Text = "Highest Score : 0";
        }
    }
}