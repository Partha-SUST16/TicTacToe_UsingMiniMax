using System;

namespace TicTacToe_MinMax
{
    /// <summary>
    /// An Exception representing an invalid move
    /// </summary>
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException() : base()
        {
        }

        public InvalidMoveException(string msg) : base(msg)
        {
        }
    }
}