using DataAccessLayer.ConnectedLayer;
using DataAccessLayer.Models;
using DeweyDecimalTrainerApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeweyDecimalTrainerApplication.Windows
{
    /// <summary>
    /// Interaction logic for LeaderBoard.xaml
    /// </summary>
    public partial class LeaderBoard : Window
    {
        private List<ModelLeaderBoard> mlb;
        private List<ModelRegisterLogin> user;
        //Storing the connection string for passing through to the DataAccessLayer
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int currentUser;

        public LeaderBoard(int user)
        { 
            InitializeComponent();
            currentUser = user;
        }

        private void btToMain_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to return to main menu? ", "Return to Main Menu Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MainMenu mm = new MainMenu(currentUser);
                mm.Show();
                this.Close();
            }
        }

        private void WinLeaderboard_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateReplacingBooksLeaderboard();
            PopulateIdentifyingAreasLeaderboard();
            PopulateFindingCallNumbersLeaderboard();

        }

        private void PopulateReplacingBooksLeaderboard()
        {
            LearningGames lg = new LearningGames();
            RegisterLogin rl = new RegisterLogin();

            mlb = lg.GetAllLeadBoardRecords(connectionString).OrderBy(a => a.REPLACING_BOOKS_PERSONAL_BEST).ToList();

            int i = 1;
            foreach (var item in mlb)
            {

                user = rl.GetUser(item.USER_LIBRARY_CARD_ID, connectionString);

                DataGridBind dgb = new DataGridBind
                {
                    POSITION = i.ToString(),
                    NAME = user[0].USER_FNAME + " " + user[0].USER_LNAME,
                    SCORE = item.REPLACING_BOOKS_PERSONAL_BEST
                };


                dgReplacingBooks.Items.Add(dgb);
                i++;
            }
        }

        private void PopulateIdentifyingAreasLeaderboard()
        {
            LearningGames lg = new LearningGames();
            RegisterLogin rl = new RegisterLogin();

            mlb = lg.GetAllLeadBoardRecords(connectionString).OrderByDescending(a => a.IDENTIFYING_AREAS_PERSONAL_BEST).ToList();

            int i = 1;
            foreach (var item in mlb)
            {

                user = rl.GetUser(item.USER_LIBRARY_CARD_ID, connectionString);

                DataGridBind dgb = new DataGridBind
                {
                    POSITION = i.ToString(),
                    NAME = user[0].USER_FNAME + " " + user[0].USER_LNAME,
                    SCORE = item.IDENTIFYING_AREAS_PERSONAL_BEST.ToString()
                };


                dgIdentifyingAreas.Items.Add(dgb);
                i++;
            }
        }

        private void PopulateFindingCallNumbersLeaderboard()
        {
            LearningGames lg = new LearningGames();
            RegisterLogin rl = new RegisterLogin();

            mlb = lg.GetAllLeadBoardRecords(connectionString).OrderByDescending(a => a.FINDING_CALL_NUMBERS_PERSONAL_BEST).ToList();

            int i = 1;
            foreach (var item in mlb)
            {

                user = rl.GetUser(item.USER_LIBRARY_CARD_ID, connectionString);

                DataGridBind dgb = new DataGridBind
                {
                    POSITION = i.ToString(),
                    NAME = user[0].USER_FNAME + " " + user[0].USER_LNAME,
                    SCORE = item.FINDING_CALL_NUMBERS_PERSONAL_BEST.ToString()
                };


                dgFindingCallNumbers.Items.Add(dgb);
                i++;
            }
        }

    }
}
