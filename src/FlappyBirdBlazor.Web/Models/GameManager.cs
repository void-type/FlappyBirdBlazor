using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdBlazor.Web.Models
{
    public class GameManager
    {
        public const int ContainerWidth = 900;
        public const int ContainerHeight = 700;
        public const int GroundHeight = 100;
        public const int SkyHeight = ContainerHeight - GroundHeight;

        public const int DelayPerFrame = 16;

        public const int BirdStartingDistanceFromGround = 200;
        public const int BirdFlapStrength = 50;
        public const int BirdGravity = 3;
        public const int BirdMaxFlapHeight = SkyHeight - BirdFlapStrength;
        public readonly int BirdStartingDistanceFromLeft = (ContainerWidth / 2) - (BirdModel.Width / 2);
        public bool GodMode { get; set; } = true;


        public const int PipeHeightVariation = 160;
        public const int PipeGapHeight = 130;
        public const int PipeSpacing = 250;
        public const int PipeSpeed = 4;

        private readonly Random _random = new Random();
        public event EventHandler OnReadyToRender;

        public bool IsRunning { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;
        public BirdModel Bird { get; private set; }
        public List<PipeModel> Pipes { get; private set; }

        public UserInputState InputState { get; set; }

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

        private void ResetGame()
        {
            Bird = new BirdModel(BirdStartingDistanceFromLeft, BirdStartingDistanceFromGround, SkyHeight - BirdFlapStrength);
            Pipes = new List<PipeModel>();
            InputState = new UserInputState();
            IsGameOver = false;
            IsPaused = false;
        }

        public async Task MainLoop()
        {
            IsRunning = true;

            while (IsRunning)
            {
                var frameInputState = InputState;
                InputState = new UserInputState();

                if (!IsPaused)
                {
                    MoveActors(frameInputState);
                    CheckForCollisions();
                    ManagePipes();
                }

                OnReadyToRender?.Invoke(this, EventArgs.Empty);
                await Task.Delay(DelayPerFrame);
            }
        }

        public void MoveActors(UserInputState oldInput)
        {
            if (oldInput.Jump)
            {
                Bird.Flap(BirdFlapStrength);
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

        public void CheckForCollisions()
        {
            if (Bird.IsOnGround)
            {
                Bird.Fall(Bird.Bottom);
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

        public void GameOver()
        {
            if (!GodMode)
            {
                IsGameOver = true;
                IsRunning = false;
            }
        }

        public void ButtonPressed(UserInput input)
        {
            switch (input)
            {
                case UserInput.Jump:
                    InputState.Jump = true;
                    break;
                case UserInput.Left:
                    InputState.Left = true;
                    break;
                case UserInput.Right:
                    InputState.Right = true;
                    break;
                case UserInput.Down:
                    InputState.Down = true;
                    break;
                case UserInput.Up:
                    InputState.Up = true;
                    break;
                case UserInput.Pause:
                    if (IsRunning)
                    {
                        IsPaused = !IsPaused;
                    }
                    break;
            }
        }
    }
}
