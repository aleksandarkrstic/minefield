using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Minefield_NS;

namespace Minefield_UnitTest
{
    [TestClass]
    public class MinefieldTests
    {
        private Minefield game;

        [TestInitialize] 
        public void Init() 
        {
            game = new Minefield(8, 10, 3);
        }

        [TestMethod]
        public void TestChessNotationConversion()
        {
            Assert.AreEqual("A1", game.ToChessNotation(0, 0));
            Assert.AreEqual("E5", game.ToChessNotation(4, 4));
            Assert.AreEqual("H8", game.ToChessNotation(7, 7));
        }

        [TestMethod]
        public void TestInitialLivesMovesAndMines()
        {
            game.RemoveMine(0, 1);
            game.ProcessMove('D');
            Assert.AreEqual(3, game.Lives);
            Assert.AreEqual(1, game.Moves);
            Assert.AreEqual(10, game.NumberOfMines);
        }

        [TestMethod]
        public void TestInvalidMove()
        {
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('Q'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('F'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('E'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('1'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('2'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('!'));
            Assert.AreEqual(MF_STATUS.MF_INVALID_MOVE, game.ProcessMove('.'));
        }                                                           

        [TestMethod]
        public void TestImpossibleMove()
        {
            Assert.AreEqual(MF_STATUS.MF_MOVE_NOT_ALLOWED, game.ProcessMove('W'));
            Assert.AreEqual(MF_STATUS.MF_MOVE_NOT_ALLOWED, game.ProcessMove('A'));
        }

        [TestMethod]
        public void TestMineHit()
        {
            game.PlaceMine(0, 1);
            Assert.AreEqual(MF_STATUS.MF_MINE_HIT, game.ProcessMove('D'));
            Assert.AreEqual(2, game.Lives);
        }

        [TestMethod]
        public void TestOkMove()
        {
            game.RemoveMine(0, 1);
            Assert.AreEqual(MF_STATUS.MF_OK, game.ProcessMove('D'));
        }
    }
}
