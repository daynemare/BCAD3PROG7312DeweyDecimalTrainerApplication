using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataAccessLayer.Models;
using DataAccessLayer.ConnectedLayer;
using System.Configuration;
using DeweyDecimalTrainerApplication.Windows;
using DeweyDecimalTrainerApplication.Models;

namespace DeweyDecimalTrainerApplication.Windows
{

    public partial class ReplaceBooks : Window
    {
        //Storing the connection string for passing through to the DataAccessLayer
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private Stopwatch stopwatch;
        private Timer timer;
        private const string startTimeDisplay = "00:00:00";

        
        private string randomNumber = "";
        private string callNumber = "";
        private string author = "";
        private int subjectAreaNumber = 0;
        private string stringSubjectAreaNumber;
        private int subSectionNumber = 0;
        private string stringSubSectionNumber;
        private Random rand = new Random();
        private Random random = new Random();
        private int currentUser;
        private string pb;

        //Lists used for holding user and sorted data
        public static List<DataGridBind> unsortedListString = new List<DataGridBind>();
        public static List<DataGridBind> usersList = new List<DataGridBind>();
        private List<string> sortedList = new List<string>();
        private LinkedList<string> userOrderLinkedList = new LinkedList<string>();

        List<ModelLeaderBoard> getAllRecords = new List<ModelLeaderBoard>();

        //Stores the current champions time
        public object ChampTime { get; private set; }

        public ReplaceBooks(int user)
        {
            InitializeComponent();
            //stores the current users ID
            currentUser = user;

            tbTimeDisplay.Text = startTimeDisplay;
            stopwatch = new Stopwatch();
            timer = new Timer(1000);
            timer.Elapsed += OnTimerElapse;
        }

