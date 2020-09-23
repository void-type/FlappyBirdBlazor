using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdBlazor.Web.Models
{
    public class GameManager
    {
        private const int ContainerWidth = 500;
        private const int ContainerHeight = 700;
        private const int GroundHeight = 100;
        private const int SkyHeight = ContainerHeight - GroundHeight;

        private const int GameSpeedMultiplier = 2;
        private const int DelayPerFrame = 16;

        private const int BirdStartingDistanceFromGround = 200;
        private const int BirdFlapStrength = 50;
        private const int BirdGravity = 1;
        private const int MaxFlapHeight = SkyHeight - BirdFlapStrength;

        private const int PipeGapHeight = 130;
        private const int PipeSpeed = 1;

        private readonly Random _random = new Random();
        private readonly int _birdStartingDistanceFromLeft = (ContainerWidth / 2) - (BirdModel.Width / 2);

        public bool IsRunning { get; private set; } = false;
        public event EventHandler OnTick;
        public BirdModel Bird { get; private set; }
        public List<PipeModel> Pipes { get; private set; }

        public GameManager()
        {
            ResetActors();
        }

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                ResetActors();
                await MainLoop();
            }
        }

        private void ResetActors()
        {
            Bird = new BirdModel(_birdStartingDistanceFromLeft, BirdStartingDistanceFromGround);
            Pipes = new List<PipeModel>();
        }

        public async Task MainLoop()
        {
            while (IsRunning)
            {
                MoveActors();
                CheckForCollisions();
                ManagePipes();

                // Invoke render
                OnTick?.Invoke(this, EventArgs.Empty);
                await Task.Delay(DelayPerFrame);
            }
        }

        public void MoveActors()
        {
            Bird.Fall(BirdGravity * GameSpeedMultiplier);

            foreach (var pipe in Pipes)
            {
                pipe.Move(PipeSpeed * GameSpeedMultiplier);
            }
        }

        public void CheckForCollisions()
        {
            if (Bird.IsOnGround)
            {
                GameOver();
            }

            var closestPipe = Pipes.FirstOrDefault(p => p.IsInVerticalSpace(Bird.Left, Bird.Right));

            if (closestPipe != null)
            {
                if (!Bird.IsBetweenY(closestPipe.GapBottom, closestPipe.GapTop))
                {
                    GameOver();
                }
            }
        }

        public void ManagePipes()
        {
            if (!Pipes.Any() || Pipes.Last().Left < (ContainerWidth / 2))
            {
                var bottom = _random.Next(0, 60) - GroundHeight;
                Pipes.Add(new PipeModel(ContainerWidth, bottom, PipeGapHeight));
            }

            var firstPipe = Pipes.First();

            if (firstPipe.IsOffScreen)
            {
                Pipes.Remove(firstPipe);
            }
        }

        public void SpacePressed()
        {
            if (IsRunning && Bird.Bottom <= MaxFlapHeight)
            {
                Bird.Flap(BirdFlapStrength);
            }
        }

        public void GameOver()
        {
            IsRunning = false;
        }
    }
}
