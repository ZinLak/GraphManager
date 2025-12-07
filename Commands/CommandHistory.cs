using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager.Commands
{
    public class CommandHistory
    {
        private Stack<IUndoableCommand> _undoStack = new Stack<IUndoableCommand>();
        private Stack<IUndoableCommand> _redoStack = new Stack<IUndoableCommand>();

        public void AddAndExecute(IUndoableCommand command)
        {
            command.Execute();

            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.UnExecute();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }
    }
}
