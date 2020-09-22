using System;

namespace FlappyBirdBlazor.Web.Models
{
    public class PipeModel
    {
        private readonly Random _random = new Random();
        public int DistanceFromLeft { get; private set; } = 500;
        public int DistanceFromGround { get; private set; }
        public bool IsOffScreen => DistanceFromLeft <= -60;
        public int Gap { get; }

        public PipeModel(int pipeGap)
        {
            DistanceFromGround = _random.Next(0, 60);
            Gap = pipeGap;
        }

        public void Move(int speed)
        {
            DistanceFromLeft -= speed;
        }
    }
}
