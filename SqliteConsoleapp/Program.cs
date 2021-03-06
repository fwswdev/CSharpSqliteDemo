﻿/*
 * Thanks to http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/
 * For the wonderful tutorial
 * 
 * For password protected SQLite tutorial http://stackoverflow.com/questions/12190672/can-i-password-encrypt-sqlite-database
 * */

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteConsoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbFile = "c:/test3d.db";
            SQLiteConnection.CreateFile(dbFile);

            using (var m_dbConnection =
                new SQLiteConnection(String.Format("Data Source={0};Version=3;", dbFile)))
            {

                m_dbConnection.Open();
                m_dbConnection.ChangePassword("Mypass");

                Action<string> funcSqlStr = delegate(string x)
                {
                    string sqlStr = x;
                    var commandDel = new SQLiteCommand(sqlStr, m_dbConnection);
                    commandDel.ExecuteNonQuery();
                    commandDel.Dispose();
                };


                string sql = "create table highscores (name varchar(20), score int)";
                funcSqlStr(sql);

                sql = "insert into highscores (name, score) values ('Me', 3000)";
                funcSqlStr(sql);

                sql = "insert into highscores (name, score) values ('Myself', 6000)";
                funcSqlStr(sql);

                sql = "insert into highscores (name, score) values ('And I', 9001)";
                funcSqlStr(sql);

                sql = "select * from highscores order by score desc";
                var command = new SQLiteCommand(sql, m_dbConnection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
                reader.Dispose();

            }

            Console.ReadKey();
            

        }
    }
}
