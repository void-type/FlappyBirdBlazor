using System;

namespace FlappyBirdBlazor.Web.Models
{
    public class BirdModel
    {
        public int DistanceFromGround { get; private set; } = 100;
        public bool IsOnGround => DistanceFromGround <= 0;

        internal void Fall(int gravity)
        {
            DistanceFromGround -= gravity;
        }

        internal void Flap(int strength)
        {
            DistanceFromGround += strength;
        }
    }
}
