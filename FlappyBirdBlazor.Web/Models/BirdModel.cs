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

        public BirdModel(int left, int bottom)
        {
            Left = left;
            Bottom = bottom;
        }

        internal void Fall(int distance)
        {
            Bottom -= distance;
        }

        internal void Flap(int distance)
        {
            Bottom += distance;
        }

        internal bool IsBetweenY(int bottomBound, int topBound)
        {
            var topCheck = Top <= topBound;

            var bottomCheck = Bottom >= bottomBound;

            return topCheck && bottomCheck;
        }
    }
}
