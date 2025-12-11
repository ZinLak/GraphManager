using GraphManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;


namespace GraphManager.Commands
{
    public class DeleteBlockCommand : IUndoableCommand
    {
        private readonly ObservableCollection<TaskBlock> _blockCollection;
        private readonly ObservableCollection<TaskLink> _linkCollection;
        private readonly TaskBlock _blockToDelete;
        private List<TaskLink> _deleteLinks;

        private readonly double _savedX;
        private readonly double _savedY;

        public DeleteBlockCommand(ObservableCollection<TaskBlock> blocks, ObservableCollection<TaskLink> link, TaskBlock block)
        {
            _blockCollection = blocks;
            _linkCollection = link;
            _blockToDelete = block;

            _savedX = block.X;
            _savedY = block.Y;
        }

        public void Execute()
        {
            _deleteLinks = _linkCollection.Where(l => l.SourceBlockId == _blockToDelete.Id || l.TargetBlockId == _blockToDelete.Id).ToList();

            foreach (var link in _deleteLinks)
            {
                _linkCollection.Remove(link);
            }
            _blockCollection.Remove(_blockToDelete);
        }

        public void UnExecute()
        {
            _blockToDelete.X = _savedX;
            _blockToDelete.Y = _savedY;
            _blockCollection.Add(_blockToDelete);

            foreach (var link in _deleteLinks)
            {
                _linkCollection.Add(link);
            }
        }
    }
}
