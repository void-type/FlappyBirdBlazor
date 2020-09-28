using System.Collections.Generic;

namespace FlappyBirdBlazor.Web.Models
{
    public class UserInputManager
    {
        private readonly HashSet<UserInputCommand> commands = new HashSet<UserInputCommand>();

        public UserInputManager()
        {
            ResetState();
        }

        public void ResetState()
        {
            commands.Clear();
        }

        public UserInputState GetState()
        {
            return new UserInputState(commands);
        }

        public void AddCommand(UserInputCommand command)
        {
            commands.Add(command);
        }
    }
}
