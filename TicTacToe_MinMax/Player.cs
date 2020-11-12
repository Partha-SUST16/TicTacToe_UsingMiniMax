using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_MinMax
{
    // This delegate is used to respond to moves by a player
    public delegate void PlayerMovedHandler(object sender, PlayerMovedArgs args);
    /// <summary>
    /// This class abstracts the idea of a Player and includes some commone functionality.
    /// It includes an event for clients to be notified when a move is made
    /// </summary>
    public abstract class Player
    {

        // Listen for a move made by a player
        public event PlayerMovedHandler PlayerMoved;

        protected TicTacToeMove currentMove;
        public Player(string name, Board.Pieces p)
        {
            this.Name = name;
            this.PlayerPiece = p;
        }
        public abstract void Move(object gameBoard);
        public TicTacToeMove CurrentMove => currentMove;

        /// <summary>
        /// This is invoked by subclasses to indicate that the player decided on a move
        /// </summary>
        public virtual void OnPlayerMoved()
        {
            if (PlayerMoved != null)
                PlayerMoved(this, new PlayerMovedArgs(currentMove, this));
        }

        /// <summary>
        /// Get or Set the player's piece
        /// </summary>
        public Board.Pieces PlayerPiece { get; set; }
        /// <summary>
        /// Get or set the player's name
        /// </summary>
        public string Name { get; set; }
    }
}
