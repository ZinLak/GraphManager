using System.Windows;
using GraphManager.Models;

namespace GraphManager.Commands
{
    public class MoveBlockCommand : IUndoableCommand
    {
        private readonly TaskBlock _block;
        private readonly double _oldX, _oldY;
        private readonly double _newX, _newY;

        public MoveBlockCommand(TaskBlock block, double oldX, double oldY, double newX, double newY)
        {
            _block = block;
            _oldX = oldX;
            _oldY = oldY;
            _newX = newX;
            _newY = newY;
        }

        public void Execute()
        {
            _block.X = _newX;
            _block.Y = _newY;
        }

        public void UnExecute()
        {
            _block.X = _oldX;
            _block.Y = _oldY;
        }
    }
}
