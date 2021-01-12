using System;
using System.Collections.Generic;
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
using DataAccessLayer.Models;
using DataAccessLayer.ConnectedLayer;
using System.Data.SQLite;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using DeweyDecimalTrainerApplication.Windows;

namespace DeweyDecimalTrainerApplication.Windows
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class LoginRegister : Window
    {
        //Declaration of the Database connection string to be passed to the Data Access Layer DLL
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //Declaration of a global RegisterLogin Class object - this class resides with in the Data Access Layer DLL
        private RegisterLogin cl = new RegisterLogin();

        public LoginRegister()
        {
            InitializeComponent();

        }

        //This method checks to see if password and username input from each textbox matches password and username data stored in the USER table in the application database.  
    
        public void LoginUser()
        {
            ModelRegisterLogin user = new ModelRegisterLogin();

            List<ModelRegisterLogin> list = cl.GetAllFromUser(connectionString);

            try
            {
                user = list.Find((ModelRegisterLogin x) => x.USER_LIBRARY_CARD_ID == Int32.Parse(tbUsernameLog.Text));


                    if (user== null)
                    {
                        MessageBox.Show("Incorrect username or password\nPlease recheck your credentials and try again", "Error");
                        tbUsernameLog.Text = "";
                        tbPasswordLog.Password = "";
                    }                    
                    else if(user.USER_LIBRARY_CARD_ID.ToString().Equals(tbUsernameLog.Text) && user.USER_PASSWORD.ToString().Equals(tbPasswordLog.Password))
                    {
                        MessageBox.Show("Login Successful !\nWelcome " + user.USER_FNAME.ToString());
                        MainMenu mm = new MainMenu(Int32.Parse(tbUsernameLog.Text));
                        mm.Show();
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password\nPlease recheck your credentials and try again", "Error");
                        tbUsernameLog.Text = "";
                        tbPasswordLog.Password = "";
                    }
               



            }
            catch (System.Exception)
            {
                MessageBox.Show("Incorrect username or password\nPlease recheck your credentials and try again", "Error");
                tbUsernameLog.Text = "";
                tbPasswordLog.Password = "";
            }
        }
        
        //This method inputs new user data into the database
        public void RegisterNewUser()
        {
            LearningGames lg = new LearningGames();

            try
            {
                var login = Int32.Parse(tbUsername.Text);
                var password = tbPassword.Password;
                var fName = tbFName.Text;
                var lName = tbLName.Text;

                var c = new ModelRegisterLogin
                {
                    USER_LIBRARY_CARD_ID = login,
                    USER_PASSWORD = password,
                    USER_FNAME = fName,
                    USER_LNAME = lName
                };

                cl.InsertUserRegData(c, connectionString);

                var ml = new ModelLeaderBoard
                {
                    USER_LIBRARY_CARD_ID = login,
                    REPLACING_BOOKS_PERSONAL_BEST = "Not Set",
                    IDENTIFYING_AREAS_PERSONAL_BEST = 0,
                    FINDING_CALL_NUMBERS_PERSONAL_BEST = 0

                };

                lg.InsertNewBlankLeaderboardRecord(ml,connectionString);
                



                MessageBox.Show($"Registration Successful!" +
              "\nYou may now Login,  " + tbFName.Text + " !");
                tabLoginRegister.SelectedIndex = 1;
            }
            catch (System.Exception)
            {
                MessageBox.Show("User already exists or the input for LIBRARY CARD ID is not a number\nPlease try again", "Error");
                tbUsername.Text = "";
                tbPassword.Password = "";
            }
        }
        
        
        //This method ensures that the user's text box and password box data meets application constraints before calling the RegisterNewUser() method.
 
        public void ValidateAndInputNewUser()
        {
            if (tbUsername.Text.Length == 0 || tbPassword.Password.Length == 0 || tbFName.Text.Length == 0 || tbLName.Text.Length == 0)
            {
                MessageBox.Show("Fields can not be empty,\n Please fill in a valid library card ID number, password, first name and last name.", "Error");

            }
            else if (tbPassword.Password.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters in length,\n Please try again.", "Error");

            }
            else if (tbUsername.Text.Length != 8)
            {
                MessageBox.Show("Library Card ID number must be 8 characters in length,\n Please try again.", "Error");
            }
            else
            {
                RegisterNewUser();

            }

        }
        
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            ValidateAndInputNewUser();

        }

        private void BtSaveLog_Click(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }


    }
    }
