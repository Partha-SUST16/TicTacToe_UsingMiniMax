using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_MinMax
{
    /// <summary>
    /// This class represents a tic-tac-toe board
    /// It is Cloneable so that we can copy board configurations when searching for a move
    /// </summary>
    public class Board : ICloneable
    {
        public enum Pieces { X, O, Empty };

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
