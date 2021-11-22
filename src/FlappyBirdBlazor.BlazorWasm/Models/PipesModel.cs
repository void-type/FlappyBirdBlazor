namespace FlappyBirdBlazor.BlazorWasm.Models
{
    public class PipesModel : ActorModel
    {
        public PipeModel TopPipe { get; }
        public PipeModel BottomPipe { get; }

        public PipesModel(int left, int bottom, int pipeHeight, int gapHeight, int width) : base(left, bottom, pipeHeight, width)
        {
            BottomPipe = new PipeModel(left, bottom, pipeHeight, width);
            TopPipe = new PipeModel(left, BottomPipe.Top + gapHeight, pipeHeight, width);
            Height = TopPipe.Top - BottomPipe.Bottom;
        }

        public void Move(int speed)
        {
            Left -= speed;
            TopPipe.Move(speed);
            BottomPipe.Move(speed);
        }
    }
}
