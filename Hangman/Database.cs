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
    class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    connection.CreateTable<Userscore>();
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

        public bool insertIntoTable(Userscore userscore)
        {
            //Log.Info("AAA","AAA");
            try
            {
                //Log.Info("BBB", "BBB");
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    connection.Insert(userscore);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Userscore> selectTable(String level)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    return connection.Query<Userscore>("SELECT * FROM Userscore Where Level=? ORDER BY Score DESC", level).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Edit Operation  

        public bool updateTable(Userscore userscore)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    connection.Query<Userscore>("UPDATE Userscore set Level=?, Name=?, Score=? Where Name=?", userscore.Level, userscore.Name, userscore.Score, userscore.Name);
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

        public bool removeTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    connection.DeleteAll<Userscore>();
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

        public bool getTable(String level)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    var mmm = connection.Query<Userscore>("SELECT * FROM Userscore Where Level=?", level);
                    if(mmm.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool getUser(String username)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    var mmm = connection.Query<Userscore>("SELECT * FROM Userscore Where Name=?", username);
                    if (mmm.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public int getCount(String level)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    var mmm = connection.Query<Userscore>("SELECT * FROM Userscore Where Level=?", level);
                    return mmm.Count;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }

        public bool getMaxTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    var mmm = connection.Query<Userscore>("SELECT * FROM Userscore");
                    if (mmm.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public int getMaxScore()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Scores.db")))
                {
                    var hhh = connection.Query<Userscore>("SELECT * FROM Userscore ORDER BY Score DESC");
                    if (hhh.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return hhh[0].Score;
                    }
                    
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