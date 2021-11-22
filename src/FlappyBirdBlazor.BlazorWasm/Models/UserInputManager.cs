using System.Collections.Generic;

namespace FlappyBirdBlazor.BlazorWasm.Models
{
    public class UserInputManager
    {
        private readonly HashSet<UserInputCommand> _commands = new();

        public UserInputManager()
        {
            ResetState();
        }

        public void ResetState()
        {
            _commands.Clear();
        }

        public HashSet<UserInputCommand> GetState()
        {
            return new HashSet<UserInputCommand>(_commands);
        }

        public void AddCommand(UserInputCommand command)
        {
            _commands.Add(command);
        }
    }
}
