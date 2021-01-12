using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace DeweyDecimalTrainerApplication.Windows
{
    
    public partial class Splash : Window
    {
        private DispatcherTimer dt = new DispatcherTimer();

        public Splash()
        {
            InitializeComponent();

            dt.Tick += new EventHandler(Dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 4);
            dt.Start();
            
        }
        //method that displays the LoginRegister window after the declared time above occurs
        private void Dt_Tick(object sender, EventArgs e)
        {
            dt.Stop();
          
            LoginRegister lr = new LoginRegister();
            lr.Show();
          
            this.Close();
        }
    }

}
