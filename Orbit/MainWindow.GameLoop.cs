using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Orbit
{
    public partial class MainWindow
    {
        private TimeSpan _lastFrameTime;
        private int _terrorAiTicker;
        private int _spawnTicker;
        private int _fartlingTicker;
        private int _cashTicker;
        private readonly int _terrorAiIntervalMs = 100;      // old TerorTimer
        private readonly int _spawnIntervalMs = 1000;         // old SpawnTime
        private readonly int _fartlingIntervalMs = 500;       // old Lv2Timer
        private readonly int _cashIntervalMs = 2000;          // old TickTime

        private void InitializeGameLoop()
        {
            CompositionTarget.Rendering += GameLoop_Rendering;
            _lastFrameTime = TimeSpan.Zero;
        }

        private void GameLoop_Rendering(object? sender, EventArgs e)
        {
            if (e is RenderingEventArgs rArgs)
            {
                if (_lastFrameTime == TimeSpan.Zero) _lastFrameTime = rArgs.RenderingTime;
                var delta = rArgs.RenderingTime - _lastFrameTime;
                _lastFrameTime = rArgs.RenderingTime;

                int elapsedMs = (int)delta.TotalMilliseconds;
                if (elapsedMs <= 0) return;

                // accumulate pseudo timers
                _terrorAiTicker += elapsedMs;
                _spawnTicker += elapsedMs;
                _fartlingTicker += elapsedMs;
                _cashTicker += elapsedMs;

                // High frequency (each frame) updates
                FrameUpdate();

                if (_terrorAiTicker >= _terrorAiIntervalMs)
                {
                    _terrorAiTicker = 0;
                    TerorApaAI();
                }
                if (_spawnTicker >= _spawnIntervalMs)
                {
                    _spawnTicker = 0;
                    if (HPapa < 1)
                    {
                        Respawntime--;
                        RespawnTXT.Text = "" + Respawntime;
                    }
                    CreatFartShot();
                    if (GameOver.Visibility == Visibility.Visible)
                    {
                        gameovertimer++;
                    }
                }
                if (_fartlingTicker >= _fartlingIntervalMs)
                {
                    _fartlingTicker = 0;
                    CreatFartlingShot();
                }
                if (_cashTicker >= _cashIntervalMs)
                {
                    _cashTicker = 0;
                    sentrytimer++;
                    Peng.Text = "Cash: " + sentrytimer;
                }
            }
        }

        private void FrameUpdate()
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
        }
    }
}
