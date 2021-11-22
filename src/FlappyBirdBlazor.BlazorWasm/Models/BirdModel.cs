namespace FlappyBirdBlazor.BlazorWasm.Models
{
    public class BirdModel : ActorModel
    {
        private int _flapTargetDistance = 0;
        private int _flapAnimationCountdown = 0;
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
            if (_flapAnimationCountdown > 0)
            {
                Bottom += _flapTargetDistance / FlapAnimationLength;
                _flapAnimationCountdown--;
            }
            else
            {
                Bottom -= distance;
            }
        }

        public void Flap(int distance)
        {
            if (_flapAnimationCountdown > 0)
            {
                return;
            }

            _flapTargetDistance = distance;
            _flapAnimationCountdown = FlapAnimationLength;
        }
    }
}
