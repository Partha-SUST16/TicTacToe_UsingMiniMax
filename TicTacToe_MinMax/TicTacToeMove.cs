namespace TicTacToe_MinMax
{
    /// <summary>
/// Represents a tic-tac-toe move
/// </summary>
    public class TicTacToeMove
    {
        public TicTacToeMove(int position, Board.Pieces piece)
        {
            Position = position;
            Piece = piece;
        }

        /// <summary>
        /// gets or sets the position on the board
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the piece making this move
        /// </summary>
        public Board.Pieces Piece { get; set; }
    }
}