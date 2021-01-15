using System;
using System.Collections.Generic;
using System.Configuration;
using DataAccessLayer.Models;
using DataAccessLayer.ConnectedLayer;
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
using DeweyDecimalTrainerApplication.Windows;

namespace DeweyDecimalTrainerApplication.Windows
{
 
    public partial class MainMenu : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int currentUser;
        public MainMenu(int user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void BtReplaceBooks_Click(object sender, RoutedEventArgs e)
        {
           ReplaceBooks rp = new ReplaceBooks(currentUser);
           rp.Show();
           this.Close();

        }

        private void MainMenu1_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterLogin rl = new RegisterLogin();

            List<ModelRegisterLogin> getUserName = rl.GetUser(currentUser, connectionString);

            lbCurrentUser.Content = "Welcome " + getUserName[0].USER_FNAME + " !";

        }

        private void btSignOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to sign out?", "Sign Out Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                LoginRegister lr = new LoginRegister();
                lr.Show();
                this.Close();
            }
        }

        private void btIdentifyingAreas_Click(object sender, RoutedEventArgs e)
        {
            IdentifyingAreas ia = new IdentifyingAreas(currentUser);
            ia.Show();
            this.Close();
        }

        private void btFindingCallNumbers_Click(object sender, RoutedEventArgs e)
        {
            FindingCallNumbers fc = new FindingCallNumbers(currentUser);
            fc.Show();
            this.Close();
        }

        private void btLeaderboard_Click(object sender, RoutedEventArgs e)
        {
           LeaderBoard lb = new LeaderBoard(currentUser);
            lb.Show();
            this.Close();
        }
    }
}
