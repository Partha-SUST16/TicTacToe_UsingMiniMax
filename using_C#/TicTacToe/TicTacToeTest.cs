using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeTestSuite
{
    using NUnit.Framework;
    using TicTacToe;
    [TestFixture]
    public class TicTacToeTest
    {

        TicTacToeGame t;

        
        


        public TicTacToeTest()
        {
            t = new TicTacToeGame();
        }

        [SetUp]
        public void Init()
        {

        }


        [Test]
        public void TestMoves()
        {
            t.TakeSquare(4, TicTacToeGame.Players.Player1);
            
            // make sure it's Player's 2 turn now
            Assert.IsTrue(TicTacToeGame.Players.Player2 == t.CurrentPlayerTurn);
        

            // let O in position 0
            t.TakeSquare(0, TicTacToeGame.Players.Player2);

            // let X take position 3
            t.TakeSquare(3, TicTacToeGame.Players.Player1);

            // let O take position 1
            t.TakeSquare(1, TicTacToeGame.Players.Player2);

            // let X take psoition 5
            t.TakeSquare(5, TicTacToeGame.Players.Player1);

            // we should have a winner now

            Assert.IsTrue(t.GameBoard.HasWinner());


            
            // test diagonally
            t = new TicTacToeGame();

            t.TakeSquare(0, TicTacToeGame.Players.Player1);
            t.TakeSquare(1, TicTacToeGame.Players.Player2);
            t.TakeSquare(4, TicTacToeGame.Players.Player1);
            t.TakeSquare(2, TicTacToeGame.Players.Player2);
            t.TakeSquare(8, TicTacToeGame.Players.Player1);

            // game should be over via diagonal to the right
            Assert.IsTrue(t.GameBoard.HasWinner());
            


            

            
        
        }

        [Test]
       // [ExpectedException(typeof(InvalidMoveException))]
        public void MoveOffTheBoard()
        {
            Assert.That(()=> t.TakeSquare(10, t.CurrentPlayerTurn),Throws.TypeOf<InvalidMoveException>());
            ;
        }

    }
}
