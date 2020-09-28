namespace FlappyBirdBlazor.Web.Models
{
    public class PipeModel : ActorModel
    {
        public PipeModel(int left, int bottom, int height, int width) : base(left, bottom, height, width)
        {
        }

        public void Move(int speed)
        {
            Left -= speed;
        }
    }
}
