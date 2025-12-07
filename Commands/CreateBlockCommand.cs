using GraphManager.Models;
using System.Collections.ObjectModel;

namespace GraphManager.Commands
{
    public class CreateBlockCommand : IUndoableCommand
    {
        private readonly ObservableCollection<TaskBlock> _collection;
        private readonly TaskBlock _block;

        public CreateBlockCommand(ObservableCollection<TaskBlock> collection, TaskBlock block)
        {
            _collection = collection;
            _block = block;
        }

        public void Execute()
        {
            if (!_collection.Contains(_block))
            {
                _collection.Add(_block);
            }
        }

        public void UnExecute()
        { 
            _collection.Remove(_block);
        }
    }
}
