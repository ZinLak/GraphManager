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

        public DeleteBlockCommand(ObservableCollection<TaskBlock> blocks, ObservableCollection<TaskLink> link, TaskBlock block)
        {
            _blockCollection = blocks;
            _linkCollection = link;
            _blockToDelete = block;
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
            _blockCollection.Add(_blockToDelete);

            foreach (var link in _deleteLinks)
            {
                _linkCollection.Add(link);
            }
        }
    }
}
