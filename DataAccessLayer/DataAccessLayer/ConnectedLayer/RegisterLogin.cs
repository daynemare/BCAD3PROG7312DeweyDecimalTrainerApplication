using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using Dapper;

namespace DataAccessLayer.ConnectedLayer
{//This class perfroms Sql operations relating to Login and Registration data
    public class RegisterLogin
     {
        
        public void InsertUserRegData(ModelRegisterLogin user, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelRegisterLogin>("Insert Into USER(USER_LIBRARY_CARD_ID, USER_PASSWORD,USER_FNAME,USER_LNAME) Values" + $"('{user.USER_LIBRARY_CARD_ID}','{user.USER_PASSWORD}','{user.USER_FNAME}','{user.USER_LNAME}')", new DynamicParameters());
            }

        }
   
        public List<ModelRegisterLogin> GetAllFromUser(string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelRegisterLogin>("SELECT * FROM USER", new DynamicParameters());
                return list.ToList();
            }
        }

        public List<ModelRegisterLogin> GetUser(int id,string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelRegisterLogin>("SELECT * FROM USER WHERE USER_LIBRARY_CARD_ID= '" + id + "'", new DynamicParameters());
                return list.ToList();
            }
        }

    }
}
