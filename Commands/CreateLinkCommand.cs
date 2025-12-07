using GraphManager.Models;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace GraphManager.Commands
{
    public class CreateLinkCommand : IUndoableCommand
    {
        private readonly ObservableCollection<TaskLink> _collection;
        private readonly TaskLink _link;

        public CreateLinkCommand(ObservableCollection<TaskLink> collection, TaskLink link)
        {
            _collection = collection;
            _link = link;
        }

        public void Execute()
        {
            if (!_collection.Contains(_link))
            {
                _collection.Add(_link);
            }

        }
        public void UnExecute()
        {
            _collection.Remove(_link); 
        }
    }
}
