using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
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
        BitmapImage Pootexture = new BitmapImage(new Uri("pack://application:,,,/PooShot.png"));
        BitmapImage Fart1Texture = new BitmapImage(new Uri("pack://application:,,,/fartshooter.png"));
        BitmapImage Fart2Texture = new BitmapImage(new Uri("pack://application:,,,/fartling.png"));

        List <Image> Bullets = new List<Image>();
        List<Image> PooBullets = new List<Image>();

        List<FartlingRate> Fartling = new List<FartlingRate>();
        List<FartShotRate> Fartshooter = new List<FartShotRate>();
        
        int ApeSpeedX = 0;
        int ApeSpeedY = 0;
        int HPapa = 10;
        int HPslott = 100;
        int PooTimer = 0;
        double BombApaHP = 8;
        double TerrorHP = 3;
        int Respawntime = 5;
        double sentrytimer = 0;
        int Tkills = 0;
        int Bkills = 0;
        int RebuildCounter = 5;
        double RebuildCost = 20;
        bool fartcheck = true;
        bool Fatcheck = true;
        DispatcherTimer Timer = new DispatcherTimer();

        DispatcherTimer TerorTimer = new DispatcherTimer();

        DispatcherTimer Lv2Timer = new DispatcherTimer();

        DispatcherTimer SpawnTime = new DispatcherTimer();

        DispatcherTimer TickTime = new DispatcherTimer();

        Random rnd = new Random();

        MediaPlayer Rico = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();

            GameOver.Visibility = Visibility.Hidden;
            Faster_Farting.Visibility = Visibility.Visible;

            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Start();

            TerorTimer.Tick += new EventHandler(TimerTeror_Tick);
            TerorTimer.Interval = TimeSpan.FromMilliseconds(100);
            TerorTimer.Start();

            Lv2Timer.Tick += new EventHandler(Lv2);
            Lv2Timer.Interval = TimeSpan.FromMilliseconds(500);
            Lv2Timer.Start();

            SpawnTime.Tick += new EventHandler(Spawn_Tick);
            SpawnTime.Interval = TimeSpan.FromMilliseconds(1000);
            SpawnTime.Start();

            TickTime.Tick += new EventHandler(Tick_Tick);
            TickTime.Interval = TimeSpan.FromMilliseconds(2000);
            TickTime.Start();

            int x = 350;
            int y = 150;
            ApaHPtext.Text = "HP: " + HPapa;
            SlottHPtext.Text = "Slott: " + HPslott;
            BombApaHPtxt.Text = "" + BombApaHP;
            TapaHptxt.Text = "" + TerrorHP;
            RespawnTXT.Text = "" + Respawntime;
            Peng.Text = "Cash: " + sentrytimer;
            TerrorKill.Text = "TerrorApor: " + Tkills;
            BombKill.Text = "BombApor: " + Bkills;
            Rebuilds_left.Text = "Rebuilds Left: " + RebuildCounter;

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveApe();
            Movepoo();
            MoveBombapa();
            MoveBullets();
            Checkcolition();
            Dödfix();
            spawn_snabb();
            Peng.Text = "Cash: " + sentrytimer;
            if (Faster_Farting.Visibility == Visibility.Hidden)
            {
                PooTimer += 12;
            }
            else
            {
                PooTimer += 8;
            }
            if (Big_booty.Visibility == Visibility.Hidden)
            {
                PooTimer += 1;
            }
            else 
            { 
                 
            }


        }

        private void Tick_Tick(object sender, EventArgs e)
        {
            sentrytimer++;
            Peng.Text = "Cash: " + sentrytimer;
        }

        private void TimerTeror_Tick(object sender, EventArgs e)
        {

            TerorApaAI();
        }
        private void Spawn_Tick(object sender, EventArgs e)
        {
            
            if (HPapa < 1)
            { 
                Respawntime --;
                RespawnTXT.Text = "" + Respawntime;
            }

            CreatFartShot();

        }
        private void Lv2(object sender, EventArgs e)
        {
            CreatFartlingShot();

        }
        private void spawn_snabb()
        {
            if (HPapa < 1)
            {
                RespawnTXT.Visibility = Visibility.Visible;
                Apan.Visibility = Visibility.Hidden;
                Canvas.SetLeft(Apan, 1100);
                sentrytimer = 0;
            }
            else
            {
                RespawnTXT.Visibility = Visibility.Hidden;
                Apan.Visibility = Visibility.Visible;
            }
            if(Respawntime < 0)
            {
                if(Big_booty.Visibility == Visibility.Hidden)
                {
                    HPapa = 20;
                }
                else
                {
                    HPapa = 10;
                }
                Respawntime = 5;
                ApaHPtext.Text = "HP: " + HPapa;
                Canvas.SetTop(Apan, 200);
                Canvas.SetLeft(Apan, 165);
            }

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            double apay = Canvas.GetTop(Apan);
            double apax = Canvas.GetLeft(Apan);

            if (e.Key == Key.A )
            {
                ApeSpeedX = -4;
            }
            if (e.Key == Key.D )
            {
                ApeSpeedX = 4;
            }
            if (e.Key == Key.W )
            {
                ApeSpeedY = -4;
            }
            if (e.Key == Key.S)
            {
                ApeSpeedY = 4;
            }
            if (PooTimer > 120)
            {
                if (e.Key == Key.Space)
                {
                    pooskott();
                    PooTimer = 0;  
                }
            }
            if (sentrytimer > 15)
            {
                if (e.Key == Key.Q)
                {
                    Sentry1();
                    sentrytimer -= 15;
                    Peng.Text = "Cash: " + sentrytimer;
                }
            }
            if (sentrytimer > 35)
            {
                if (e.Key == Key.E)
                {
                    Sentry2();
                    sentrytimer -= 35 ;
                    Peng.Text = "Cash: " + sentrytimer;
                }
            }
            if (e.Key == Key.P)
            {
                sentrytimer += 100;
            }
            if (e.Key == Key.O)
            {
                sentrytimer += 1;
            }
            if (e.Key == Key.T)
            {
                Tkills += 1;
            }
            if (e.Key == Key.B)
            {
                Bkills += 1;
                BombKill.Text = "BombApor: " + Bkills;
            }
            if (e.Key == Key.D1 && sentrytimer>74)
            {
                if (fartcheck = true)
                {
                    Faster_Farting.Visibility = Visibility.Hidden;
                    sentrytimer -= 75;
                    fartcheck = false;
                }
            }
            if (e.Key == Key.D2 && sentrytimer > 54)
            {
                if (Fatcheck = true)
                {
                    Big_booty.Visibility = Visibility.Hidden;
                    sentrytimer -= 55;
                    HPapa += 10;
                    ApaHPtext.Text = "HP: " + HPapa;
                    Fatcheck = false;
                }
            }
        }
        void MoveApe()
        {
            double apay = Canvas.GetTop(Apan);
            double apax = Canvas.GetLeft(Apan);
            if (apax>=750 && apax <= 800)
            {
                ApeSpeedX = -1;
            }
            if (apax <= 75 )
            {
                ApeSpeedX = 1;
            }
            if (apay>=360)
            {
                ApeSpeedY = -1;
            }
            if (apay <= 0)
            {
                ApeSpeedY = 1;
            }
            Canvas.SetLeft(Apan, Canvas.GetLeft(Apan) + ApeSpeedX);
            Canvas.SetTop(Apan, Canvas.GetTop(Apan) + ApeSpeedY);
            if (Big_booty.Visibility == Visibility.Hidden)
            {
                Canvas.SetTop(Ass_extention, Canvas.GetTop(Apan) + 28);
                Canvas.SetLeft(Ass_extention, Canvas.GetLeft(Apan)+6);
            }
            else
            {

            }

        }
        private void Sentry1() 
        {
            FartShotRate fart1 = new FartShotRate() { Width = 80, Height = 50 };
            fart1.Source = Fart1Texture;
            
            Canvas.SetLeft(fart1, Canvas.GetLeft(Apan));

            Canvas.SetTop(fart1, Canvas.GetTop(Apan));
            Fartshooter.Add(fart1);

            GameCanvas.Children.Add(fart1);
        }
        private void Sentry2()
        {
            FartlingRate fart2 = new FartlingRate() { Width = 100, Height = 50 };
            fart2.Source = Fart2Texture;

            Canvas.SetLeft(fart2, Canvas.GetLeft(Apan));

            Canvas.SetTop(fart2, Canvas.GetTop(Apan));
            Fartling.Add(fart2);

            GameCanvas.Children.Add(fart2);
        }
        private void pooskott()
        {
            Image Pooimg = new Image() { Width = 30, Height = 30 };
            Pooimg.Source = Pootexture;

            Canvas.SetLeft(Pooimg, Canvas.GetLeft(Apan));

            Canvas.SetTop(Pooimg, Canvas.GetTop(Apan));

            PooBullets.Add(Pooimg);

            GameCanvas.Children.Add(Pooimg);
        }
        private void Movepoo()
        {
            foreach (Image Pooimg in PooBullets)
            {
                Canvas.SetLeft(Pooimg, Canvas.GetLeft(Pooimg) + 6);
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A) 
            {
                ApeSpeedX = 0;
            }
            if (e.Key == Key.D) 
            { 
                ApeSpeedX = 0;
            }
            if (e.Key == Key.W)
            {
                ApeSpeedY = 0;
            }
            if (e.Key == Key.S) 
            { 
                ApeSpeedY = 0;
            }
            if ( e.Key == Key.P )
            {
                sentrytimer += 100;
            }
            if (e.Key == Key.O)
            {
                sentrytimer += 1;
            }
            if (e.Key == Key.T)
            {
                Tkills += 1;
                TerrorKill.Text = "TerrorApor: " + Tkills;
            }
            if (e.Key == Key.B)
            {
                Bkills += 1;
                BombKill.Text = "BombApor: " + Bkills;
                HPslott--;
                SlottHPtext.Text = "Slott: " + HPslott;
            }
        }
        private void CreatFartShot()
        {
            foreach(var fart in Fartshooter)
            {
                Image Pooimg = new Image() { Width = 30, Height = 30 };
                Pooimg.Source = Pootexture;

                Canvas.SetLeft(Pooimg, Canvas.GetLeft(fart));

                Canvas.SetTop(Pooimg, Canvas.GetTop(fart));

                PooBullets.Add(Pooimg);

                GameCanvas.Children.Add(Pooimg);
            }
        }
        private void CreatFartlingShot()
        {
            foreach (var fart in Fartling)
            {
                
                Image Pooimg = new Image() { Width = 30, Height = 30 };
                Pooimg.Source = Pootexture;

                Canvas.SetLeft(Pooimg, Canvas.GetLeft(fart));

                Canvas.SetTop(Pooimg, Canvas.GetTop(fart));

                PooBullets.Add(Pooimg);

                GameCanvas.Children.Add(Pooimg);
            }
        }
        private void Checkcolition()
        {
            Rect aparect = new Rect(Canvas.GetLeft(Apan), Canvas.GetTop(Apan), Apan.Width, Apan.Height);
            Rect slottrect = new Rect(Canvas.GetLeft(Slott), Canvas.GetTop(Slott), Slott.Width, Slott.Height);
            Rect terrorrect = new Rect(Canvas.GetLeft(Tapa), Canvas.GetTop(Tapa), Tapa.Width, Tapa.Height);
            Rect bombrect = new Rect(Canvas.GetLeft(Bombapa), Canvas.GetTop(Bombapa), Bombapa.Width, Bombapa.Height);
            Rect elimrect = new Rect(Canvas.GetLeft(Eliminator), Canvas.GetTop(Eliminator), Eliminator.Width, Eliminator.Height);
            Rect telimrect = new Rect(Canvas.GetLeft(TEliminator), Canvas.GetTop(TEliminator), TEliminator.Width, TEliminator.Height);
            //if ()
            //{
            //    HPslott -= 1;
            //    SlottHPtext.Text = "Slott: " + HPslott;
            //}
            
            
            foreach (var bullet in Bullets)
            {
                Rect skottrect = new Rect(Canvas.GetLeft(bullet), Canvas.GetTop(bullet), bullet.Width, bullet.Height);
                if (skottrect.IntersectsWith(aparect))
                {
                    HPapa -= 1;
                    ApaHPtext.Text = "HP: " + HPapa;
                    bullet.Visibility = Visibility.Hidden;

                    GameCanvas.Children.Remove(bullet);
                }
                if (skottrect.IntersectsWith(telimrect))
                {
                    bullet.Visibility = Visibility.Hidden;

                    GameCanvas.Children.Remove(bullet);
                }
                foreach(var fs1 in Fartshooter)
                {
                    Rect fs1Rect = new Rect(Canvas.GetLeft(fs1), Canvas.GetTop(fs1), fs1.Width, fs1.Height);
                    if (fs1Rect.IntersectsWith(skottrect))
                    {
                        fs1.Life -= 0.06666;
                        bullet.Visibility = Visibility.Hidden;

                        GameCanvas.Children.Remove(bullet);
                        fs1.Opacity = fs1.Life;
                    }
                    if(fs1.Life <= 0)
                    {
                        GameCanvas.Children.Remove(fs1);
                    }
                }
                Fartshooter.RemoveAll(x => x.Life <= 0);

                foreach (var fs2 in Fartling)
                {
                    Rect fs2Rect = new Rect(Canvas.GetLeft(fs2), Canvas.GetTop(fs2), fs2.Width, fs2.Height);
                    if (fs2Rect.IntersectsWith(skottrect))
                    {
                        fs2.Life2 -= 0.02857;
                        bullet.Visibility = Visibility.Hidden;

                        GameCanvas.Children.Remove(bullet);
                        fs2.Opacity = fs2.Life2;
                    }
                    if (fs2.Life2 <= 0)
                    {
                        GameCanvas.Children.Remove(fs2);
                    }
                }
                Fartling.RemoveAll(x => x.Life2 <= 0);

            }
            if (HPslott < 1)
            {
                GameOver.Visibility = Visibility.Visible;
            }
            Bullets.RemoveAll(x => x.Visibility == Visibility.Hidden);

            foreach(var skott in PooBullets)
            {
                Rect poorect = new Rect(Canvas.GetLeft(skott), Canvas.GetTop(skott), skott.Width, skott.Height);
                if (poorect.IntersectsWith(bombrect))
                {
                    BombApaHP -= 1;
                    BombApaHPtxt.Text = "" + BombApaHP;
                    skott.Visibility = Visibility.Hidden;

                    GameCanvas.Children.Remove(skott);
                }
                if (poorect.IntersectsWith(terrorrect))
                {
                    TerrorHP -= 1;
                    TapaHptxt.Text = "" + TerrorHP;

                    skott.Visibility = Visibility.Hidden;

                    GameCanvas.Children.Remove(skott);
                }
                if (poorect.IntersectsWith(elimrect))
                {
                    skott.Visibility = Visibility.Hidden;

                    GameCanvas.Children.Remove(skott);
                }

            }
            PooBullets.RemoveAll(x => x.Visibility == Visibility.Hidden);
        }
        private void TerorApaAI()
        {
            int i = rnd.Next(1, 9);
            if (i == 1)
            {
                MoveTerorApaAI();
            }
            if (i == 2)
            {
                Terorskut();
            }
            if (i == 3)
            {
                MoveTerorApaAI();
                Terorskut();
            }
            if (i >= 7)
            {
                MoveTerorApaAI();
            }
        }
        private void MoveTerorApaAI()
        {
            int movetop = rnd.Next(-20, 20);
            int moveleft = rnd.Next(-20, 10);
            int Ty = rnd.Next(10, 340);
            if (Canvas.GetLeft(Tapa) + moveleft < 65)
            {
                Canvas.SetLeft(Tapa, 750);
                Canvas.SetTop(Tapa, Ty);
                HPslott -= 5;
                SlottHPtext.Text = "Slott: " + HPslott;
                TerrorHP = 3;
                TapaHptxt.Text = "" + TerrorHP;
            }
            else
            {
                Canvas.SetLeft(Tapa, Canvas.GetLeft(Tapa) + moveleft);
            }
           
            Canvas.SetTop(Tapa, Canvas.GetTop(Tapa) + movetop);
            if (Canvas.GetTop(Tapa) + movetop < -10)
            {
                Canvas.SetTop(Tapa, 10);
            }

            if (Canvas.GetTop(Tapa) + movetop > 360)
            {
                Canvas.SetTop(Tapa, 340);
            }

            

        }
        private void Dödfix()
        {
            int Tyny = rnd.Next(10, 340);
            if (TerrorHP < 1)
            {
                Canvas.SetLeft(Tapa, 750);
                Canvas.SetTop(Tapa, Tyny);

                sentrytimer++;

                if (Tkills >= 100)
                {
                    ScalingT();
                }
                else
                {
                    TerrorHP = 3;
                }
                TapaHptxt.Text = "" + TerrorHP;
                Tkills++;
                TerrorKill.Text= "TerrorApor: " + Tkills;
            }
            Canvas.SetTop(TapaHptxt, Canvas.GetTop(Tapa) - 20);
            Canvas.SetLeft(TapaHptxt, Canvas.GetLeft(Tapa) + 25);
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
        private void MoveBombapa()
        {
            Canvas.SetTop(Bombapa, Canvas.GetTop(Bombapa));
            Canvas.SetLeft(Bombapa, Canvas.GetLeft(Bombapa) - 1);

            int By = rnd.Next(10, 340);
            if (Canvas.GetLeft(Bombapa) < 65)
            {
                Canvas.SetLeft(Bombapa, 750);
                Canvas.SetTop(Bombapa, By);
                HPslott -= 15;
                SlottHPtext.Text = "Slott: " + HPslott;
                BombApaHP = 8;
                BombApaHPtxt.Text = "" + BombApaHP;
            }
            else
            {
                Canvas.SetLeft(Bombapa, Canvas.GetLeft(Bombapa));
            }

            Canvas.SetTop(BombEX, Canvas.GetTop(Bombapa)-35);
            Canvas.SetLeft(BombEX, Canvas.GetLeft(Bombapa)-30);
            Canvas.SetTop(BombApaHPtxt, Canvas.GetTop(Bombapa) - 20);
            Canvas.SetLeft(BombApaHPtxt, Canvas.GetLeft(Bombapa) + 40);

            if (Canvas.GetLeft(Bombapa)<75)
            {
                BombEX.Visibility = Visibility.Visible;
                Bombapa.Visibility = Visibility.Hidden;
            }
            else
            {
                BombEX.Visibility = Visibility.Hidden;
                Bombapa.Visibility = Visibility.Visible;
            }

            if (BombApaHP < 1)
            {
                Canvas.SetLeft(Bombapa, 750);
                Canvas.SetTop(Bombapa, By);
                sentrytimer+=3;
                if (Bkills >= 100)
                {
                    ScalingB();
                }
                else
                {
                    BombApaHP = 8;
                }
                BombApaHPtxt.Text = "" + BombApaHP;
                Bkills++;
                BombKill.Text = "BombApor: " + Bkills;
            }
        }
        private void ScalingT()
        {
            Double Tscale = Tkills * 0.01;
            TerrorHP = 3 * Tscale;
            TerrorKill.Text = "TerrorApor: " + Tkills;
        }
        private void ScalingB()
        {
            Double Bscale = Bkills * 0.01;
            BombApaHP = 8 * Bscale;
            BombKill.Text = "BombApor: " + Bkills;
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sentrytimer > RebuildCost && HPslott <= 80 && RebuildCounter>0)
            {
                sentrytimer -= RebuildCost;
                HPslott += 20;
                SlottHPtext.Text = "Slott: "+HPslott;
                RebuildCounter--;
                Rebuilds_left.Text = "Rebuilds Left: " + RebuildCounter;
                RebuildCost = RebuildCost * 1.5;
                RebuildTXT.Text =  RebuildCost.ToString();
                if (RebuildCounter < 1)
                {
                    Rebuild.Visibility = Visibility.Hidden;
                    RebuildTXT.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}