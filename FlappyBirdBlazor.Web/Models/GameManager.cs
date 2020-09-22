using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdBlazor.Web.Models
{
    public class GameManager
    {
        private readonly int _delayPerFrame = 20;
        private readonly int _gameSpeedMultiplier = 2;
        private readonly int _gravity = 2;
        private readonly int _speed = 2;
        private readonly int _birdFlapStrength = 50;
        private readonly int _maxFlapHeight = 580 - 50;
        private readonly int _pipeGap = 130;

        public BirdModel Bird { get; private set; } = new BirdModel();
        public List<PipeModel> Pipes { get; private set; } = new List<PipeModel>();
        public bool IsRunning { get; private set; } = false;
        public event EventHandler MainLoopCompleted;

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Bird = new BirdModel();
                Pipes = new List<PipeModel>();
                await MainLoop();
            }
        }

        public async Task MainLoop()
        {
            while (IsRunning)
            {
                MoveActors();
                CheckForCollisions();
                ManagePipes();

                // Invoke render
                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(_delayPerFrame);
            }
        }

        public void MoveActors()
        {
            Bird.Fall(_gravity * _gameSpeedMultiplier);

            foreach (var pipe in Pipes)
            {
                pipe.Move(_speed * _gameSpeedMultiplier);
            }
        }

        public void CheckForCollisions()
        {
            if (Bird.IsOnGround)
            {
                GameOver();
            }
        }

        public void ManagePipes()
        {
            if (!Pipes.Any() || Pipes.Last().DistanceFromLeft < 250)
            {
                Pipes.Add(new PipeModel(_pipeGap));
            }

            var firstPipe = Pipes.First();

            if (firstPipe.IsOffScreen)
            {
                Pipes.Remove(firstPipe);
            }
        }

        public void SpacePressed()
        {
            if (IsRunning && Bird.DistanceFromGround <= _maxFlapHeight)
            {
                Bird.Flap(_birdFlapStrength);
            }
        }

        public void GameOver()
        {
            IsRunning = false;
        }
    }
}
