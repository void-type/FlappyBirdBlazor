namespace FlappyBirdBlazor.BlazorWasm.Models
{
    public abstract class ActorModel
    {
        public int Bottom { get; protected set; }
        public int Left { get; protected set; }
        public int Top => Bottom + Height;
        public int Right => Left + Width;
        public int Height { get; protected set; }
        public int Width { get; protected set; }

        protected ActorModel(int left, int bottom, int height, int width)
        {
            SetActorPosition(left, bottom);
            SetActorSize(height, width);
        }

        protected void SetActorSize(int height, int width)
        {
            Height = height;
            Width = width;
        }

        protected void SetActorPosition(int left, int bottom)
        {
            Left = left;
            Bottom = bottom;
        }

        public bool IsTouchingX(ActorModel actor)
        {
            return Right >= actor.Left && Left <= actor.Right;
        }

        public bool IsTouchingY(ActorModel actor)
        {
            return Top >= actor.Bottom && Bottom <= actor.Top;
        }
    }
}
