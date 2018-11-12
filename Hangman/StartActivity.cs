using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hangman
{
    [Activity(Label = "StartActivity")]
    public class StartActivity : Activity
    {
        Userdatabase db;
        private Button button_start;
        private Button button_resume;
        private Button button_exit;
        private TextView title_text;
        private TextView user_text;
        private TextView newuser_text;
        private TextView olduser_text;
        private LinearLayout linear_resume;
        public String flag_level = "easy";
        public String flag_category = "animal";
        
        long lastPress;
        private EditText nameedit;
        private TextView titl_dialog;
        private ListView userlist;
        List<Users> listSource = new List<Users>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_start);

            db = new Userdatabase();
            db.createDatabase();

            /*if (db.confirmTable(MainActivity.username) == 0)
            {
                Save_username();
            }*/

            button_start = FindViewById<Button>(Resource.Id.btn_start);
            button_resume = FindViewById<Button>(Resource.Id.btn_resume);
            button_exit = FindViewById<Button>(Resource.Id.btn_exit);
            title_text = FindViewById<TextView>(Resource.Id.txt_title);
            user_text = FindViewById<TextView>(Resource.Id.txt_username);
            newuser_text = FindViewById<TextView>(Resource.Id.txt_newuser);
            olduser_text = FindViewById<TextView>(Resource.Id.txt_olduser);
            linear_resume = FindViewById<LinearLayout>(Resource.Id.linear_resume);

            user_text.Text = "Welcome " + MainActivity.username;

            Spinner spinner_cate = FindViewById<Spinner>(Resource.Id.spinner_category);
            spinner_cate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_cate_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.category_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_cate.Adapter = adapter;

            Spinner spinner_level = FindViewById<Spinner>(Resource.Id.spinner_level);
            spinner_level.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_level_ItemSelected);
            var adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.level_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_level.Adapter = adapter1;

            Typeface tf1 = Typeface.CreateFromAsset(Assets, "Sketch.ttf");
            Typeface tf2 = Typeface.CreateFromAsset(Assets, "Romantic Beach.ttf");
            Typeface tf3 = Typeface.CreateFromAsset(Assets, "Sin City.ttf");
            Typeface tf4 = Typeface.CreateFromAsset(Assets, "SouthernAire_Personal_Use_Only.ttf");

            title_text.Typeface = tf4;

            Show_resumlinear();

            newuser_text.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.user_input_dialog_box, null);
                Android.Support.V7.App.AlertDialog.Builder alertbuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertbuilder.SetView(view);
                alertbuilder.SetPositiveButton("Create", (EventHandler<DialogClickEventArgs>)null);
                alertbuilder.SetNegativeButton("Cancel", (EventHandler<DialogClickEventArgs>)null);
                nameedit = view.FindViewById<EditText>(Resource.Id.edit_name);
                titl_dialog = view.FindViewById<TextView>(Resource.Id.dialogTitle);
                alertbuilder.SetCancelable(false);
                Android.Support.V7.App.AlertDialog dialog = alertbuilder.Create();
                dialog.Show();
                var createBtn = dialog.GetButton((int)DialogButtonType.Positive);
                var cancelBtn = dialog.GetButton((int)DialogButtonType.Negative);
                createBtn.Click += (sender, args) =>
                {
                    string m_username = nameedit.Text;
                    if (m_username.Equals(""))
                    {
                        titl_dialog.Text = "You have no name";
                    }
                    else
                    {
                        if (db.confirmTable(m_username) == 0)
                        {
                            MainActivity.username = m_username;
                            user_text.Text = "Welcome " + MainActivity.username;
                            Save_username();
                            dialog.Dismiss();
                        }
                        else
                        {
                            titl_dialog.Text = "This user is already";
                        }
                    }
                };
                cancelBtn.Click += (sender, args) =>
                {
                    // Dismiss dialog.
                    Console.WriteLine("I will dismiss now!");
                    dialog.Dismiss();
                };
            };

            olduser_text.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.username_select_dialog_box, null);
                Android.Support.V7.App.AlertDialog.Builder alertbuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertbuilder.SetView(view);
                userlist = view.FindViewById<ListView>(Resource.Id.list_user);
                alertbuilder.SetNegativeButton("Cancel", (EventHandler<DialogClickEventArgs>)null);
                alertbuilder.SetCancelable(false);
                Android.Support.V7.App.AlertDialog dialog = alertbuilder.Create();
                dialog.Show();
                var cancelBtn = dialog.GetButton((int)DialogButtonType.Negative);
                
                listSource = db.selectTable();
                var adapter11 = new UserListAdapter(this, listSource);
                userlist.Adapter = adapter11;
                this.userlist.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                {
                    string name111 = listSource[e.Position].Name;
                    MainActivity.username = name111;
                    user_text.Text = "Welcome " + MainActivity.username;
                    dialog.Dismiss();
                };
                cancelBtn.Click += (sender, args) =>
                {
                    // Dismiss dialog.
                    Console.WriteLine("I will dismiss now!");
                    dialog.Dismiss();
                };
            };

            button_start.Click += delegate
            {
                MainActivity.arrayname = flag_category;
                MainActivity.levelname = flag_level;
                if (!flag_category.Equals("-- Select --")){
                    Click_Start();
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Warning!");
                    alert.SetMessage("Please select a category");
                    alert.SetButton("OK", (c, ev) =>
                    {

                    });
                    alert.Show();
                }
            };

            button_exit.Click += delegate
            {
                Exit();
            };
        }

        public void Save_username()
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

        public void Show_resumlinear()
        {
            if(MainActivity.flag_resume == 0)
            {
                linear_resume.Visibility = ViewStates.Gone;
            }
            else
            {
                linear_resume.Visibility = ViewStates.Visible;
            }
            button_resume.Click += delegate
            {
                Resume_game();
            };
        }

        public void Resume_game()
        {
            if (MainActivity.flag_resume == 1)
            {
                MainActivity.flag_resume = 0;
                base.OnBackPressed();
                MainActivity.timer.Start();
            } else if (MainActivity.flag_resume == 2)
            {
                MainActivity.flag_resume = 0;
                base.OnBackPressed();
            }
        }

        public void Click_Start()
        {
            MainActivity.hint_value = 20;
            MainActivity.solved_value = 0;
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        private void Spinner_cate_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string str_category = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            flag_category = str_category;
            Get_array();
        }

        public void Get_array()
        {
            switch (flag_category)
            {
                case "Animals":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.animals_array);
                    break;
                case "Countries":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.countries_array);
                    break;
                case "Movies":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.movies_array);
                    break;
                case "Foods":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.foods_array);
                    break;
                case "School":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.school_array);
                    break;
                case "Clothes":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.clothes_array);
                    break;
                case "Colors":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.colors_array);
                    break;
                case "Family":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.family_array);
                    break;
                case "Body":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.body_array);
                    break;
                case "Sports":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.sports_array);
                    break;
                case "Vegetables":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.vegetables_array);
                    break;
                case "Doctor":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.doctor_array);
                    break;
                case "Jobs":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.jobs_array);
                    break;
                case "Fruits":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.fruits_array);
                    break;
                case "Tools":
                    MainActivity.str_array = Resources.GetStringArray(Resource.Array.tools_array);
                    break;
                default:
                    break;
            }
        }

        private void Spinner_level_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string str_level = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            flag_level = str_level;
        }

        public void Exit()
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

        public override void OnBackPressed()
        {
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastPress > 5000)
            {
                Toast.MakeText(this, "Press back again to exit", ToastLength.Long).Show();
                lastPress = currentTime;
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}