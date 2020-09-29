using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdBlazor.Web.Models
{
    public class GameManager
    {
        public const int DelayPerFrame = 16;
        public bool Invincibility { get; set; } = false;
        public bool ShowGameState { get; set; } = false;

        public const int ContainerWidth = 900;
        public const int ContainerHeight = 700;
        public const int GroundHeight = 100;
        public const int SkyHeight = ContainerHeight - GroundHeight;

        public const int BirdInitialBottom = 200;
        public const int BirdFlapStrength = 50;
        public const int BirdGravity = 3;
        public const int BirdHeight = 45;
        public const int BirdWidth = 60;

        public const int PipeYAxisVariation = 160;
        public const int PipeGapHeight = 130;
        public const int PipeSpacing = 250;
        public const int PipeSpeed = 4;
        public int PipeHeight => ContainerHeight;
        public const int PipeWidth = 60;

        public bool IsRunning { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;
        public BirdModel Bird { get; private set; }
        public List<PipesModel> Pipes { get; private set; }
        public event EventHandler OnReadyToRender;

        private readonly Random _random = new Random();
        private readonly UserInputManager _inputManager = new UserInputManager();

        public GameManager()
        {
            ResetGame();
        }

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                ResetGame();
                await MainLoop();
            }
        }

        public void ResetGame()
        {
            _inputManager.ResetState();
            var birdInitialLeft = (ContainerWidth / 2) - (BirdWidth / 2);
            Bird = new BirdModel(birdInitialLeft, BirdInitialBottom, BirdHeight, BirdWidth);
            Pipes = new List<PipesModel>();
            IsGameOver = false;
            IsPaused = false;
        }

        public void HandleUserInput(UserInputCommand command)
        {
            _inputManager.AddCommand(command);
        }

        private async Task MainLoop()
        {
            IsRunning = true;

            while (IsRunning)
            {
                var inputState = _inputManager.GetState();
                _inputManager.ResetState();

                if (inputState.Pause)
                {
                    IsPaused = !IsPaused;
                }

                if (!IsPaused)
                {
                    MoveActors(inputState);
                    CheckForCollisions();
                    ManagePipes();
                }

                OnReadyToRender?.Invoke(this, EventArgs.Empty);
                await Task.Delay(DelayPerFrame);
            }
        }

        private void MoveActors(UserInputState inputState)
        {
            if (inputState.Jump)
            {
                if (Bird.Bottom <= (SkyHeight - BirdFlapStrength))
                {
                    Bird.Flap(BirdFlapStrength);
                }
            }
            else
            {
                Bird.Fall(BirdGravity);
            }

            foreach (var pipe in Pipes)
            {
                pipe.Move(PipeSpeed);
            }
        }

        private void CheckForCollisions()
        {
            if (Bird.Bottom <= 0)
            {
                Bird.Teleport(0);
                GameOver();
            }

            var closestPipe = Pipes.FirstOrDefault(p => p.IsTouchingX(Bird));

            if (closestPipe != null)
            {
                var topPipe = closestPipe.TopPipe;
                var bottomPipe = closestPipe.BottomPipe;

                if (Bird.IsTouchingY(topPipe) || Bird.IsTouchingY(bottomPipe))
                {
                    GameOver();
                }
            }
        }

        private void ManagePipes()
        {
            if (!Pipes.Any() || Pipes.Last().Left < (ContainerWidth - PipeSpacing))
            {
                var pipeBottomNeutral = (SkyHeight / 2) - (PipeGapHeight / 2) - PipeHeight;
                var pipeBottom = pipeBottomNeutral - PipeYAxisVariation + _random.Next(0, PipeYAxisVariation);

                Pipes.Add(new PipesModel(ContainerWidth, pipeBottom, PipeHeight, PipeGapHeight, PipeWidth));
            }

            var firstPipe = Pipes.First();

            if (firstPipe.Left <= -firstPipe.Width)
            {
                Pipes.Remove(firstPipe);
            }
        }

        private void GameOver()
        {
            if (!Invincibility)
            {
                IsGameOver = true;
                IsRunning = false;
            }
        }
    }
}
