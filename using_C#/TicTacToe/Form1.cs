using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TicTacToe
{
    


    /// <summary>
    /// This class is responsible for 'driving' the game.
    /// It handles drawing the game board, handling board click events,
    /// and polling the players for their moves.
    /// </summary>
    public partial class TicTacToeForm : Form
    {
        
        /// <summary>
        /// Used to notify a client that user has double clicked on a game square
        /// </summary>
        public event SquareDoubleClickHandler SquareDoubleClicked;


        protected TicTacToeGame game;
     
        protected List<Player> players;

        

        protected Thread mainThread = null;

        // thread used to request a player to move
        // runs in a seperate thread so that it does not block main thread
        // and effect UI
        protected Thread playerThread;


        // used as a flag to indicate that a p
        protected TicTacToeMove lastMove = null;

        public TicTacToeForm()
        {
            InitializeComponent();

            ticTacToePanel.Paint += new PaintEventHandler(ticTacToePanel_Paint);
            this.ticTacToePanel.MouseDoubleClick += new MouseEventHandler(ticTacToePanel_MouseDoubleClick);

            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        }


        /// <summary>
        /// Add the player as a participant to the game
        /// </summary>
        /// <param name="p">The player to add</param>
        public void AddPlayer(Player p)
        {
            if (players == null)
                players = new List<Player>();

            if (this.players.Count > 2)
                throw new Exception("Must have only 2 players");

            if (players.Count == 1)
                if (players[0].PlayerPiece == p.PlayerPiece)
                    throw new Exception("Players Must have different board pieces");

            players.Add(p);
        }

        /// <summary>
        /// Remove all the players from the game.
        /// </summary>
        public void RemoveAllPlayers()
        {
            players.Clear();
        }


        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // clean up threads

            if (mainThread != null)
                mainThread.Abort();

            if (playerThread != null)
                playerThread.Abort();

        }

   


        private void TicTacToeForm_Load(object sender, EventArgs e)
        {
            LaunchGame();
        }



        /// <summary>
        /// Gets the game started.  Players must have already been added.
        /// </summary>
        public void LaunchGame()
        {
            if (players.Count != 2)
                throw new Exception("There must be two players for this game!");

            game = new TicTacToeGame(players[0].PlayerPiece, players[1].PlayerPiece);

            this.Show();

  
            ticTacToePanel.Invalidate();

            mainThread = new Thread(new ThreadStart(ProcessPlayerMoves));
            mainThread.Start();



        }


      

        //
        protected TicTacToeMove GetMoveForPlayer(Player p)
        {
            lastMove = null;

            playerThread = new Thread(p.Move);
            playerThread.Start(game.GameBoard);


            // register a listener
            p.PlayerMoved += new PlayerMovedHandler(player_PlayerMoved);

            // lastMove is assigned in the player_PlayerMoved handler
            while (lastMove == null)
                ;

            // if we get here the player moved

            // unregister the listenter
            p.PlayerMoved -= player_PlayerMoved;


            // kill the thread
            playerThread.Abort();

            return p.CurrentMove;

        }

        // Gets each players move applies it to the game board
        private void ProcessPlayerMoves()
        {
            while (!game.IsGameOver())
            {

                for (int i = 0; i < players.Count; i++)
                {
                    Player p = players[i];

                    TicTacToeMove playerMove = GetMoveForPlayer(p);

                    game.MakeMove(new TicTacToeMove(playerMove.Position, p.PlayerPiece));

                    // update the graphics
                    this.ticTacToePanel.Invalidate();


                    if (IsGameOver())
                    {
                        ShowEndOfGameMessage(players[i]);
                        FinishGame();
                    }
               
                }

              
            }

            
        }


        private bool IsGameOver()
        {
            return game.GameBoard.HasWinner() || game.GameBoard.IsDraw();
        }

        private void ShowEndOfGameMessage(Player lastPlayerToAct)
        {
            string msg = "Game Over! ";

            if (game.GameBoard.HasWinner())
                msg += lastPlayerToAct.Name + " wins!";
            else
                msg += "It's a draw.";

             MessageBox.Show(msg);
        }

        private void FinishGame()
        {
            // kill the main game driver thread
            mainThread.Abort();

            // now unregister the mouseclick listener
            this.ticTacToePanel.MouseDoubleClick -= this.ticTacToePanel_MouseDoubleClick;
        }



        private void player_PlayerMoved(object sender, PlayerMovedArgs args)
        {
            lastMove = args.Move;
        }



        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                mainThread.Abort();


            LaunchGame();

        }





        void ticTacToePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            int position = TranslateMouseClickToPosition(e.X, e.Y);

            // we will only process the event and notify any clients of the SquareDoubleClicked
            // event if the the square is empty
            if (this.game.GameBoard.IsValidSquare(position))
            {

                // Determine the position of th square clicked on the panel and then inoke the event
                TicTacToeBoardClickedEventArgs t = new TicTacToeBoardClickedEventArgs(position);

                if (SquareDoubleClicked != null)
                    SquareDoubleClicked(this, t);
            }

        }


        // Determines the board location given a mouse click on the 
        // game board
        private int TranslateMouseClickToPosition(int x, int y)
        {
            int pixelsPerColumn = ticTacToePanel.Width / game.Columns;
            int pixelsPerRow = ticTacToePanel.Height / game.Rows;


            int row = y / pixelsPerRow;
            int col = x / pixelsPerColumn;

            int position = row * game.Columns + col;

            return position;

        }



        // redraw the grid and pieces on the board
        private void ticTacToePanel_Paint(object sender, PaintEventArgs e)
        {

            DrawGrid(e.Graphics);
            DrawPieces(e.Graphics);

        }


        // draws each piece on the board onto the panel
        private void DrawPieces(Graphics g)
        {

            for (int i = 0; i < game.Rows * game.Columns; i++)
            {

                Board.Pieces piece = game.GameBoard.GetPieceAtPosition(i);
                if (piece == Board.Pieces.X || piece == Board.Pieces.O)
                {
                    DrawPiece(piece, g, i);
                }
            }
        }







        private void DrawGrid(Graphics g)
        {
            // equally space the grid rows

            float pixelsPerRow = ticTacToePanel.Width / game.Rows;
            float pixelsPerColumn = ticTacToePanel.Height / game.Columns;

            // now draw the grid lines

            Pen p = new Pen(Brushes.Black);
            p.Width = 6;

            for (int i = 0; i < game.Rows - 1; i++)
            {
                g.DrawLine(p, pixelsPerColumn * (i + 1), 0, pixelsPerColumn * (i + 1), ticTacToePanel.Height);
            }


            // now draw the horizontal lines

            for (int j = 0; j < game.Columns - 1; j++)
            {
                g.DrawLine(p, 0, pixelsPerRow * (j + 1), ticTacToePanel.Width, pixelsPerRow * (j + 1));

            }




        }


        /// <summary>
        /// This method returns a PointF struct representing the cooridinates
        /// of where a piece should be drawn on the board given the position (0-8) on the board.
        /// The coords are essentially the center of a position on the board with offsets that allow the
        /// piece to be drawn in a position so that it is centered
        /// </summary>
        /// <param name="position">The position on the board for which we want the drawing coords</param>
        /// <returns>a PointF representing the x and y coords on the panel the piece should be drawn</returns>
        private PointF GetPieceDrawingCoordsFromPosition(int position)
        {
            // xOffset and yOffset are used for small corrections
            // in placing the X and O's on the screen
            // in order to get them centered
            int xOffset = -5;
            int yOffset = -5;

            int row = position / game.Columns;
            int col = position % game.Columns;

            int pixelsPerRow = ticTacToePanel.Height / game.Rows;
            int pixelsPerColumn = ticTacToePanel.Width / game.Columns;

            float midPixelsPerRow = pixelsPerRow / 3;
            float midPixelsPerColumn = pixelsPerColumn / 3;

            float xCoord = pixelsPerColumn * col + midPixelsPerColumn + xOffset;
            float yCoord = pixelsPerRow * row + midPixelsPerRow + yOffset;


            return new PointF(xCoord, yCoord);
        }


        
        /// <summary>
        /// Draws the specified piece on the board in the designated position.
        /// Position 0 is the upper left square on the board and position 8 is the lower right
        /// corner square on the board
        /// </summary>
        /// <param name="p">The piece we wish to draw</param>
        /// <param name="g">The graphics object we are drawing on</param>
        /// <param name="position">The position on the board to draw at</param>
        private void DrawPiece(Board.Pieces p, Graphics g, int position)
        {
            float pixelsPerRow = ticTacToePanel.Width / game.Rows;
            float pixelsPerColumn = ticTacToePanel.Height / game.Columns;

            PointF point = GetPieceDrawingCoordsFromPosition(position);
            Font f = new Font("Arial", 40);

            if (p == Board.Pieces.X)
                g.DrawString("X", f, Brushes.Blue, point);

            else if (p == Board.Pieces.O)
                g.DrawString("O", f, Brushes.Red, point);

        }

        private void TicTacToeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        
            if (mainThread != null)
            {
                
                mainThread.Abort();
            }

            if (playerThread != null)
                playerThread.Abort();
        }


    }

    // A delegate for the use with the SquareDoubleClickHandler
    // 
    public delegate void SquareDoubleClickHandler(object sender, TicTacToeBoardClickedEventArgs args);

    public class TicTacToeBoardClickedEventArgs : System.EventArgs
    {
        protected int boardPosition;

        public TicTacToeBoardClickedEventArgs(int position) :base()
        {
            boardPosition = position;
        }

        public int BoardPosition
        {
            get { return boardPosition; }
        }
    }


 
}