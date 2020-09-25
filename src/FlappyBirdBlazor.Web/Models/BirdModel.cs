namespace FlappyBirdBlazor.Web.Models
{
    public class BirdModel
    {
        public static readonly int Height = 45;
        public static readonly int Width = 60;

        public int Bottom { get; private set; }
        public int Left { get; }
        public int Top => Bottom + Height;
        public int Right => Left + Width;

        public bool IsOnGround => Bottom <= 0;
        public int MaxFlapHeight { get; }

        private int UpwardMomentum = 0;
        private int FlapFrameCount = 0;
        private const int FlapAnimationLength = 3;

        public BirdModel(int left, int bottom, int maxFlapHeight)
        {
            Left = left;
            Bottom = bottom;
            MaxFlapHeight = maxFlapHeight;
        }

        internal void Fall(int distance)
        {
            if (FlapFrameCount > 0)
            {
                Bottom += UpwardMomentum / FlapAnimationLength;
                FlapFrameCount -= 1;
            }
            else
            {
                Bottom -= distance;
            }
        }

        internal void Flap(int distance)
        {
            if (FlapFrameCount == 0  && Bottom <= MaxFlapHeight)
            {
                UpwardMomentum = distance;
                FlapFrameCount = FlapAnimationLength;
            }
        }

        internal bool IsBetweenY(int bottomBound, int topBound)
        {
            var topCheck = Top <= topBound;
            var bottomCheck = Bottom >= bottomBound;

            return topCheck && bottomCheck;
        }
    }
}