        //When the timer and stopwatch are active method constantly updates the timer text box value on the GUI
        private void OnTimerElapse(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                tbTimeDisplay.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            }
            );
        }

        private void ReplaceBooks_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLeaderBoardPersonalBest();

        
        }


        //Updates labels related to personal best score and champion
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

                List<ModelLeaderBoard> getPersonalBest = lg.GetPersonalBestReplacingBooks(mlb, connectionString);

                if (getPersonalBest.Count == 1)
                {
 
                        lbPersonalBest.Text = getPersonalBest[0].REPLACING_BOOKS_PERSONAL_BEST.ToString();
                        pb = getPersonalBest[0].REPLACING_BOOKS_PERSONAL_BEST.ToString();

                }

                getAllRecords = lg.GetAllLeadBoardRecords(connectionString);

                SelectionSortBestTime();

                lbChampTime.Text = getAllRecords[0].REPLACING_BOOKS_PERSONAL_BEST;

                List<ModelLeaderBoard> getBestUserId = lg.GetChampionNameReplacingBooks(getAllRecords[0].REPLACING_BOOKS_PERSONAL_BEST.ToString(), connectionString);

                int id = getBestUserId[0].USER_LIBRARY_CARD_ID;

                List<ModelRegisterLogin> getBestUser = rl.GetUser(id, connectionString);
                tbChampionName.Text = getBestUser[0].USER_FNAME + " " + getBestUser[0].USER_LNAME;
               ChampTime = getAllRecords[0].REPLACING_BOOKS_PERSONAL_BEST;
            }
            catch (Exception)
            {
                tbChampionName.Text = "No Champion Yet !";

            }
        }

        
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        //this method programmatically selects a row on the datagrid used for book sorting
        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
         
            object item = dataGrid.Items[rowIndex]; 
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
               
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (row != null)
            {
                DataGridCell cell = GetCell(dataGrid, row, 0);
                if (cell != null)
                    cell.Focus();
            }
        }


        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                   
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        //This method is used to format generated numbers below 100 to a call number format by adding the appropriate number of zeros
        private string AddZeros(int san)
        {

            if (san <= 10 && san > 0)
            {
                return "00" + san.ToString();

            }
            else if (san < 100 && san > 10)
            {
                return "0" + san.ToString();

            }

            return san.ToString();

        }

        //This method generates part of a randomized call number - the subject area number and subsection number
        private string GenerateRandomNumber()
        {

            subjectAreaNumber = rand.Next(1000);

            stringSubjectAreaNumber = AddZeros(subjectAreaNumber);

            subSectionNumber = rand.Next(10000);

            stringSubSectionNumber = subSectionNumber.ToString();

            return stringSubjectAreaNumber + "." + stringSubSectionNumber;

        }
        //This method generates part of a randomized call number - the author
        private string GenerateRandomAuthor()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[3];


            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;


        }

        //Sorting Algorithm used to sort the callnumbers in ascending order to check against the list the user has manually sorted
        private void SelectionSortUserBooks()
        {

            int smallest;
            for (int i = 0; i < unsortedListString.Count - 1; i++)
            {
                smallest = i;
                string minValue = unsortedListString[i].CALL_NUMBER;

                for (int index = i + 1; index < unsortedListString.Count; index++)
                {
                    if (unsortedListString[index].CALL_NUMBER.CompareTo(minValue) < 0)
                    {
                        smallest = index;
                        minValue = unsortedListString[index].CALL_NUMBER;
                    }
                }
                SwapUserBooks(i, smallest);

            }

        }
        //Selection sort swap helper method for the call number Sorting Algorithm 
        private void SwapUserBooks(int first, int second)
        {
            string temporary = unsortedListString[first].CALL_NUMBER;
            unsortedListString[first].CALL_NUMBER = unsortedListString[second].CALL_NUMBER;
            unsortedListString[second].CALL_NUMBER = temporary;
        }

        //Sorting Algorithm to calculate best user time 
        private void SelectionSortBestTime()
        {


            int smallest;
            for (int i = 0; i < getAllRecords.Count - 1; i++)
            {
                smallest = i;
                string minValue = getAllRecords[i].REPLACING_BOOKS_PERSONAL_BEST;

                for (int index = i + 1; index < getAllRecords.Count; index++)
                {
                    if (getAllRecords[index].REPLACING_BOOKS_PERSONAL_BEST.CompareTo(minValue) < 0)
                    {
                        smallest = index;
                        minValue = getAllRecords[index].REPLACING_BOOKS_PERSONAL_BEST;
                    }
                }
                SwapBestTime(i, smallest);

            }

        }

        //Selection sort swap helper method for the Sorting Algorithm to calculate best user time 
        private void SwapBestTime(int first, int second)
        {
            string temporary = getAllRecords[first].REPLACING_BOOKS_PERSONAL_BEST;
            getAllRecords[first].REPLACING_BOOKS_PERSONAL_BEST = getAllRecords[second].REPLACING_BOOKS_PERSONAL_BEST;
            getAllRecords[second].REPLACING_BOOKS_PERSONAL_BEST = temporary;
        }

        //when the main menu button is clicked show the main menu window and close this window
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

        
        private void btDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string swapAValue;
                int swapAIndex;
                string swapBValue;

                swapAIndex = dgCallNumbers.SelectedIndex;

                swapAValue = usersList[swapAIndex].CALL_NUMBER;
                swapBValue = usersList[swapAIndex + 1].CALL_NUMBER;
                usersList[swapAIndex].CALL_NUMBER = swapBValue;
                usersList[swapAIndex + 1].CALL_NUMBER = swapAValue;

                dgCallNumbers.Items.Clear();

                foreach (var callNumber in usersList)
                {
                    DataGridBind dgb = new DataGridBind
                    {
                        CALL_NUMBER = callNumber.CALL_NUMBER
                    };


                    dgCallNumbers.Items.Add(dgb);
                }

                SelectRowByIndex(dgCallNumbers, swapAIndex + 1);

            }
            catch (Exception)
            {

                MessageBox.Show("Please select a Call Number row to move before clicking a direction button");
            }


        }

        private void btUp_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string swapAValue;
                int swapAIndex;
                string swapBValue;

                swapAIndex = dgCallNumbers.SelectedIndex;

                swapAValue = usersList[swapAIndex].CALL_NUMBER;
                swapBValue = usersList[swapAIndex - 1].CALL_NUMBER;
                usersList[swapAIndex].CALL_NUMBER = swapBValue;
                usersList[swapAIndex - 1].CALL_NUMBER = swapAValue;

                dgCallNumbers.Items.Clear();

                foreach (var callNumber in usersList)
                {
                    DataGridBind dgb = new DataGridBind
                    {
                        CALL_NUMBER = callNumber.CALL_NUMBER
                    };


                    dgCallNumbers.Items.Add(dgb);
                }
                SelectRowByIndex(dgCallNumbers, swapAIndex - 1);

            }
            catch (Exception)
            {

                MessageBox.Show("Please select a Call Number row to move before clicking a direction button");
            }


        }

        private void dgCallNumbers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCallNumbers.SelectedIndex == 0)
            {
                btUp.IsEnabled = false;
                btDown.IsEnabled = true;
            }
            else if (dgCallNumbers.SelectedIndex == 9)
            {
                btDown.IsEnabled = false;
                btUp.IsEnabled = true;
            }
            else
            {
                btUp.IsEnabled = true;
                btDown.IsEnabled = true;
            }
        }

        private void btStartStop_Click(object sender, RoutedEventArgs e)
        {

            if (btStartStop.Content.Equals("START") || btStartStop.Content.Equals("TRY AGAIN?"))
            {
                stopwatch.Reset();
                tbTimeDisplay.Text = startTimeDisplay;

                btStartStop.Content = "STOP";
                btDown.IsEnabled = true;
                btUp.IsEnabled = true;
                unsortedListString.Clear();
                usersList.Clear();
                userOrderLinkedList.Clear();
                sortedList.Clear();

                for (int i = 0; i < 10; i++)
                {
                    randomNumber = GenerateRandomNumber();
                    author = GenerateRandomAuthor();
                    callNumber = randomNumber + " " + author;

                    DataGridBind dgb = new DataGridBind
                    {
                        CALL_NUMBER = callNumber
                    };
                    unsortedListString.Insert(i, dgb);
                    usersList.Insert(i, dgb);

                  
                }

                dgCallNumbers.Items.Clear();

                foreach (var callNumber in unsortedListString)
                {
                    DataGridBind dgb = new DataGridBind
                    {
                        CALL_NUMBER = callNumber.CALL_NUMBER
                    };


                    dgCallNumbers.Items.Add(dgb);
                }

                stopwatch.Start();
                timer.Start();

            }
            else if(btStartStop.Content.Equals("STOP"))
            {
                LearningGames lg = new LearningGames();

                string timeScore = tbTimeDisplay.Text;
                stopwatch.Stop();
                timer.Stop();
               

                for (int i = 0; i < 10; i++)
                {

                    DataGridBind dgb = new DataGridBind
                    {
                        CALL_NUMBER = usersList[i].CALL_NUMBER
                    };

                    userOrderLinkedList.AddLast(usersList[i].CALL_NUMBER.ToString());

                }

                SelectionSortUserBooks();

                foreach (var item in unsortedListString)
                {
                    sortedList.Add(item.CALL_NUMBER);
                }

                bool equal = userOrderLinkedList.SequenceEqual(sortedList);

             

                if (equal)
                {
                  
                    ModelLeaderBoard ml = new ModelLeaderBoard
                    {
                        USER_LIBRARY_CARD_ID = currentUser,
                        REPLACING_BOOKS_PERSONAL_BEST = timeScore

                    };

                    List<ModelLeaderBoard> getPersonalBest = lg.GetPersonalBestReplacingBooks(ml, connectionString);
               
                    if(pb.Equals("Not Set"))
                    {
                     
                        List<ModelLeaderBoard> getLeaderBoardRecords = lg.GetAllLeadBoardRecords(connectionString);

                        if (getLeaderBoardRecords.Count==0)
                        {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                            MessageBox.Show("Congratulations ! You won, set your first PERSONAL BEST and are the first CHAMPION for this game mode ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST !");
                        }
                        else if(TimeSpan.Compare(TimeSpan.Parse(timeScore), TimeSpan.Parse(ChampTime.ToString())) == -1)
                        {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                            MessageBox.Show("Congratulations ! You won, set your first PERSONAL BEST and are the new CHAMPION for this game mode ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST !");
                        }
                        else
                        {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                            MessageBox.Show("Congratulations ! You won and set your first PERSONAL BEST ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST !");
                        }

                        UpdateLeaderBoardPersonalBest();
                    }
                    else if(TimeSpan.Compare(TimeSpan.Parse(timeScore), TimeSpan.Parse(ChampTime.ToString())) == -1)
                    {
                       
                        if (TimeSpan.Compare(TimeSpan.Parse(timeScore), TimeSpan.Parse(getPersonalBest[0].REPLACING_BOOKS_PERSONAL_BEST.ToString())) == -1)
                        {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                            MessageBox.Show("Congratulations ! You WON, beat your PERSONAL BEST time and are the new CHAMPION ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST for this game mode!");
                            UpdateLeaderBoardPersonalBest();
                        }
                        else
                        {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                            MessageBox.Show("Congratulations ! You WON and are the new CHAMPION ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST for this game mode!");
                            UpdateLeaderBoardPersonalBest();
                        }

                    }
                    else if (TimeSpan.Compare(TimeSpan.Parse(timeScore), TimeSpan.Parse(getPersonalBest[0].REPLACING_BOOKS_PERSONAL_BEST.ToString()))==-1)
                    {
                            lg.UpdateNewPersonalBestReplacingBooks(ml, connectionString);
                        MessageBox.Show("Congratulations ! You WON and beat your PERSONAL BEST time ! It took you " + timeScore + " to complete the game! Try again to beat your PERSONAL BEST or to become library CHAMPION for this game mode!");
                        UpdateLeaderBoardPersonalBest();
                    }
                    else
                    {
                        MessageBox.Show("Congratulations ! You WON ! It took you " + timeScore+ " to complete the game! Try again to beat your PERSONAL BEST or to become library CHAMPION for this game mode!");
                    }
                    
                    
                }
                else
                {
                    MessageBox.Show("SORRY You LOSE ! Unfortunately the CALL NUMBER order was INCORRECT! TRY AGAIN to have another chance at WINNING this game mode !");
                }

                btStartStop.Content = "TRY AGAIN?";
            }
           


        }

     
    }
}
