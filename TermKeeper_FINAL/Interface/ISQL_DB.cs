using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace TermKeeper_FINAL.Interface
{
    public interface ISQL_DB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
