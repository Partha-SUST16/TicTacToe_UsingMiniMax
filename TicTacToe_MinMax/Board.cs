using System;
using System.Collections.Generic;
using System.Drawing;
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

        public const int COLUMNS = 3;
        public const int ROWS = 3;
        protected const int WINNING_LENGTH = 3;

        public bool haveWinner;  // do we have a winner?

        protected Pieces winningPiece;
        protected int[,] board; // a two-dimensional array representing the game board

        /// <summary>
        /// Constructs a new board from a previous game state.
        /// The game state conventions are as follows:
        /// the first index indicates the board row, the second index represents 
        /// the column.  For example, gameState[1,2] represents the 2nd row and third column
        /// of the board.
        /// A value of 0 indicates an open square on the board
        /// A value of 1 indicates an 'X' on the board
        /// A value of 2 indicates an 'O' on the board
        /// 
        /// </summary>
        /// <param name="gameState">a two-dimensional array representing the game state</param>
        public Board(int[,] gameState) : this()
        {
            for (int i = 0; i <= gameState.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= gameState.GetUpperBound(1); j++)
                {
                    this.board[i, j] = gameState[i, j];
                }
            }
        }

        /// <summary>
        /// Constucts an empty board
        /// </summary>
        public Board()
        {
            board = new int[ROWS,COLUMNS];
        }
        /// <summary>
        /// Returns the winner's piece (an 'X' or an 'O')
        /// </summary>
        public Pieces WinningPiece
        {
            get { return winningPiece; }
            set { winningPiece = value; }
        }

        /// <summary>
        /// Returns the number of rows in the game board
        /// </summary>
        public int Rows => ROWS;

        /// <summary>
        /// Returns the number of columns in the game board
        /// </summary>
        public int Columns => COLUMNS;

        // maps a board position number to a point containing 
        // the row in the x value and the column in the y value
        protected Point GetPoint(int position)
        {
            Point p = new Point();

            p.X = position / COLUMNS;
            p.Y = position % ROWS;

            return p;
        }
        // is the position available
        private bool IsPositionOpen(int row, int col)
        {
            return board[row, col] == 0;
        }
        /// <summary>
        /// Returns true if the position is on the board and currently not occupied by 
        /// an 'X' or an 'O'.  Position 0 is the upper-left most square and increases row-by-row
        /// so that first square in the second row is position 3 and position and position 8
        /// is the 3rd square in the 3rd row
        /// 
        /// 0 1 2 
        /// 3 4 5
        /// 6 7 8
        /// 
        /// </summary>
        /// <param name="position">The position to test</param>
        /// <returns></returns>
        public bool IsValidSquare(int position)
        {
            Point p = GetPoint(position);

            if (p.X >= 0 && p.X < ROWS && p.Y >= 0 && p.Y < COLUMNS && IsPositionOpen(p.X, p.Y))
            {
                return true;
            }
            return false;
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
