using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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

namespace Orbit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage bulletTexture = new BitmapImage(new Uri("pack://application:,,,/Electrick_bullet.png"));

        List<Image> Bullets = new List<Image>();

        int ApeSpeedX = 0;
        int ApeSpeedY = 0;
        DispatcherTimer Timer = new DispatcherTimer();

        DispatcherTimer TerorTimer = new DispatcherTimer();

        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();

            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Start();

            TerorTimer.Tick += new EventHandler(TimerTeror_Tick);
            TerorTimer.Interval = TimeSpan.FromMilliseconds(100);
            TerorTimer.Start();

            int x = 350;
            int y = 150;

            Canvas.SetLeft(Apan, x);
            Canvas.SetTop(Apan, y);


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveApe();
            MoveBullets();
        }
        private void TimerTeror_Tick(object sender, EventArgs e)
        {

            TerorApaAI();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.A)
            {
                ApeSpeedX = -4;
            }
            if (e.Key == Key.D)
            {
                ApeSpeedX = 4;
            }
            if (e.Key == Key.W)
            {
                ApeSpeedY = -4;
            }
            if (e.Key == Key.S)
            {
                ApeSpeedY = 4;
            }


        }

        void MoveApe()
        {
            Canvas.SetLeft(Apan, Canvas.GetLeft(Apan) + ApeSpeedX);
            Canvas.SetTop(Apan, Canvas.GetTop(Apan) + ApeSpeedY);
        }



        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            ApeSpeedX = 0;
            ApeSpeedY = 0;
        }
        private void TerorApaAI()
        {
            int i = rnd.Next(1, 6);
            if (i == 1)
            {
                MoveTerorApaAI();
            }
            if (i == 2)
            {
                Terorskut();
            }
        }
        private void MoveTerorApaAI()
        {
            int movetop = rnd.Next(-20, 20);
            int moveleft = rnd.Next(-20, 10);
            Canvas.SetLeft(Tapa, Canvas.GetLeft(Tapa) + moveleft);
            Canvas.SetTop(Tapa, Canvas.GetTop(Tapa) + movetop);

        }
        private void Terorskut()
        {
            Image img = new Image() { Width = 50, Height = 20 };
            img.Source = bulletTexture;
            //Sätt positionerna

            Canvas.SetLeft(img, Canvas.GetLeft(Tapa));

            Canvas.SetTop(img, Canvas.GetTop(Tapa));
            Bullets.Add(img);

            //Lägg ut bilden på skärmen

            GameCanvas.Children.Add(img);
        }
        private void MoveBullets()
        { 
            foreach(Image img in Bullets)
            {
                Canvas.SetLeft(img, Canvas.GetLeft(img)-5);
            }
        }
    }
}