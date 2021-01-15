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
{
    public class LearningGames
    {
        public void InsertNewPersonalBestReplacingBooks(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("Insert Into LEADERBOARD(USER_LIBRARY_CARD_ID,REPLACING_BOOKS_PERSONAL_BEST)Values" + $"('{modelLeaderBoard.USER_LIBRARY_CARD_ID}','{modelLeaderBoard.REPLACING_BOOKS_PERSONAL_BEST}')", new DynamicParameters());
            }

        }

        public void InsertNewPersonalBestIdentifyingAreas(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("Insert Into LEADERBOARD(USER_LIBRARY_CARD_ID,IDENTIFYING_AREAS_PERSONAL_BEST)Values" + $"('{modelLeaderBoard.USER_LIBRARY_CARD_ID}','{modelLeaderBoard.IDENTIFYING_AREAS_PERSONAL_BEST}')", new DynamicParameters());
            }

        }

        public void InsertNewPersonalBestFindingCallNumbers(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("Insert Into LEADERBOARD(USER_LIBRARY_CARD_ID,FINDING_CALL_NUMBERS_PERSONAL_BEST)Values" + $"('{modelLeaderBoard.USER_LIBRARY_CARD_ID}','{modelLeaderBoard.FINDING_CALL_NUMBERS_PERSONAL_BEST}')", new DynamicParameters());
            }

        }

        public void InsertNewBlankLeaderboardRecord(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("Insert Into LEADERBOARD(USER_LIBRARY_CARD_ID,REPLACING_BOOKS_PERSONAL_BEST,IDENTIFYING_AREAS_PERSONAL_BEST,FINDING_CALL_NUMBERS_PERSONAL_BEST)Values" + $"('{modelLeaderBoard.USER_LIBRARY_CARD_ID}','{modelLeaderBoard.REPLACING_BOOKS_PERSONAL_BEST}','{modelLeaderBoard.IDENTIFYING_AREAS_PERSONAL_BEST}','{modelLeaderBoard.FINDING_CALL_NUMBERS_PERSONAL_BEST}')", new DynamicParameters());
            }

        }


        public List<ModelLeaderBoard> GetPersonalBestReplacingBooks(ModelLeaderBoard modelLeaderBoard,string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT REPLACING_BOOKS_PERSONAL_BEST FROM LEADERBOARD WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID+ "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetPersonalBestIdentifyingAreas(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT IDENTIFYING_AREAS_PERSONAL_BEST FROM LEADERBOARD WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID + "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetPersonalBestFindingCallNumbers(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT FINDING_CALL_NUMBERS_PERSONAL_BEST FROM LEADERBOARD WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID + "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetChampionNameReplacingBooks(string record, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT USER_LIBRARY_CARD_ID FROM LEADERBOARD WHERE REPLACING_BOOKS_PERSONAL_BEST = '" + record + "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetChampionNameIdentifyingAreas(string record, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT USER_LIBRARY_CARD_ID FROM LEADERBOARD WHERE IDENTIFYING_AREAS_PERSONAL_BEST = '" + record + "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetChampionNameFindingCallNumbers(string record, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT USER_LIBRARY_CARD_ID FROM LEADERBOARD WHERE FINDING_CALL_NUMBERS_PERSONAL_BEST = '" + record + "'", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetAllLeadBoardRecords(string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT * FROM LEADERBOARD", new DynamicParameters());
                return list.ToList();
            }

        }
        public List<ModelLeaderBoard> GetIdentifyingAreasChampion(string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT * FROM LEADERBOARD ORDER BY IDENTIFYING_AREAS_PERSONAL_BEST DESC", new DynamicParameters());
                return list.ToList();
            }

        }

        public List<ModelLeaderBoard> GetFindingCallNumbersChampion(string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var list = cnn.Query<ModelLeaderBoard>("SELECT * FROM LEADERBOARD ORDER BY FINDING_CALL_NUMBERS_PERSONAL_BEST DESC", new DynamicParameters());
                return list.ToList();
            }

        }

        public void UpdateNewPersonalBestReplacingBooks(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("UPDATE LEADERBOARD SET REPLACING_BOOKS_PERSONAL_BEST='" + modelLeaderBoard.REPLACING_BOOKS_PERSONAL_BEST+ "'WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID+ "'", new DynamicParameters());
            }

        }

        public void UpdateNewPersonalBestIdentifyingAreas(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("UPDATE LEADERBOARD SET IDENTIFYING_AREAS_PERSONAL_BEST='" + modelLeaderBoard.IDENTIFYING_AREAS_PERSONAL_BEST + "'WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID + "'", new DynamicParameters());
            }

        }

        public void UpdateNewPersonalBestFindingCallNumbers(ModelLeaderBoard modelLeaderBoard, string cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(cs))
            {
                var cmd = cnn.Query<ModelLeaderBoard>("UPDATE LEADERBOARD SET FINDING_CALL_NUMBERS_PERSONAL_BEST='" + modelLeaderBoard.FINDING_CALL_NUMBERS_PERSONAL_BEST + "'WHERE USER_LIBRARY_CARD_ID = '" + modelLeaderBoard.USER_LIBRARY_CARD_ID + "'", new DynamicParameters());
            }

        }


    }
}
