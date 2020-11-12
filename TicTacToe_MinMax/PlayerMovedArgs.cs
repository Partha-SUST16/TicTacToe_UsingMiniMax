using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_MinMax
{
    /// <summary>
    /// A class for encapuslating a player moved
    /// This is passed along with PlayerMoved events
    /// </summary>
    public class PlayerMovedArgs : System.EventArgs
    {
        protected TicTacToeMove move;
        protected Player player;

        /// <summary>
        /// Constructs a new PlayerMovedArgs object with the specified Move and Player
        /// </summary>
        /// <param name="m">The move to make</param>
        /// <param name="player">The player making the move</param>
        public PlayerMovedArgs(TicTacToeMove m, Player player) : base()
        {
            this.player = player;
            move = m;
        }
        public TicTacToeMove Move => move;

        public Player Player => player;
    }
}
