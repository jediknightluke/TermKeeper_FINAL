using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TermKeeper_FINAL.Droid.Interface;
using TermKeeper_FINAL.Interface;


[assembly: Xamarin.Forms.Dependency(typeof(SQL_DB))]

namespace TermKeeper_FINAL.Droid.Interface
{
    public class SQL_DB : ISQL_DB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docPath, "MySqlLite.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}