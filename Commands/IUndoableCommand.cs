using GraphManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager.Commands
{
    public interface IUndoableCommand
    {
        void Execute();
        void UnExecute();
    }
}