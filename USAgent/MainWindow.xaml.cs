using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Configuration;
using System.Diagnostics;

namespace USAgent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Start:true
        /// Stop:false
        /// </summary>
        private bool m_bState = false;
        System.Windows.Threading.DispatcherTimer m_dtimer;
        public MainWindow()
        {
            InitializeComponent();

            TimerBeginning();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void StartOrStop(object sender, MouseButtonEventArgs e)
        {
            if(m_bState)
            {
                Quit();
            }
            else
            {
                Start();
            }
        }

        private void TimerBeginning()
        {
            if(m_dtimer == null)
            {
                double dTimespan = double.Parse(ConfigurationManager.AppSettings["timespan"]);

                m_dtimer = new System.Windows.Threading.DispatcherTimer();
                m_dtimer.Interval = TimeSpan.FromSeconds(dTimespan);
                m_dtimer.Tick += dtimer_Tick;

                m_dtimer.Start();
            }
        }

        private void dtimer_Tick(object sender,EventArgs e)
        {
            Start();

            m_dtimer.Stop();
        }

        private void Start()
        {
            Process[] myProgress;
            myProgress = Process.GetProcessesByName("demo");

            if(myProgress.Length == 0)
            {
                string sCurPath = System.Environment.CurrentDirectory;
                sCurPath += "\\demo.exe";
                System.Diagnostics.Process.Start(sCurPath);
            }

            switchicon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/quit.png"));
            m_bState = true;

            WindowState = WindowState.Minimized;
        }

        private void Quit()
        {
            Process[] myProgress;
            myProgress = Process.GetProcessesByName("demo");

            foreach(Process p in myProgress)
            {
                if(p.ProcessName == "demo")
                {
                    p.Kill();
                }
            }
            switchicon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/start.png"));
            m_bState = false;
        }
    }
}
