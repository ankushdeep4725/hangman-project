using System;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Collections.Generic;
using Android.Preferences;

namespace Hangman
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        public static Timer timer;
        Database db;

        int sec = 0;
        public static int flag_resume = 0;
        public static int top_score = 0;
        public static int flag_succes = 0;
        public static int hint_value = 20;
        public static int solved_value = 0;
        public static string arrayname;
        public static string username;
        public static string levelname;
        private TextView[] tv;
        private TextView hint_text;
        private TextView time_text;
        private TextView current_text;
        private TextView topscore_text;
        private ImageView img_main;
        private ImageView img_hint;
        private LinearLayout linear_text;
        public static String target_str;
        private String now_str;
        private ImageView back_button;
        private Button btn_a, btn_b, btn_c, btn_d, btn_l, btn_k, btn_e, btn_f, btn_g, btn_h, btn_i, btn_j, btn_m, btn_n, btn_o, btn_p, btn_q, btn_r, btn_s, btn_t, btn_u, btn_v, btn_w, btn_x, btn_y, btn_z;
        private int flag_key = 0;
        private int flag_limit = 0;
        private int limit = 0;
        private String get_key;
        private String flag_color;
        private String str_text;
        public static String[] str_array;
        private String[] str_target;
        private String[] str_now;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.activity_main);

            db = new Database();
            db.createDatabase();

            linear_text = FindViewById<LinearLayout>(Resource.Id.custom_editor);
            hint_text = FindViewById<TextView>(Resource.Id.txt_hint);
            time_text = FindViewById<TextView>(Resource.Id.txt_time);
            current_text = FindViewById<TextView>(Resource.Id.txt_current);
            topscore_text = FindViewById<TextView>(Resource.Id.txt_top);
            back_button = FindViewById<ImageView>(Resource.Id.img_back);

            switch (levelname)
            {
                case "Easy":
                    limit = 60;
                    break;
                case "Medium":
                    limit = 40;
                    break;
                case "Difficulty":
                    limit = 30;
                    break;
                default:
                    break;
            }

            if (db.getMaxTable())
            {
                top_score = db.getMaxScore();
            }
            else
            {
                top_score = 0;
            }
            
            topscore_text.Text = top_score.ToString();

            time_text.Text = " " + limit.ToString() + " ";
            hint_text.Text = hint_value.ToString();

            current_text.Text = solved_value.ToString();

            Get_targetstring();
            now_str = target_str;
            tv = new TextView[target_str.Length];

            timer = new Timer
            {
                Interval = 1000 // 1 second  
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Creat_customeditor();
            Creat_customkeyboard();
            Get_stringarray();
            Show_hint();
            Back_start();
        }

        public void Get_targetstring()
        {
            Random generator = new Random();
            int select = generator.Next(0, str_array.Length);
            target_str = str_array[select];
            Remove_item();
        }

        public void Remove_item()
        {
            var list = new List<string>(str_array);
            list.Remove(target_str);
            str_array = list.ToArray();
        }

        public void Back_start()
        {
            back_button.Click += delegate
            {
                //timer.Dispose();
                timer.Stop();
                flag_resume = 1;
                StartActivity(new Intent(Application.Context, typeof(StartActivity)));
                //Finish();
            };
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            sec++;
            int sec1 = limit - sec;
            if (sec1 == 0)
            {
                flag_succes = 2;
                //timer.Dispose();
                timer.Stop();
                StartActivity(new Intent(Application.Context, typeof(ScoreviewActivity)));
                Finish();
            }
            RunOnUiThread(() => { time_text.Text = $" {sec1} "; });
        }

        public void Show_hint()
        {
            img_hint = FindViewById<ImageView>(Resource.Id.img_hint);
            Random generator = new Random();
            
            int click_times = 0;
            int pos = 0;
            if (hint_value >= 1)
            {
                img_hint.Click += delegate
                {
                    if (hint_value >= 1)
                    {
                        click_times = click_times + 1;
                        hint_value = hint_value - 1;
                        hint_text.Text = hint_value.ToString();
                        //int hint_num;
                        
                        int num = generator.Next(0, now_str.Length);
                        String key = str_now[num];
                        for(int i=0;i< target_str.Length; i++)
                        {
                            if (str_target[i].Equals(key))
                            {
                                pos = i;
                                tv[pos].Text = key;
                            }
                        }
                        tv[pos].Text = key;
                        now_str = now_str.Replace(str_now[num], "");
                        switch (key)
                        {
                            case "A":
                                btn_a.SetBackgroundColor(Color.Transparent);
                                btn_a.Clickable = false;
                                btn_a.SetTextColor(Color.Red);
                                break;
                            case "B":
                                btn_b.SetBackgroundColor(Color.Transparent);
                                btn_b.Clickable = false;
                                btn_b.SetTextColor(Color.Red);
                                break;
                            case "C":
                                btn_c.SetBackgroundColor(Color.Transparent);
                                btn_c.Clickable = false;
                                btn_c.SetTextColor(Color.Red);
                                break;
                            case "D":
                                btn_d.SetBackgroundColor(Color.Transparent);
                                btn_d.Clickable = false;
                                btn_d.SetTextColor(Color.Red);
                                break;
                            case "E":
                                btn_e.SetBackgroundColor(Color.Transparent);
                                btn_e.Clickable = false;
                                btn_e.SetTextColor(Color.Red);
                                break;
                            case "F":
                                btn_f.SetBackgroundColor(Color.Transparent);
                                btn_f.Clickable = false;
                                btn_f.SetTextColor(Color.Red);
                                break;
                            case "G":
                                btn_g.SetBackgroundColor(Color.Transparent);
                                btn_g.Clickable = false;
                                btn_g.SetTextColor(Color.Red);
                                break;
                            case "H":
                                btn_h.SetBackgroundColor(Color.Transparent);
                                btn_h.Clickable = false;
                                btn_h.SetTextColor(Color.Red);
                                break;
                            case "I":
                                btn_i.SetBackgroundColor(Color.Transparent);
                                btn_i.Clickable = false;
                                btn_i.SetTextColor(Color.Red);
                                break;
                            case "J":
                                btn_j.SetBackgroundColor(Color.Transparent);
                                btn_j.Clickable = false;
                                btn_j.SetTextColor(Color.Red);
                                break;
                            case "K":
                                btn_k.SetBackgroundColor(Color.Transparent);
                                btn_k.Clickable = false;
                                btn_k.SetTextColor(Color.Red);
                                break;
                            case "L":
                                btn_l.SetBackgroundColor(Color.Transparent);
                                btn_l.Clickable = false;
                                btn_l.SetTextColor(Color.Red);
                                break;
                            case "M":
                                btn_m.SetBackgroundColor(Color.Transparent);
                                btn_m.Clickable = false;
                                btn_m.SetTextColor(Color.Red);
                                break;
                            case "N":
                                btn_n.SetBackgroundColor(Color.Transparent);
                                btn_n.Clickable = false;
                                btn_n.SetTextColor(Color.Red);
                                break;
                            case "O":
                                btn_o.SetBackgroundColor(Color.Transparent);
                                btn_o.Clickable = false;
                                btn_o.SetTextColor(Color.Red);
                                break;
                            case "P":
                                btn_p.SetBackgroundColor(Color.Transparent);
                                btn_p.Clickable = false;
                                btn_p.SetTextColor(Color.Red);
                                break;
                            case "Q":
                                btn_q.SetBackgroundColor(Color.Transparent);
                                btn_q.Clickable = false;
                                btn_q.SetTextColor(Color.Red);
                                break;
                            case "R":
                                btn_r.SetBackgroundColor(Color.Transparent);
                                btn_r.Clickable = false;
                                btn_r.SetTextColor(Color.Red);
                                break;
                            case "S":
                                btn_s.SetBackgroundColor(Color.Transparent);
                                btn_s.Clickable = false;
                                btn_s.SetTextColor(Color.Red);
                                break;
                            case "T":
                                btn_t.SetBackgroundColor(Color.Transparent);
                                btn_t.Clickable = false;
                                btn_t.SetTextColor(Color.Red);
                                break;
                            case "U":
                                btn_u.SetBackgroundColor(Color.Transparent);
                                btn_u.Clickable = false;
                                btn_u.SetTextColor(Color.Red);
                                break;
                            case "V":
                                btn_v.SetBackgroundColor(Color.Transparent);
                                btn_v.Clickable = false;
                                btn_v.SetTextColor(Color.Red);
                                break;
                            case "W":
                                btn_w.SetBackgroundColor(Color.Transparent);
                                btn_w.Clickable = false;
                                btn_w.SetTextColor(Color.Red);
                                break;
                            case "X":
                                btn_x.SetBackgroundColor(Color.Transparent);
                                btn_x.Clickable = false;
                                btn_x.SetTextColor(Color.Red);
                                break;
                            case "Y":
                                btn_y.SetBackgroundColor(Color.Transparent);
                                btn_y.Clickable = false;
                                btn_y.SetTextColor(Color.Red);
                                break;
                            case "Z":
                                btn_z.SetBackgroundColor(Color.Transparent);
                                btn_z.Clickable = false;
                                btn_z.SetTextColor(Color.Red);
                                break;
                            default:
                                break;
                        }
                        Compare_result();
                        Get_stringarray();
                    }
                };
            } else
            {
                img_hint.Clickable = false;
            }
        }

        public void Get_stringarray()
        {
            str_target = new String[target_str.Length];
            for (int i = 0; i < target_str.Length; i++)
            {
                str_target[i] = target_str.Substring(i, 1);
            }

            str_now = new String[now_str.Length];
            for (int i = 0; i < now_str.Length; i++)
            {
                str_now[i] = now_str.Substring(i, 1);
            }
        }

        public void Creat_customkeyboard()
        {
            btn_a = FindViewById<Button>(Resource.Id.key_a);
            btn_b = FindViewById<Button>(Resource.Id.key_b);
            btn_c = FindViewById<Button>(Resource.Id.key_c);
            btn_d = FindViewById<Button>(Resource.Id.key_d);
            btn_e = FindViewById<Button>(Resource.Id.key_e);
            btn_f = FindViewById<Button>(Resource.Id.key_f);
            btn_g = FindViewById<Button>(Resource.Id.key_g);
            btn_h = FindViewById<Button>(Resource.Id.key_h);
            btn_i = FindViewById<Button>(Resource.Id.key_i);
            btn_j = FindViewById<Button>(Resource.Id.key_j);
            btn_k = FindViewById<Button>(Resource.Id.key_k);
            btn_l = FindViewById<Button>(Resource.Id.key_l);
            btn_m = FindViewById<Button>(Resource.Id.key_m);
            btn_n = FindViewById<Button>(Resource.Id.key_n);
            btn_o = FindViewById<Button>(Resource.Id.key_o);
            btn_p = FindViewById<Button>(Resource.Id.key_p);
            btn_q = FindViewById<Button>(Resource.Id.key_q);
            btn_r = FindViewById<Button>(Resource.Id.key_r);
            btn_s = FindViewById<Button>(Resource.Id.key_s);
            btn_t = FindViewById<Button>(Resource.Id.key_t);
            btn_u = FindViewById<Button>(Resource.Id.key_u);
            btn_v = FindViewById<Button>(Resource.Id.key_v);
            btn_w = FindViewById<Button>(Resource.Id.key_w);
            btn_x = FindViewById<Button>(Resource.Id.key_x);
            btn_y = FindViewById<Button>(Resource.Id.key_y);
            btn_z = FindViewById<Button>(Resource.Id.key_z);

            btn_a.Click += delegate
            {
                flag_key = 1;
                btn_a.SetBackgroundColor(Color.Transparent);
                btn_a.Clickable = false;
                btn_a.SetTextColor(Color.White);
                Click_key();
            };
            btn_b.Click += delegate
            {
                flag_key = 2;
                btn_b.SetBackgroundColor(Color.Transparent);
                btn_b.Clickable = false;
                btn_b.SetTextColor(Color.White);
                Click_key();
            };
            btn_c.Click += delegate
            {
                flag_key = 3;
                btn_c.SetBackgroundColor(Color.Transparent);
                btn_c.Clickable = false;
                btn_c.SetTextColor(Color.White);
                Click_key();
            };
            btn_d.Click += delegate
            {
                flag_key = 4;
                btn_d.SetBackgroundColor(Color.Transparent);
                btn_d.Clickable = false;
                btn_d.SetTextColor(Color.White);
                Click_key();
            };
            btn_e.Click += delegate
            {
                flag_key = 5;
                btn_e.SetBackgroundColor(Color.Transparent);
                btn_e.Clickable = false;
                btn_e.SetTextColor(Color.White);
                Click_key();
            };
            btn_f.Click += delegate
            {
                flag_key = 6;
                btn_f.SetBackgroundColor(Color.Transparent);
                btn_f.Clickable = false;
                btn_f.SetTextColor(Color.White);
                Click_key();
            };
            btn_g.Click += delegate
            {
                flag_key = 7;
                btn_g.SetBackgroundColor(Color.Transparent);
                btn_g.Clickable = false;
                btn_g.SetTextColor(Color.White);
                Click_key();
            };
            btn_h.Click += delegate
            {
                flag_key = 8;
                btn_h.SetBackgroundColor(Color.Transparent);
                btn_h.Clickable = false;
                btn_h.SetTextColor(Color.White);
                Click_key();
            };
            btn_i.Click += delegate
            {
                flag_key = 9;
                btn_i.SetBackgroundColor(Color.Transparent);
                btn_i.Clickable = false;
                btn_i.SetTextColor(Color.White);
                Click_key();
            };
            btn_j.Click += delegate
            {
                flag_key = 10;
                btn_j.SetBackgroundColor(Color.Transparent);
                btn_j.Clickable = false;
                btn_j.SetTextColor(Color.White);
                Click_key();
            };
            btn_k.Click += delegate
            {
                flag_key = 11;
                btn_k.SetBackgroundColor(Color.Transparent);
                btn_k.Clickable = false;
                btn_k.SetTextColor(Color.White);
                Click_key();
            };
            btn_l.Click += delegate
            {
                flag_key = 12;
                btn_l.SetBackgroundColor(Color.Transparent);
                btn_l.Clickable = false;
                btn_l.SetTextColor(Color.White);
                Click_key();
            };
            btn_m.Click += delegate
            {
                flag_key = 13;
                btn_m.SetBackgroundColor(Color.Transparent);
                btn_m.Clickable = false;
                btn_m.SetTextColor(Color.White);
                Click_key();
            };
            btn_n.Click += delegate
            {
                flag_key = 14;
                btn_n.SetBackgroundColor(Color.Transparent);
                btn_n.Clickable = false;
                btn_n.SetTextColor(Color.White);
                Click_key();
            };
            btn_o.Click += delegate
            {
                flag_key = 15;
                btn_o.SetBackgroundColor(Color.Transparent);
                btn_o.Clickable = false;
                btn_o.SetTextColor(Color.White);
                Click_key();
            };
            btn_p.Click += delegate
            {
                flag_key = 16;
                btn_p.SetBackgroundColor(Color.Transparent);
                btn_p.Clickable = false;
                btn_p.SetTextColor(Color.White);
                Click_key();
            };
            btn_q.Click += delegate
            {
                flag_key = 17;
                btn_q.SetBackgroundColor(Color.Transparent);
                btn_q.Clickable = false;
                btn_q.SetTextColor(Color.White);
                Click_key();
            };
            btn_r.Click += delegate
            {
                flag_key = 18;
                btn_r.SetBackgroundColor(Color.Transparent);
                btn_r.Clickable = false;
                btn_r.SetTextColor(Color.White);
                Click_key();
            };
            btn_s.Click += delegate
            {
                flag_key = 19;
                btn_s.SetBackgroundColor(Color.Transparent);
                btn_s.Clickable = false;
                btn_s.SetTextColor(Color.White);
                Click_key();
            };
            btn_t.Click += delegate
            {
                flag_key = 20;
                btn_t.SetBackgroundColor(Color.Transparent);
                btn_t.Clickable = false;
                btn_t.SetTextColor(Color.White);
                Click_key();
            };
            btn_u.Click += delegate
            {
                flag_key = 21;
                btn_u.SetBackgroundColor(Color.Transparent);
                btn_u.Clickable = false;
                btn_u.SetTextColor(Color.White);
                Click_key();
            };
            btn_v.Click += delegate
            {
                flag_key = 22;
                btn_v.SetBackgroundColor(Color.Transparent);
                btn_v.Clickable = false;
                btn_v.SetTextColor(Color.White);
                Click_key();
            };
            btn_w.Click += delegate
            {
                flag_key = 23;
                btn_w.SetBackgroundColor(Color.Transparent);
                btn_w.Clickable = false;
                btn_w.SetTextColor(Color.White);
                Click_key();
            };
            btn_x.Click += delegate
            {
                flag_key = 24;
                btn_x.SetBackgroundColor(Color.Transparent);
                btn_x.Clickable = false;
                btn_x.SetTextColor(Color.White);
                Click_key();
            };
            btn_y.Click += delegate
            {
                flag_key = 25;
                btn_y.SetBackgroundColor(Color.Transparent);
                btn_y.Clickable = false;
                btn_y.SetTextColor(Color.White);
                Click_key();
            };
            btn_z.Click += delegate
            {
                flag_key = 26;
                btn_z.SetBackgroundColor(Color.Transparent);
                btn_z.Clickable = false;
                btn_z.SetTextColor(Color.White);
                Click_key();
            };
        }

        public void Click_key()
        {
            switch (flag_key)
            {
                case 1:
                    get_key = "A";
                    break;
                case 2:
                    get_key = "B";
                    break;
                case 3:
                    get_key = "C";
                    break;
                case 4:
                    get_key = "D";
                    break;
                case 5:
                    get_key = "E";
                    break;
                case 6:
                    get_key = "F";
                    break;
                case 7:
                    get_key = "G";
                    break;
                case 8:
                    get_key = "H";
                    break;
                case 9:
                    get_key = "I";
                    break;
                case 10:
                    get_key = "J";
                    break;
                case 11:
                    get_key = "K";
                    break;
                case 12:
                    get_key = "L";
                    break;
                case 13:
                    get_key = "M";
                    break;
                case 14:
                    get_key = "N";
                    break;
                case 15:
                    get_key = "O";
                    break;
                case 16:
                    get_key = "P";
                    break;
                case 17:
                    get_key = "Q";
                    break;
                case 18:
                    get_key = "R";
                    break;
                case 19:
                    get_key = "S";
                    break;
                case 20:
                    get_key = "T";
                    break;
                case 21:
                    get_key = "U";
                    break;
                case 22:
                    get_key = "V";
                    break;
                case 23:
                    get_key = "W";
                    break;
                case 24:
                    get_key = "X";
                    break;
                case 25:
                    get_key = "Y";
                    break;
                case 26:
                    get_key = "Z";
                    break;
                default:
                    break;
            }
            flag_limit = flag_limit + 1;
            Display_string();
        }

        public void Display_string()
        {
            int pos_k = 0;
            for (int i = 0; i < now_str.Length; i++)
            {
                if(get_key.Equals(str_now[i]))
                {
                    for(int j = 0; j < target_str.Length; j++)
                    {
                        if (str_target[j].Equals(get_key))
                        {
                            pos_k = j;
                            tv[pos_k].Text = get_key;
                        }
                    }
                    tv[pos_k].Text = get_key;
                    flag_limit = flag_limit - 1;
                    flag_color = get_key;
                    now_str = now_str.Replace(str_now[i], "");
                }
            }
            Get_stringarray();
            Display_image();
            Set_buttoncolor();
            Compare_result();
        }

        public void Set_buttoncolor()
        {
            switch (flag_color)
            {
                case "A":
                    btn_a.SetTextColor(Color.Red);
                    break;
                case "B":
                    btn_b.SetTextColor(Color.Red);
                    break;
                case "C":
                    btn_c.SetTextColor(Color.Red);
                    break;
                case "D":
                    btn_d.SetTextColor(Color.Red);
                    break;
                case "E":
                    btn_e.SetTextColor(Color.Red);
                    break;
                case "F":
                    btn_f.SetTextColor(Color.Red);
                    break;
                case "G":
                    btn_g.SetTextColor(Color.Red);
                    break;
                case "H":
                    btn_h.SetTextColor(Color.Red);
                    break;
                case "I":
                    btn_i.SetTextColor(Color.Red);
                    break;
                case "J":
                    btn_j.SetTextColor(Color.Red);
                    break;
                case "K":
                    btn_k.SetTextColor(Color.Red);
                    break;
                case "L":
                    btn_l.SetTextColor(Color.Red);
                    break;
                case "M":
                    btn_m.SetTextColor(Color.Red);
                    break;
                case "N":
                    btn_n.SetTextColor(Color.Red);
                    break;
                case "O":
                    btn_o.SetTextColor(Color.Red);
                    break;
                case "P":
                    btn_p.SetTextColor(Color.Red);
                    break;
                case "Q":
                    btn_q.SetTextColor(Color.Red);
                    break;
                case "R":
                    btn_r.SetTextColor(Color.Red);
                    break;
                case "S":
                    btn_s.SetTextColor(Color.Red);
                    break;
                case "T":
                    btn_t.SetTextColor(Color.Red);
                    break;
                case "U":
                    btn_u.SetTextColor(Color.Red);
                    break;
                case "V":
                    btn_v.SetTextColor(Color.Red);
                    break;
                case "W":
                    btn_w.SetTextColor(Color.Red);
                    break;
                case "X":
                    btn_x.SetTextColor(Color.Red);
                    break;
                case "Y":
                    btn_y.SetTextColor(Color.Red);
                    break;
                case "Z":
                    btn_z.SetTextColor(Color.Red);
                    break;
                default:
                    //Code
                    break;
            }
        }

        public void Display_image()
        {
            img_main = FindViewById<ImageView>(Resource.Id.main_img);
            switch (flag_limit)
            {
                case 1:
                    img_main.SetImageResource(Resource.Drawable.hang_1);
                    break;
                case 2:
                    img_main.SetImageResource(Resource.Drawable.hang_2);
                    break;
                case 3:
                    img_main.SetImageResource(Resource.Drawable.hang_3);
                    break;
                case 4:
                    img_main.SetImageResource(Resource.Drawable.hang_4);
                    break;
                case 5:
                    img_main.SetImageResource(Resource.Drawable.hang_5);
                    break;
                case 6:
                    img_main.SetImageResource(Resource.Drawable.hang_6);
                    flag_succes = 0;
                    timer.Stop();
                    StartActivity(new Intent(Application.Context, typeof(ScoreviewActivity)));
                    Finish();
                    break;
                default:
                    break;
            }
        }

        public void Compare_result()
        {
            str_text = "";
            for (int i = 0; i < target_str.Length; i++)
            {
                if (!tv[i].Text.Equals("_"))
                {
                    str_text = str_text + tv[i].Text;
                }
            }
            if (str_text.Equals(target_str))
            {
                flag_succes = 1;
                hint_value = hint_value + 1;
                solved_value = solved_value + 1;
                flag_resume = 0;
                timer.Stop();
                StartActivity(new Intent(Application.Context, typeof(ScoreActivity)));
                Finish();
            }
        }

        public void Creat_customeditor()
        {
            for (int index = 0; index < target_str.Length; index++)
            {
                tv[index] = new TextView(this)
                {
                    Text = "_",
                    Id = index,
                    TextSize = 30
                };
                tv[index].SetPadding(12,0,12,0);
                tv[index].SetTextColor(global::Android.Graphics.Color.ParseColor("#FFFFFF"));
                linear_text.AddView(tv[index]);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            
            switch (id)
            {
                case Resource.Id.action_exit:
                    timer.Stop();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);

                    alert.SetTitle("Confirm EXIT");

                    alert.SetMessage("Do you want to Exit?");

                    alert.SetPositiveButton("Yes", (senderAlert, args) => {
                        StartActivity(new Intent(Application.Context, typeof(ScoreviewActivity)));
                        Finish();
                    });

                    alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                        timer.Start();
                    });

                    Dialog dialog = alert.Create();

                    dialog.Show();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
	}
}

