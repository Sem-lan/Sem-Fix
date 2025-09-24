using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Orbit.Game.Enemies
{
    public class TerrorApe
    {
        private readonly Image _image;
        private readonly Random _rnd;
        public double HP { get; set; }
        public double BaseHP { get; }

        public TerrorApe(Image image, Random rnd, double baseHp = 3)
        {
            _image = image;
            _rnd = rnd;
            BaseHP = baseHp;
            HP = baseHp;
        }

        public double X => Canvas.GetLeft(_image);
        public double Y => Canvas.GetTop(_image);

        public Rect Bounds => new Rect(X, Y, _image.Width, _image.Height);

        public void ResetPosition()
        {
            Canvas.SetLeft(_image, 750);
            Canvas.SetTop(_image, _rnd.Next(10, 340));
        }

        public void AI_Move(ref int slottHp, Action<double> onHpReset)
        {
            int movetop = _rnd.Next(-20, 20);
            int moveleft = _rnd.Next(-20, 10);
            int Ty = _rnd.Next(10, 340);
            if (X + moveleft < 65)
            {
                Canvas.SetLeft(_image, 750);
                Canvas.SetTop(_image, Ty);
                slottHp -= 5;
                HP = BaseHP;
                onHpReset(HP);
            }
            else
            {
                Canvas.SetLeft(_image, X + moveleft);
            }
            Canvas.SetTop(_image, Y + movetop);
            if (Y + movetop < -10) Canvas.SetTop(_image, 10);
            if (Y + movetop > 360) Canvas.SetTop(_image, 340);
        }

        public Image Shoot(BitmapImage bulletTexture)
        {
            var img = new Image { Width = 50, Height = 20, Source = bulletTexture };
            Canvas.SetLeft(img, X);
            Canvas.SetTop(img, Y);
            return img;
        }
    }
}
