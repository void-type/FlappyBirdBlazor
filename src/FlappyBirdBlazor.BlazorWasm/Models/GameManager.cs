using System.Diagnostics;
using System.Security.Cryptography;

namespace FlappyBirdBlazor.BlazorWasm.Models
{
    public class GameManager
    {
        public const int DelayPerFrame = 16;
        public const int ContainerWidth = 700;
        public const int ContainerHeight = 650;
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
        public const int PipeHeight = ContainerHeight;
        public const int PipeWidth = 60;
        public const int BirdInitialLeft = (ContainerWidth / 2) - (BirdWidth / 2);
        public const int PipeBottomNeutral = (SkyHeight / 2) - (PipeGapHeight / 2) - PipeHeight;

        public bool Invincibility { get; private set; }
        public int FramesPerSecond { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool ShowGameState { get; private set; }
        public BirdModel Bird { get; private set; } = new BirdModel(BirdInitialLeft, BirdInitialBottom, BirdHeight, BirdWidth);
        public List<PipesModel> Pipes { get; private set; } = new List<PipesModel>();
        public event EventHandler? OnReadyToRender;

        private readonly UserInputManager _inputManager = new();

        public GameManager()
        {
            ResetGame();
        }

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                ResetGame();
                await MainLoop();
            }
        }

        public void ResetGame()
        {
            _inputManager.ResetState();
            Bird = new BirdModel(BirdInitialLeft, BirdInitialBottom, BirdHeight, BirdWidth);
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
            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(DelayPerFrame));

            int frameCount = 0;

            var lastSecond = Stopwatch.GetTimestamp();

            while (IsRunning && await timer.WaitForNextTickAsync())
            {
                var inputState = _inputManager.GetState();
                _inputManager.ResetState();

                if (inputState.Contains(UserInputCommand.Pause))
                {
                    IsPaused = !IsPaused;
                }

                if (inputState.Contains(UserInputCommand.ShowGameState))
                {
                    ShowGameState = !ShowGameState;
                }

                if (inputState.Contains(UserInputCommand.Invincibility))
                {
                    Invincibility = !Invincibility;
                }

                if (!IsPaused)
                {
                    MoveActors(inputState);
                    CheckForCollisions();
                    ManagePipes();
                }

                OnReadyToRender?.Invoke(this, EventArgs.Empty);

                frameCount++;

                if (Stopwatch.GetElapsedTime(lastSecond).TotalSeconds >= 1)
                {
                    FramesPerSecond = frameCount;
                    lastSecond = Stopwatch.GetTimestamp();
                    frameCount = 0;
                }
            }
        }

        private void MoveActors(HashSet<UserInputCommand> inputState)
        {
            if (inputState.Contains(UserInputCommand.Jump))
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

            var closestPipe = Pipes.Find(p => p.IsTouchingX(Bird));

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
            if (Pipes.Count == 0 || Pipes.Last().Left < (ContainerWidth - PipeSpacing))
            {
                var pipeBottom = PipeBottomNeutral - PipeYAxisVariation + RandomNumberGenerator.GetInt32(0, PipeYAxisVariation);

                Pipes.Add(new PipesModel(ContainerWidth, pipeBottom, PipeHeight, PipeGapHeight, PipeWidth));
            }

            var firstPipe = Pipes[0];

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
