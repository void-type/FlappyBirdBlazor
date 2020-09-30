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

        public HashSet<UserInputCommand> GetState()
        {
            return new HashSet<UserInputCommand>(commands);
        }

        public void AddCommand(UserInputCommand command)
        {
            commands.Add(command);
        }
    }
}
