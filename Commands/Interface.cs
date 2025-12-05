// Попытка сделать Undo\Redo

//using GraphManager.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraphManager.Commands
//{
//    public class MoveBlockCommand : IUndoableCommand
//    {
//        private readonly TaskBlock _block;
//        private readonly Point _oldPosition;
//        private readonly Point _newPosition;

//        public MoveBlockCommand(TaskBlock block, Point oldPos, Point newPos)
//        {
//            _block = block;
//            _oldPosition = oldPos;
//            _newPosition = newPos;
//        }

//        public void Execute()
//        {
//            _block.X = _newPosition.X;
//            _block.Y = _newPosition.Y;
//        }

//        public void UnExecute()
//        {
//            // При отмене ставим блок на старую позицию
//            _block.X = _oldPosition.X;
//            _block.Y = _oldPosition.Y;
//        }
//    }
//}
