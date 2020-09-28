using System.Collections.Generic;

namespace FlappyBirdBlazor.Web.Models
{
    public struct UserInputState
    {
        public UserInputState(HashSet<UserInputCommand> commands)
        {
            Jump = commands.Contains(UserInputCommand.Jump);
            Left = commands.Contains(UserInputCommand.Left);
            Right = commands.Contains(UserInputCommand.Right);
            Down = commands.Contains(UserInputCommand.Down);
            Up = commands.Contains(UserInputCommand.Up);
            Pause = commands.Contains(UserInputCommand.Pause);
        }

        public bool Jump { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Down { get; set; }
        public bool Up { get; set; }
        public bool Pause { get; set; }
    }
}
