using System;
using System.Windows;
using System.Windows.Controls;

namespace Orbit.Game.Enemies
{
    public class BombApe
    {
        private readonly Image _image;
        private readonly Image _explosion;
        private readonly Random _rnd;
        public double HP { get; set; }
        public double BaseHP { get; }

        public BombApe(Image image, Image explosion, Random rnd, double baseHp = 8)
        {
            _image = image;
            _explosion = explosion;
            _rnd = rnd;
            BaseHP = baseHp;
            HP = baseHp;
        }

        public double X => Canvas.GetLeft(_image);
        public double Y => Canvas.GetTop(_image);

        public Rect Bounds => new Rect(X, Y, _image.Width, _image.Height);

        public void Move(ref int slottHp, Action<double> onHpReset)
        {
            Canvas.SetLeft(_image, X - 1);
            int By = _rnd.Next(10, 340);
            if (X < 65)
            {
                Canvas.SetLeft(_image, 750);
                Canvas.SetTop(_image, By);
                slottHp -= 15;
                HP = BaseHP;
                onHpReset(HP);
            }
            _explosion.Visibility = X < 75 ? Visibility.Visible : Visibility.Hidden;
            _image.Visibility = X < 75 ? Visibility.Hidden : Visibility.Visible;
        }

        public void ResetPosition()
        {
            Canvas.SetLeft(_image, 750);
            Canvas.SetTop(_image, _rnd.Next(10, 340));
        }
    }
}
