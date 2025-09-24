using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Orbit.Game
{
    public class Player
    {
        private readonly Image _image;
        private readonly FrameworkElement _extension; // butt extension rectangle
        public int HP { get; set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }

        public Player(Image image, FrameworkElement extension, int startHp = 10)
        {
            _image = image;
            _extension = extension;
            HP = startHp;
        }

        public double X => Canvas.GetLeft(_image);
        public double Y => Canvas.GetTop(_image);

        public void UpdateMovement(bool showExtension)
        {
            double apax = X;
            double apay = Y;

            if (apax >= 750 && apax <= 800)
                SpeedX = -1;
            if (apax <= 75)
                SpeedX = 1;
            if (apay >= 360)
                SpeedY = -1;
            if (apay <= 0)
                SpeedY = 1;

            Canvas.SetLeft(_image, X + SpeedX);
            Canvas.SetTop(_image, Y + SpeedY);

            if (showExtension)
            {
                Canvas.SetTop(_extension, Canvas.GetTop(_image) + 28);
                Canvas.SetLeft(_extension, Canvas.GetLeft(_image) + 6);
            }
        }

        public Image CreateProjectile(BitmapImage texture)
        {
            var poo = new Image { Width = 30, Height = 30, Source = texture };
            Canvas.SetLeft(poo, X);
            Canvas.SetTop(poo, Y);
            return poo;
        }

        public void Respawn(int hp)
        {
            HP = hp;
            Canvas.SetTop(_image, 200);
            Canvas.SetLeft(_image, 165);
        }
    }
}
