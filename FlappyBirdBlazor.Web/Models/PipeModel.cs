namespace FlappyBirdBlazor.Web.Models
{
    public class PipeModel
    {
        public static readonly int Width = 60;
        public static readonly int Height = 300;

        public int Bottom { get; }
        public int Left { get; private set; }
        public int Top => Bottom + Height;
        public int Right => Left + Width;

        public bool IsOffScreen => Left <= -Width;

        public int GapHeight { get; }
        public int GapBottom => Bottom + Height;
        public int GapTop => GapBottom + GapHeight;

        public PipeModel(int left, int bottom, int gapHeight)
        {
            Left = left;
            Bottom = bottom;
            GapHeight = gapHeight;
        }

        public void Move(int speed)
        {
            Left -= speed;
        }

        public bool IsInVerticalSpace(int leftBound, int rightBound)
        {
            return Right >= leftBound && Left <= rightBound;
        }
    }
}
