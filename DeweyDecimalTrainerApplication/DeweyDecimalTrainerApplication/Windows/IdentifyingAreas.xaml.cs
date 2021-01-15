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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeweyDecimalTrainerApplication.Windows;
using DataAccessLayer.Models;
using DataAccessLayer.ConnectedLayer;

namespace DeweyDecimalTrainerApplication.Windows
{

     
    public partial class IdentifyingAreas : Window
    {
        //Storing the connection string for passing through to the DataAccessLayer
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //global variables for storing game data
        private int alternateCheck;
        private int userCurrentScore;
        private int userCurrentIncorrect;
        private int roundScore;
     
        private int roundIncorrect;
        //Dictionary for storing top-level categories and relating call numbers
        private Dictionary<string, string> callNumbersAndDescriptions = new Dictionary<string, string>();

        private List<ModelLeaderBoard> getAllRecords = new List<ModelLeaderBoard>();
      
        //properties for managing conditions
        private bool isGameStart { get; set; }
        private bool isGameWin { get; set; }
        private bool isChamp { get; set; }

        private int currentUser;

        public IdentifyingAreas(int user)
        {
            InitializeComponent();
            //stores the current users ID
            currentUser = user;
           
        }

        private void BtToMain_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to return to main menu? If a game is still in session all current progress will be lost ", "Return to Main Menu Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MainMenu mm = new MainMenu(currentUser);
                mm.Show();
                this.Close();
            }

            
        }

        private void PopulateDictionary()
        {
            //Populating dictionary with call number data
            callNumbersAndDescriptions.Add("0_call", "000");
            callNumbersAndDescriptions.Add("1_call", "100");
            callNumbersAndDescriptions.Add("2_call", "200");
            callNumbersAndDescriptions.Add("3_call", "300");
            callNumbersAndDescriptions.Add("4_call", "400");
            callNumbersAndDescriptions.Add("5_call", "500");
            callNumbersAndDescriptions.Add("6_call", "600");
            callNumbersAndDescriptions.Add("7_call", "700");
            callNumbersAndDescriptions.Add("8_call", "800");
            callNumbersAndDescriptions.Add("9_call", "900");

            //Populating dictionary with description data
            callNumbersAndDescriptions.Add("0_des", "Computer science, information & general works");
            callNumbersAndDescriptions.Add("1_des", "Philosophy & psychology");
            callNumbersAndDescriptions.Add("2_des", "Religion");
            callNumbersAndDescriptions.Add("3_des", "Social sciences");
            callNumbersAndDescriptions.Add("4_des", "Language");
            callNumbersAndDescriptions.Add("5_des", "Science");
            callNumbersAndDescriptions.Add("6_des", "Technology");
            callNumbersAndDescriptions.Add("7_des", "Arts & recreation");
            callNumbersAndDescriptions.Add("8_des", "Literature");
            callNumbersAndDescriptions.Add("9_des", "History & geography");

        }

        private void IdentifyingAreasWin_Loaded(object sender, RoutedEventArgs e)
        {
            isChamp = false;
            isGameWin = false;
            PopulateDictionary();
            isGameStart = false;
            UpdateLeaderBoardPersonalBest();

        }
        private void UpdateLeaderBoardPersonalBest()
        {

            try
            {
                LearningGames lg = new LearningGames();
                RegisterLogin rl = new RegisterLogin();

                ModelLeaderBoard mlb = new ModelLeaderBoard
                {
                    USER_LIBRARY_CARD_ID = currentUser
                };

                List<ModelLeaderBoard> getPersonalBest = lg.GetPersonalBestIdentifyingAreas(mlb, connectionString);

               
                tbPBSCore.Text = getPersonalBest[0].IDENTIFYING_AREAS_PERSONAL_BEST.ToString();

                

                if (getPersonalBest.Count == 1)
                {
                    tbPBSCore.Text = getPersonalBest[0].IDENTIFYING_AREAS_PERSONAL_BEST.ToString();
                }



                
                getAllRecords = lg.GetIdentifyingAreasChampion(connectionString);

                if (getAllRecords[0].IDENTIFYING_AREAS_PERSONAL_BEST == 0)
                {
                    tbChampionName.Text = "No Champion Yet !";
                }
                else
                {

                    tbChampScore.Text = getAllRecords[0].IDENTIFYING_AREAS_PERSONAL_BEST.ToString();

                    List<ModelLeaderBoard> getBestUserId = lg.GetChampionNameIdentifyingAreas(getAllRecords[0].IDENTIFYING_AREAS_PERSONAL_BEST.ToString(), connectionString);

                    int id = getBestUserId[0].USER_LIBRARY_CARD_ID;

                    if(id == currentUser)
                    {
                        isChamp = true;
                    }

                    List<ModelRegisterLogin> getBestUser = rl.GetUser(id, connectionString);
                    tbChampionName.Text = getBestUser[0].USER_FNAME + " " + getBestUser[0].USER_LNAME;
                }
   
            }
            catch (Exception)
            {
                tbChampionName.Text = "No Champion Yet !";

            }
        }

        private void AlternateQuestionType(int startIdentifier)
        {

            

            int lengthCall;
            int lengthDesc;

            if (startIdentifier <10)
            {
                lengthCall = 4;
                lengthDesc = 7;

                PopulateListView(lengthCall, lengthDesc);

            }
            else
            {
                lengthCall = 7;
                lengthDesc = 4;

                PopulateListView(lengthCall, lengthDesc);
            }

        }

        private void PopulateListView(int lengthCall, int lengthDesc)
        {
            int[] IndexArrayOne = new int[10];
            int[] IndexArrayTwo = new int[7];
            char[] answerChar = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            for (int i = 0; i < IndexArrayOne.Length; i++)
            {
                IndexArrayOne[i] = i;
              
            }

            Random rndOne = new Random();
            int[] randomIndexArrayOne = IndexArrayOne.OrderBy(x => rndOne.Next()).ToArray();
            
            for (int i = 0; i < IndexArrayTwo.Length; i++)
            {
                IndexArrayTwo[i] = randomIndexArrayOne[i];
            }



            Random rndTwo = new Random();
            int[] randomIndexArrayTwo = IndexArrayTwo.OrderBy(x => rndTwo.Next()).ToArray();
                  randomIndexArrayTwo = IndexArrayTwo.OrderBy(x => rndTwo.Next()).ToArray();

            try
            {
                for (int i = 0; i < lengthCall; i++)
                {

                    if (lengthCall == 4)
                    {
                        ListViewQuestions.Items.Add("Q" + (i + 1) + "-" + callNumbersAndDescriptions[randomIndexArrayOne[i].ToString() + "_call"]);
                      
                    }
                    else
                    {
                        ListViewAnswers.Items.Add(answerChar[i] + "-" + callNumbersAndDescriptions[randomIndexArrayTwo[i].ToString() + "_call"]);
                    }

                }

                for (int i = 0; i < lengthDesc; i++)
                {

                    if (lengthDesc == 4)
                    {
                        ListViewQuestions.Items.Add("Q" + (i + 1) + "-" + callNumbersAndDescriptions[randomIndexArrayOne[i].ToString() + "_des"]);
                    }
                    else
                    {
                        ListViewAnswers.Items.Add(answerChar[i] + "-" + callNumbersAndDescriptions[randomIndexArrayTwo[i].ToString() + "_des"]);
                    }

                }

            }
            catch (Exception err)
            {
                Console.WriteLine("An error occured: " + err);
            }
        }

        private void btStartCheck_Click(object sender, RoutedEventArgs e)
        {
            if(btStartCheck.Content.Equals("Check Answers"))
            {
               

                if (cbQuestionOne.SelectedIndex == 0 || cbQuestionTwo.SelectedIndex == 0 || cbQuestionThree.SelectedIndex == 0 || cbQuestionFour.SelectedIndex == 0)
                {
                    MessageBox.Show("Please ensure an answer is selected for each question from the drop down lists above before checking answers.");
                }
                else
                {
                    btStartCheck.Content = "Next Question";
                    //Get value of first index from questions list view
                    //Q1
                    string questionOneQuestionValue = ListViewQuestions.Items[0].ToString();
                    int questionOneAnswerIndex = cbQuestionOne.SelectedIndex - 1;
                    string questionOneAnswerValue = ListViewAnswers.Items[questionOneAnswerIndex].ToString();
                    questionOneQuestionValue = questionOneQuestionValue.Substring(3, questionOneQuestionValue.Length - 3);
                    questionOneAnswerValue = questionOneAnswerValue.Substring(2, questionOneAnswerValue.Length - 2);
                    //Q2
                    string questionTwoQuestionValue = ListViewQuestions.Items[1].ToString();
                    int questionTwoAnswerIndex = cbQuestionTwo.SelectedIndex - 1;         
                    string questionTwoAnswerValue = ListViewAnswers.Items[questionTwoAnswerIndex].ToString();
                    questionTwoQuestionValue = questionTwoQuestionValue.Substring(3, questionTwoQuestionValue.Length - 3);
                    questionTwoAnswerValue = questionTwoAnswerValue.Substring(2, questionTwoAnswerValue.Length - 2);
                    //Q3
                    string questionThreeQuestionValue = ListViewQuestions.Items[2].ToString();
                    int questionThreeAnswerIndex = cbQuestionThree.SelectedIndex - 1;
                    string questionThreeAnswerValue = ListViewAnswers.Items[questionThreeAnswerIndex].ToString();
                    questionThreeQuestionValue = questionThreeQuestionValue.Substring(3, questionThreeQuestionValue.Length - 3);
                    questionThreeAnswerValue = questionThreeAnswerValue.Substring(2, questionThreeAnswerValue.Length - 2);
                    //Q4
                    string questionFourQuestionValue = ListViewQuestions.Items[3].ToString();
                    int questionFourAnswerIndex = cbQuestionFour.SelectedIndex - 1;
                    string questionFourAnswerValue = ListViewAnswers.Items[questionFourAnswerIndex].ToString();
                    questionFourQuestionValue = questionFourQuestionValue.Substring(3, questionFourQuestionValue.Length - 3);
                    questionFourAnswerValue = questionFourAnswerValue.Substring(2, questionFourAnswerValue.Length - 2);


                     int correctQuestions=0;

                    //GET DICTIONARY KEYS
                    //Q1
                    var questionOneQuestionKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionOneQuestionValue).Key;
                    var questionOneAnswerKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionOneAnswerValue).Key;
                    questionOneQuestionKey = questionOneQuestionKey.Substring(0, 1);
                    questionOneAnswerKey = questionOneAnswerKey.Substring(0, 1);
                    if (Int32.Parse(questionOneQuestionKey) == Int32.Parse(questionOneAnswerKey))
                    {
                        userCurrentScore++;
                        roundScore++;
                        correctQuestions++;


                    }
                    else
                    {
                        roundIncorrect++;
                    }
                   
                  

                    //Q2
                    var questionTwoQuestionKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionTwoQuestionValue).Key;
                    var questionTwoAnswerKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionTwoAnswerValue).Key;
                    questionTwoQuestionKey = questionTwoQuestionKey.Substring(0, 1);
                    questionTwoAnswerKey = questionTwoAnswerKey.Substring(0, 1);
                    if (Int32.Parse(questionTwoQuestionKey) == Int32.Parse(questionTwoAnswerKey))
                    {
                        userCurrentScore++;
                        roundScore++;
                        correctQuestions++;

                    }
                    else
                    {
                        roundIncorrect++;
                    }

                    //Q3
                    var questionThreeQuestionKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionThreeQuestionValue).Key;
                    var questionThreeAnswerKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionThreeAnswerValue).Key;
                    questionThreeQuestionKey = questionThreeQuestionKey.Substring(0, 1);
                    questionThreeAnswerKey = questionThreeAnswerKey.Substring(0, 1);
                    if (Int32.Parse(questionThreeQuestionKey) == Int32.Parse(questionThreeAnswerKey))
                    {
                        userCurrentScore++;
                        correctQuestions++;
                        roundScore++;

                    }
                    else
                    {
                        roundIncorrect++;
                    }

                    //Q4
                    var questionFourQuestionKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionFourQuestionValue).Key;
                    var questionFourAnswerKey = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == questionFourAnswerValue).Key;
                    questionFourQuestionKey = questionFourQuestionKey.Substring(0, 1);
                    questionFourAnswerKey = questionFourAnswerKey.Substring(0, 1);
                    if (Int32.Parse(questionFourQuestionKey) == Int32.Parse(questionFourAnswerKey))
                    {
                        userCurrentScore++;
                        roundScore++;
                        correctQuestions++;

                    }
                    else
                    {
                        roundIncorrect++;
                    }


                    if (userCurrentScore <= 0)
                    {
                        userCurrentScore = 0;
                    }

                    cbQuestionOne.SelectedIndex = 0;
                    cbQuestionTwo.SelectedIndex = 0;
                    cbQuestionThree.SelectedIndex = 0;
                    cbQuestionFour.SelectedIndex = 0;
                    cbQuestionOne.IsEnabled = false;
                    cbQuestionTwo.IsEnabled = false;
                    cbQuestionThree.IsEnabled = false;
                    cbQuestionFour.IsEnabled = false;
                    ListViewQuestions.IsEnabled = false;
                    ListViewAnswers.IsEnabled = false;
                    ListViewQuestions.Items.Clear();
                    ListViewAnswers.Items.Clear();

                    btEndSessionEnabled.Visibility = Visibility.Visible;
                    btEndSessionDisabled.Visibility = Visibility.Hidden;

                    userCurrentIncorrect = userCurrentIncorrect + roundIncorrect;
                    roundScore = roundScore - roundIncorrect;
                    userCurrentScore = userCurrentScore - roundIncorrect;

                 

                    if (roundScore == 4)
                    {
                        userCurrentScore++;
                        tbCurrentScore.Text = userCurrentScore.ToString();
                        MessageBox.Show("You answered all questions correctly that round and earned a bonus point ! Well done ! Your current score is " + userCurrentScore + " points. " +
                            "Click NEXT QUESTION to continue answering questions or END SESSION to end the current game.", "Round Results");
                    }
                    else
                    {
                       
                        if ( roundScore <=0)
                        {
                            
                            roundScore = 0;
                        }

                        if(userCurrentScore <= 0)
                        {
                            userCurrentScore = 0;
                        }
                        tbCurrentScore.Text = userCurrentScore.ToString();
                        MessageBox.Show("You answered " + correctQuestions + " question(s) correctly and lost " + roundIncorrect + " point(s) that round. Your score that round was " + roundScore + ".Your current score is " + userCurrentScore + " points. " +
                                        "Click NEXT QUESTION to continue answering questions or END SESSION to end the current game.", "Round Results");

                      
                    }

                    if (userCurrentScore > Int32.Parse(tbChampScore.Text)&& userCurrentScore > Int32.Parse(tbPBSCore.Text)&&isChamp==false)
                    {
                        InsertUpdateRecords();
                        UpdateLeaderBoardPersonalBest();
                        MessageBox.Show("Congratulations, you beat your PERSONAL BEST and are the new CHAMPION for this game mode! ", "You are the new CHAMPION and beat your PERSONAL BEST  !");
                    }
                    else if (userCurrentScore > Int32.Parse(tbPBSCore.Text))
                    {

                        InsertUpdateRecords();

                        UpdateLeaderBoardPersonalBest();
                        MessageBox.Show("Congratulations, you beat your PERSONAL BEST for this game mode! ", "Personal Best Beaten !");
                    }
                    if (userCurrentScore >= 15 && isGameWin == false)
                    {

                        MessageBox.Show("You have earned enough points to win the game. Click END SESSION to win or carry on to further improve your score","Points Notification");
                        isGameWin = true;

                    }


                    roundScore = 0;
                    roundIncorrect = 0;
                 
                }


            }
            else if(isGameStart)
            {
                ListViewQuestions.IsEnabled = true;
                ListViewAnswers.IsEnabled = true;
                cbQuestionOne.IsEnabled = true;
                cbQuestionTwo.IsEnabled = true;
                cbQuestionThree.IsEnabled = true;
                cbQuestionFour.IsEnabled = true;
                btEndSessionEnabled.Visibility = Visibility.Hidden;
                btEndSessionDisabled.Visibility = Visibility.Visible;

                if (alternateCheck < 10) 
                {
                    AlternateQuestionType(5);
                    alternateCheck = 15;
                }
                else
                {
                    AlternateQuestionType(15);
                    alternateCheck = 5;
                }

                btStartCheck.Content = "Check Answers";
            }
            else
            {
              

                ListViewQuestions.IsEnabled = true;
                ListViewAnswers.IsEnabled = true;
                cbQuestionOne.IsEnabled = true;
                cbQuestionTwo.IsEnabled = true;
                cbQuestionThree.IsEnabled = true;
                cbQuestionFour.IsEnabled = true;

                Random rndStart = new Random();
                int startIdentifier = rndStart.Next(0, 20);

                if (startIdentifier<10)
                {
                    alternateCheck = 15;
                }
                else
                {
                    alternateCheck = 5;
                }

                AlternateQuestionType(startIdentifier);

                isGameStart = true;
                btStartCheck.Content = "Check Answers";
            }
           
        }

        private void btEndSessionEnabled_Click(object sender, RoutedEventArgs e)
        {

            cbQuestionOne.SelectedIndex = 0;
            cbQuestionTwo.SelectedIndex = 0;
            cbQuestionThree.SelectedIndex = 0;
            cbQuestionFour.SelectedIndex = 0;
            cbQuestionOne.IsEnabled = false;
            cbQuestionTwo.IsEnabled = false;
            cbQuestionThree.IsEnabled = false;
            cbQuestionFour.IsEnabled = false;
            ListViewQuestions.IsEnabled = false;
            ListViewAnswers.IsEnabled = false;
            ListViewQuestions.Items.Clear();
            ListViewAnswers.Items.Clear();
         

            if ( userCurrentScore<15)
            {

                MessageBox.Show("Unfortunately you did not achieve enough points in the session to win. Better luck next time ! Your final score was " + userCurrentScore + " points. You got " + userCurrentIncorrect + " question(s) incorrect during that session.", "Sorry You Lose. ");            
                
            }
            else
            {
                MessageBox.Show("Congratulations, you won in that session! Your final score was " + userCurrentScore + " points. You got " + userCurrentIncorrect + " question(s) incorrect.", "Congratulations you won ! ");
            }

            userCurrentScore = 0;
            userCurrentIncorrect = 0;
            tbCurrentScore.Text = "0";
            btStartCheck.Content = "Try Again?";
            btEndSessionEnabled.Visibility = Visibility.Hidden;
            btEndSessionDisabled.Visibility = Visibility.Visible;
        }

        private void InsertUpdateRecords()
        {

            LearningGames lg = new LearningGames();
            ModelLeaderBoard mlb = new ModelLeaderBoard
            {
                USER_LIBRARY_CARD_ID = currentUser,
                IDENTIFYING_AREAS_PERSONAL_BEST = userCurrentScore
            };


            List<ModelLeaderBoard> rec = lg.GetPersonalBestIdentifyingAreas(mlb, connectionString);
            if (rec.Count == 0)
            {
                lg.InsertNewPersonalBestIdentifyingAreas(mlb, connectionString);
            }
            else
            {
                lg.UpdateNewPersonalBestIdentifyingAreas(mlb, connectionString);
            }
        }
    }
}
