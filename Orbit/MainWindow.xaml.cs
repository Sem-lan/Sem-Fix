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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Orbit.Game;
using Orbit.Game.Enemies;

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
        int gameovertimer = 0;
        // Old timers retained but no longer started (logic moved to render loop in partial class)
        DispatcherTimer Timer = new DispatcherTimer();
        DispatcherTimer TerorTimer = new DispatcherTimer();
        DispatcherTimer Lv2Timer = new DispatcherTimer();
        DispatcherTimer SpawnTime = new DispatcherTimer();
        DispatcherTimer TickTime = new DispatcherTimer();

        Random rnd = new Random();

        MediaPlayer Rico = new MediaPlayer();

        private Player _player;
        private TerrorApe _terrorApe;
        private BombApe _bombApe;

        public MainWindow()
        {
            InitializeComponent();

            _player = new Player(Apan, Ass_extention, HPapa);
            _terrorApe = new TerrorApe(Tapa, rnd, TerrorHP);
            _bombApe = new BombApe(Bombapa, BombEX, rnd, BombApaHP);

            // Performance tweaks
            if (bulletTexture.CanFreeze) bulletTexture.Freeze();
            if (Pootexture.CanFreeze) Pootexture.Freeze();
            if (Fart1Texture.CanFreeze) Fart1Texture.Freeze();
            if (Fart2Texture.CanFreeze) Fart2Texture.Freeze();
            RenderOptions.SetBitmapScalingMode(GameCanvas, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(GameCanvas, EdgeMode.Aliased);

            Titlescreen.Visibility = Visibility.Visible;
            GameOver.Visibility = Visibility.Hidden;
            Faster_Farting.Visibility = Visibility.Visible;

            // Removed multiple DispatcherTimers to reduce overhead. Unified game loop initialized here.
            InitializeGameLoop();

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

            Canvas.SetTop(Titlescreen, -45);
            Canvas.SetLeft(Titlescreen, 0);

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Legacy method (unused now). Logic moved to FrameUpdate in partial class.
        }

        private void Tick_Tick(object sender, EventArgs e) { }
        private void TimerTeror_Tick(object sender, EventArgs e) { }
        private void Spawn_Tick(object sender, EventArgs e) { }
        private void Lv2(object sender, EventArgs e) { }
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
                _player.Respawn(HPapa);
            }

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A ) ApeSpeedX = -4;
            if (e.Key == Key.D ) ApeSpeedX = 4;
            if (e.Key == Key.W ) ApeSpeedY = -4;
            if (e.Key == Key.S) ApeSpeedY = 4;
            if (PooTimer > 120 && e.Key == Key.Space)
            {
                pooskott();
                PooTimer = 0;  
            }
            if (sentrytimer > 15 && e.Key == Key.Q)
            {
                Sentry1();
                sentrytimer -= 15;
                Peng.Text = "Cash: " + sentrytimer;
            }
            if (sentrytimer > 35 && e.Key == Key.E)
            {
                Sentry2();
                sentrytimer -= 35 ;
                Peng.Text = "Cash: " + sentrytimer;
            }
            if (e.Key == Key.P) sentrytimer += 100;
            if (e.Key == Key.O) sentrytimer += 1;
            if (e.Key == Key.T) Tkills += 1;
            if (e.Key == Key.B)
            {
                Bkills += 1;
                BombKill.Text = "BombApor: " + Bkills;
            }
            if (e.Key == Key.D1 && sentrytimer>74 && fartcheck)
            {
                Faster_Farting.Visibility = Visibility.Hidden;
                sentrytimer -= 75;
                fartcheck = false;
            }
            if (e.Key == Key.D2 && sentrytimer > 54 && Fatcheck)
            {
                Big_booty.Visibility = Visibility.Hidden;
                sentrytimer -= 55;
                HPapa += 10;
                ApaHPtext.Text = "HP: " + HPapa;
                Fatcheck = false;
            }
            if (e.Key == Key.D3)
            {
                if (sentrytimer > RebuildCost && HPslott <= 80 && RebuildCounter > 0)
                {
                    sentrytimer -= RebuildCost;
                    HPslott += 20;
                    SlottHPtext.Text = "Slott: " + HPslott;
                    RebuildCounter--;
                    Rebuilds_left.Text = "Rebuilds Left: " + RebuildCounter;
                    RebuildCost = RebuildCost * 1.5;
                    RebuildTXT.Text = RebuildCost.ToString();
                    if (RebuildCounter < 1)
                    {
                        Rebuild.Visibility = Visibility.Hidden;
                        RebuildTXT.Visibility = Visibility.Hidden;
                        dollar.Visibility = Visibility.Hidden;
                    }
                }
            }
            if (e.Key == Key.Space && Titlescreen.Visibility == Visibility.Visible)
            {
                Startgame();
            }
        }
        void MoveApe()
        {
            _player.SpeedX = ApeSpeedX;
            _player.SpeedY = ApeSpeedY;
            _player.UpdateMovement(Big_booty.Visibility == Visibility.Hidden);
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
            var poo = _player.CreateProjectile(Pootexture);
            PooBullets.Add(poo);
            GameCanvas.Children.Add(poo);
        }
        private void Movepoo()
        {
            for (int i = 0; i < PooBullets.Count; i++)
            {
                var Pooimg = PooBullets[i];
                Canvas.SetLeft(Pooimg, Canvas.GetLeft(Pooimg) + 6);
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.D) ApeSpeedX = 0;
            if (e.Key == Key.W || e.Key == Key.S) ApeSpeedY = 0;
            if ( e.Key == Key.P ) sentrytimer += 100;
            if (e.Key == Key.O) sentrytimer += 1;
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
                var poo = _player.CreateProjectile(Pootexture);
                Canvas.SetLeft(poo, Canvas.GetLeft(fart));
                Canvas.SetTop(poo, Canvas.GetTop(fart));
                PooBullets.Add(poo);
                GameCanvas.Children.Add(poo);
            }
        }
        private void CreatFartlingShot()
        {
            foreach (var fart in Fartling)
            {
                var poo = _player.CreateProjectile(Pootexture);
                Canvas.SetLeft(poo, Canvas.GetLeft(fart));
                Canvas.SetTop(poo, Canvas.GetTop(fart));
                PooBullets.Add(poo);
                GameCanvas.Children.Add(poo);
            }
        }
        private void Checkcolition()
        {
            Rect aparect = new Rect(Canvas.GetLeft(Apan), Canvas.GetTop(Apan), Apan.Width, Apan.Height);
            Rect terrorrect = new Rect(Canvas.GetLeft(Tapa), Canvas.GetTop(Tapa), Tapa.Width, Tapa.Height);
            Rect bombrect = new Rect(Canvas.GetLeft(Bombapa), Canvas.GetTop(Bombapa), Bombapa.Width, Bombapa.Height);
            Rect elimrect = new Rect(Canvas.GetLeft(Eliminator), Canvas.GetTop(Eliminator), Eliminator.Width, Eliminator.Height);
            Rect telimrect = new Rect(Canvas.GetLeft(TEliminator), Canvas.GetTop(TEliminator), TEliminator.Width, TEliminator.Height);
            Rect Reseter = new Rect(Canvas.GetLeft(Titlescreen), Canvas.GetTop(Titlescreen), Titlescreen.Width, Titlescreen.Height);

            for (int b = 0; b < Bullets.Count; b++)
            {
                var bullet = Bullets[b];
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
                    if (fs1Rect.IntersectsWith(Reseter))
                    {
                        fs1.Life -= 1;
                        fs1.Opacity = fs1.Life;
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
                    if (fs2Rect.IntersectsWith(Reseter))
                    {
                        fs2.Life2 -= 1;
                        fs2.Opacity = fs2.Life2;
                    }
                }
                Fartling.RemoveAll(x => x.Life2 <= 0);
            }
            if (HPslott < 1)
            {
                GameOverscreen();
            }
            Bullets.RemoveAll(x => x.Visibility == Visibility.Hidden);

            for (int s = 0; s < PooBullets.Count; s++)
            {
                var skott = PooBullets[s];
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
            if (i == 1) MoveTerorApaAI();
            if (i == 2) Terorskut();
            if (i == 3)
            {
                MoveTerorApaAI();
                Terorskut();
            }
            if (i >= 7) MoveTerorApaAI();
        }
        private void MoveTerorApaAI()
        {
            _terrorApe.AI_Move(ref HPslott, hp => { TerrorHP = hp; TapaHptxt.Text = "" + TerrorHP; SlottHPtext.Text = "Slott: " + HPslott; });
        }
        private void Dödfix()
        {
            if (TerrorHP < 1)
            {
                _terrorApe.ResetPosition();
                sentrytimer++;
                if (Tkills >= 100) ScalingT(); else TerrorHP = 3;
                TapaHptxt.Text = "" + TerrorHP;
                Tkills++;
                TerrorKill.Text= "TerrorApor: " + Tkills;
            }
            Canvas.SetTop(TapaHptxt, Canvas.GetTop(Tapa) - 20);
            Canvas.SetLeft(TapaHptxt, Canvas.GetLeft(Tapa) + 25);
        }
        private void Terorskut()
        {
            var img = _terrorApe.Shoot(bulletTexture);
            Bullets.Add(img);
            GameCanvas.Children.Add(img);
        }
        private void MoveBullets()
        { 
            for (int i = 0; i < Bullets.Count; i++)
            {
                var img = Bullets[i];
                Canvas.SetLeft(img, Canvas.GetLeft(img)-5);
            }
        }
        private void MoveBombapa()
        {
            _bombApe.Move(ref HPslott, hp => { BombApaHP = hp; BombApaHPtxt.Text = "" + BombApaHP; SlottHPtext.Text = "Slott: " + HPslott; });
            Canvas.SetTop(BombEX, Canvas.GetTop(Bombapa)-35);
            Canvas.SetLeft(BombEX, Canvas.GetLeft(Bombapa)-30);
            Canvas.SetTop(BombApaHPtxt, Canvas.GetTop(Bombapa) - 20);
            Canvas.SetLeft(BombApaHPtxt, Canvas.GetLeft(Bombapa) + 40);
            if (BombApaHP < 1)
            {
                _bombApe.ResetPosition();
                sentrytimer+=3;
                if (Bkills >= 100) ScalingB(); else BombApaHP = 8;
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

        private void GameOverscreen()
        {
            GameOver.Visibility = Visibility.Visible;
            Canvas.SetLeft(Tapa, 750);
            Canvas.SetTop(Tapa, 700);
            Canvas.SetTop(Titlescreen, -45);
            Canvas.SetLeft(Titlescreen, 0);
            if (gameovertimer > 5)
            {
                GameOver.Visibility = Visibility.Hidden;
                Titlescreen.Visibility = Visibility.Visible;
                gameovertimer = 0;
            }

        }
        private void Startgame()
        {
            GameOver.Visibility = Visibility.Hidden;
            Titlescreen.Visibility = Visibility.Hidden;
            Big_booty.Visibility = Visibility.Visible;
            Faster_Farting.Visibility = Visibility.Visible;
            Rebuild.Visibility = Visibility.Visible;
            RebuildTXT.Visibility = Visibility.Visible;
            dollar.Visibility = Visibility.Visible;
            ApeSpeedX = 0;
            ApeSpeedY = 0;
            HPapa = 10;
            _player.HP = HPapa;
            HPslott = 100;
            PooTimer = 0;
            BombApaHP = 8;
            _bombApe.HP = BombApaHP;
            TerrorHP = 3;
            _terrorApe.HP = TerrorHP;
            Respawntime = 5;
            sentrytimer = 0;
            Tkills = 0;
            Bkills = 0;
            RebuildCounter = 5;
            RebuildCost = 20;
            fartcheck = true;
            Fatcheck = true;
            gameovertimer = 0;
            ApaHPtext.Text = "HP: " + HPapa;
            SlottHPtext.Text = "Slott: " + HPslott;
            BombApaHPtxt.Text = "" + BombApaHP;
            TapaHptxt.Text = "" + TerrorHP;
            RespawnTXT.Text = "" + Respawntime;
            Peng.Text = "Cash: " + sentrytimer;
            TerrorKill.Text = "TerrorApor: " + Tkills;
            BombKill.Text = "BombApor: " + Bkills;
            Rebuilds_left.Text = "Rebuilds Left: " + RebuildCounter;
            RebuildTXT.Text = RebuildCost.ToString();
            Canvas.SetLeft(Bombapa, 750);
            Canvas.SetTop(Bombapa, 100);
            Canvas.SetLeft(Tapa, 750);
            Canvas.SetTop(Tapa, 400);
            Canvas.SetTop(Apan, 200);
            Canvas.SetLeft(Apan, 165);
            Canvas.SetTop(Titlescreen, 2000);
            Canvas.SetLeft(Titlescreen, 2000);
        }
    }
}