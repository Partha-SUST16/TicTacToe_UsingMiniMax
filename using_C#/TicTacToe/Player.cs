using System;

namespace TicTacToe
{
    // This delegate is used to respond to moves by a player
    public delegate void PlayerMovedHandler(object sender, PlayerMovedArgs args);


    /// <summary>
    /// This class abstracts the idea of a Player and includes some common functionality.
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


        public TicTacToeMove CurrentMove
        {
            get { return currentMove; }
        }


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


    /// <summary>
    /// This class represents a "comuter" player.  
    /// It determines moves using minmax decision rules
    /// </summary>
    public class ComputerPlayer : Player
    {
        public const int DEFAULT_SEARCH_DEPTH = 2;

      
        /// <summary>
        /// Constructs a new computer player.  The DEFAULT_SEARCH_DEPTH is used
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="p">The piece this player is using in the came</param>
        public ComputerPlayer(string name, Board.Pieces p) : this(name,
            p, DEFAULT_SEARCH_DEPTH)
        {
        }


        /// <summary>
        /// Constructs a new computer player
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="p">The piece the player is using</param>
        /// <param name="searchDepth">The depth to search for moves in the game tree.</param>
        public ComputerPlayer(string name, Board.Pieces p, int searchDepth) :base(name, p)
        {
            this.SearchDepth = searchDepth;
        }


        /// <summary>
        /// gets or sets the search depth which is the number of moves
        /// the computer player will look ahead to determine it's move
        /// Greater values yield better computer play
        /// </summary>
        public int SearchDepth { get; set; }






        /// <summary>
        /// Start the computer searching for a move
        /// Clients should listen to the OnPlayerMoved event to be notified 
        /// when the computer has found a move
        /// </summary>
        /// <param name="gameBoard">The current game board</param>
        public override void Move(object gameBoard)
        {
            Board b = (Board)gameBoard;

             //to make things interesting we move randomly if the board we
             //are going first (i.e. the board is empty)
            if (b.OpenPositions.Length == 9)
            {
                this.currentMove = GetRandomMove((Board)gameBoard);
                OnPlayerMoved();
                return;
            }

            Node root = new MaxNode(b, null, null);
            root.MyPiece = this.PlayerPiece;
            root.Evaluator = new EvaluationFunction();
            root.FindBestMove(DEFAULT_SEARCH_DEPTH);

            
            currentMove = root.BestMove;
            
            OnPlayerMoved();
        }


        // gets a random move.  this can be used to make game play interesting
        // particularly in the beginning of the game or if you wish to weaken the computer's
        // play by adding randomness.
        protected TicTacToeMove GetRandomMove(Board b)
        {
            int openPositions = b.OpenPositions.Length;
            Random rGen = new Random();

            int squareToMoveTo = rGen.Next(openPositions);

            TicTacToeMove move = new TicTacToeMove(squareToMoveTo, this.PlayerPiece);
            return move;
        }


    }

    /// <summary>
    /// This class represents a Human Player 
    /// </summary>
    public class HumanPlayer : Player
    {


        protected TicTacToeForm ticTacToeForm;

        protected bool alreadyMoved = false;

        public HumanPlayer(string name, Board.Pieces p, TicTacToeForm tttf)
            : base(name, p)
        {

            this.ticTacToeForm = tttf;

        }


        /// <summary>
        /// Make a move.  Waits for the player to double click a square 
        /// and then triggers the PlayerMoved Event
        /// </summary>
        /// <param name="gameBoard"></param>
        public override void Move(object gameBoard)
        {

            // start listening to clicks
            ticTacToeForm.SquareDoubleClicked += new SquareDoubleClickHandler(SquareDoubleClicked);

            // now wait until the user clicks
            while (!alreadyMoved)
                ;
            

            // reset the flag
            alreadyMoved = false;
            // raise the PlayerMovedEvent
            OnPlayerMoved();


        }


        // when a user double clicks a square on the TicTacToeForm this method receives the 
        // event message
        // the current move is set and the alreadyMoved flag is set to true so that the 
        // which breaks the while loop in the Move method
        void SquareDoubleClicked(object sender, TicTacToeBoardClickedEventArgs args)
        {
            // unregister the double clicked event
            ticTacToeForm.SquareDoubleClicked -= SquareDoubleClicked;

            currentMove = new TicTacToeMove(args.BoardPosition, this.PlayerPiece);
            alreadyMoved = true;

        }

    }


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
        public PlayerMovedArgs(TicTacToeMove m, Player player)
            : base()
        {
            this.player = player;
            move = m;
            
        }

        public TicTacToeMove Move
        {
            get { return move; }
        }

        public Player Player
        {
            get { return player; }
        }
    }



}