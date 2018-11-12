using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Hangman
{
    class Userdatabase
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.CreateTable<Users>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Add or Insert Operation  

        public bool insertIntoTable(Users users)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Insert(users);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Users> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    return connection.Query<Users>("SELECT * FROM Users").ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Edit Operation  

        public bool updateTable(Users users)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Query<Users>("UPDATE Users set Name=? Where Id=?", users.Name, users.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Delete Data Operation  

        public bool removeTable(Users users)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Delete(users);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Select Operation  

        public bool getTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Query<Users>("SELECT * FROM Users");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        //confirm username
        public int confirmTable(string username)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    var uuu = connection.Query<Users>("SELECT * FROM Users Where Name=?", username);
                    return uuu.Count;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }

    }
}