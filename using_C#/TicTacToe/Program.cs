using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TicTacToe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TicTacToeForm f = new TicTacToeForm();

            // Create the game players
            // Players can be either human or computer players
            // It does not matter which piece 'X' or 'O' player 1 or two have
            // but they must be different

            // Create a human player
            Player p1 = new HumanPlayer("PP", Board.Pieces.X, f);


            // Create a computer player
            // You can create varying degrees of difficulty by creating computer
            // players that build bigger game trees
            // uncomment desired player and comment all other player 2s


            // create a computer player with the default game tree search depth
            Player p2 = new ComputerPlayer("nT", Board.Pieces.O);

            // Create a computer player that only looks ahead 1 move
            // i.e only considers their immediate move and not any subsequent moves
            // by their opponent. 
            // this is a very poor player
            // Player p2 = new ComputerPlayer(Board.Pieces.X, f, 1);

            // Creates an advanced computer player that looks ahead 5 moves
            // Player p2 = new ComputerPlayer("Advanced HAL", Board.Pieces.X, 5);

            f.AddPlayer(p1);
            f.AddPlayer(p2);

            Application.Run(f);



        }
    }
}