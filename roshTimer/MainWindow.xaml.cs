using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace roshTimer
{
    public partial class MainWindow : Window
    {
        SoundPlayer sound;
        DispatcherTimer timer;
        Ini ini;

        int timur;
        int microtimur;
        const int roshMax = 20;//660
        const int roshMin = 10;//480 
        bool isTimerActive;
        bool minEnd = false;

        public MainWindow()
        {
            InitializeComponent();
            Run();
        }
        void timer_tick(object sender, EventArgs e)
        {
            if (timur - 1 > roshMax - roshMin)
            {
                timur = timur - 1;
                Timer.Text = $"{TimeForamt(timur)}";


                microtimur = microtimur - 1;
                minTimer.Text = $"{TimeForamt(microtimur)}";

            }
            else if (microtimur - 1 <= 0 && timur - 1 >= 0)
            {
                minTimer.Text = "0:00";
                timur--;
                if (!minEnd) {
                    microtimur = 0;
                    sound.Play();
                    minEnd = true;
                }
                Timer.Text = $"{TimeForamt(timur)}";
            }
            else
            {
                Timer.Text = "0:00";

                timer.Stop();
                sound.Play();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            timer.Stop();
            timur = roshMax;
            microtimur = roshMin;
            isTimerActive = false;
            Timer.Text = TimeForamt(timur);
            minTimer.Text = TimeForamt(microtimur);
        }

        private void Timer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isTimerActive)
            {
                timur = roshMax;
                microtimur = roshMin;
                minEnd = false;
                timer.Start();
                isTimerActive = !isTimerActive;
            }
            else
            {
                string textbuffer = "Roshan may spawn after ";
                textbuffer += TimeForamt(microtimur);
                textbuffer += " Guaranteed appearance after ";
                textbuffer += TimeForamt(timur);
                Clipboard.SetText(textbuffer);

            }
        }
        string TimeForamt(int time)
        {
            string fTime;
            if (time % 60 >= 10)
            {
                fTime = $"{ time / 60}:{ time % 60}";
            }
            else
            {
                fTime = $"{ time / 60}:0{ time % 60}";
            }

            return fTime;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            WritePos();
        }
        void Run()
        {
            timer = new DispatcherTimer();
            timer.Tick += timer_tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timur = roshMax;
            microtimur = roshMin;

            sound = new SoundPlayer(Properties.Resources.din_ding);

            ini = new Ini("settings.ini");

            SetSettings();
        }
        void SetSettings()
        {

            SetWindowPos();

            void SetWindowPos()
            {
                if (ini.KeyExists("PositionLeft", "Main"))
                    this.Left = double.Parse(ini.ReadINI("Main", "PositionLeft"));
                else
                    WritePos();
                if (ini.KeyExists("PositionTop", "Main"))
                    this.Top = double.Parse(ini.ReadINI("Main", "PositionTop"));
                else
                    WritePos();
            }
        }
        void WritePos()
        {
            ini.Write("Main", "PositionLeft", this.Left.ToString());
            ini.Write("Main", "PositionTop", this.Top.ToString());
        }
        
    }   
    
}
