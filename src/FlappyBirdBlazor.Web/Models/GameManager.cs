using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdBlazor.Web.Models
{
    public class GameManager
    {
        public const int ContainerWidth = 600;
        public const int ContainerHeight = 700;
        public const int GroundHeight = 100;
        public const int SkyHeight = ContainerHeight - GroundHeight;

        public const int GameSpeedMultiplier = 3;
        public const int DelayPerFrame = 16;

        public const int BirdStartingDistanceFromGround = 200;
        public const int BirdFlapStrength = 50;
        public const int BirdGravity = 1;
        public const int MaxFlapHeight = SkyHeight - BirdFlapStrength;

        public const int PipeHeightVariation = 160;
        public const int PipeGapHeight = 130;
        public const int PipeSpacing = 250;
        public const int PipeSpeed = 1;

        private readonly Random _random = new Random();
        public readonly int BirdStartingDistanceFromLeft = (ContainerWidth / 2) - (BirdModel.Width / 2);

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
            Bird = new BirdModel(BirdStartingDistanceFromLeft, BirdStartingDistanceFromGround);
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
            if (!Pipes.Any() || Pipes.Last().Left < (ContainerWidth - PipeSpacing))
            {
                var pipeBottomWhenCentered = (SkyHeight / 2) - (PipeGapHeight / 2) - PipeModel.Height;
                var pipeBottom = pipeBottomWhenCentered - PipeHeightVariation + _random.Next(0, PipeHeightVariation);

                Pipes.Add(new PipeModel(ContainerWidth, pipeBottom, PipeGapHeight));
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
