using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using System.Collections;
using System.Collections.Generic;


namespace Hangman
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        public TextView txtNumber { get; set; }
        public TextView txtScore { get; set; }
        public TextView txttoprate { get; set; }
    }
    public class MyScoreListAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Userscore> listUserscore;
        public MyScoreListAdapter(Activity activity, List<Userscore> listUserscore)
        {
            this.activity = activity;
            this.listUserscore = listUserscore;
        }
        public override int Count
        {
            get { return listUserscore.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return listUserscore[position].Id;
        }
       
                public override View GetView(int position, View convertView, ViewGroup parent)
                {
          
                    return null;
                   
                 }
     
    }
}