using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using System.Collections;
using System.Collections.Generic;

namespace Hangman
{
    public class ViewHolder1 : Java.Lang.Object
    {
        public TextView txtName { get; set; }
    }
    public class UserListAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Users> listUsers;
        public UserListAdapter(Activity activity, List<Users> listUsers)
        {
            this.activity = activity;
            this.listUsers = listUsers;
        }
        public override int Count
        {
            get { return listUsers.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return listUsers[position].Id;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.userRow, parent, false);
            var txtName = view.FindViewById<TextView>(Resource.Id.txt_username2);
            txtName.Text = listUsers[position].Name;
            return view;
        }
    }
}