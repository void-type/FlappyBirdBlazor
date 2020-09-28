namespace FlappyBirdBlazor.Web.Models
{
    public class BirdModel : ActorModel
    {
        private int FlapTargetDistance = 0;
        private int FlapAnimationCountdown = 0;
        private const int FlapAnimationLength = 3;

        public BirdModel(int left, int bottom, int height, int width) : base(left, bottom, height, width)
        {
        }

        public void Teleport(int bottom)
        {
            SetActorPosition(Left, bottom);
        }

        public void Fall(int distance)
        {
            if (FlapAnimationCountdown > 0)
            {
                Bottom += FlapTargetDistance / FlapAnimationLength;
                FlapAnimationCountdown -= 1;
            }
            else
            {
                Bottom -= distance;
            }
        }

        public void Flap(int distance)
        {
            if (FlapAnimationCountdown > 0)
            {
                return;
            }

            FlapTargetDistance = distance;
            FlapAnimationCountdown = FlapAnimationLength;
        }
    }
}
