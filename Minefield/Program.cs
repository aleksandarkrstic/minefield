using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace Minefield_NS
{
    public enum MF_STATUS
    {
        MF_OK,
        MF_INVALID_MOVE,
        MF_MOVE_NOT_ALLOWED,
        MF_MINE_HIT
    }

    public class Minefield
    {
        private readonly int BoardSize;
        private readonly int MineCount;
        private int PlayerLives;
        private int PlayerMoves;
        private int PlayerRow;
        private int PlayerColumn;
        private bool[,] Mines;

        public Minefield(int Size, int Mines, int Lives)
        {
            BoardSize = Size;
            MineCount = Mines;
            PlayerLives = Lives;
            PlayerRow = 0;
            PlayerColumn = 0;
            InitializeBoard();
            PlaceMines();
        }

        public int Lives => PlayerLives;
        public int Moves => PlayerMoves;
        public int NumberOfMines => MineCount;

        public string ToChessNotation(int row, int col)
        {
            char colChar = (char)('A' + col);
            return $"{colChar}{row + 1}";
        }

        public void Play()
        {
            PrintInstructions();

            while (PlayerLives > 0 && PlayerRow != BoardSize - 1)
            {
                PrintBoard();
                char move = GetPlayerMove();
                ProcessMove(move);
            }

            PrintBoard();
            PrintGameResult();
        }

        private void InitializeBoard()
        {
            Mines = new bool[BoardSize, BoardSize];
        }

        private void PlaceMines()
        {
            Random rand = new Random();
            int placed = 0;

            while (placed < MineCount)
            {
                int row = rand.Next(BoardSize);
                int col = rand.Next(BoardSize);

                if (!Mines[row, col] && (row != 0 || col != 0))
                {
                    Mines[row, col] = true;
                    placed++;
                }
            }
        }

        public void PlaceMine(int row, int col)
        {
            Mines[row, col] = true;
        }

        public void RemoveMine(int row, int col)
        {
            Mines[row, col] = false;
        }

        private void PrintInstructions()
        {
            Console.WriteLine($"Welcome to Minefield!");
            Console.WriteLine($"Navigate from the top row to the bottom row of a {BoardSize}x{BoardSize} grid.");
            Console.WriteLine($"Avoid hidden mines!");
            Console.WriteLine($"You have {PlayerLives} lives.");
            Console.WriteLine("Use R on your keyboard to reveal all mines.\n");
            Console.WriteLine("Use W,S,A,D on your keyboard to navigate up, down, left or right.\n");
            Console.WriteLine("'Y' represents your current position\n");
        }

        private void PrintBoard()
        {
            Console.Write("     ");

            for (char col = 'A'; col < 'A' + BoardSize; ++col)
            {
                Console.Write(col);
                Console.Write(' ');
            }

            Console.WriteLine("\n");

            for (int row = 0; row < BoardSize; ++row)
            {
                Console.Write($"{(row + 1),3}  ");

                for (int col = 0; col < BoardSize; ++col)
                {
                    if (PlayerRow == row && PlayerColumn == col)
                    {
                        Console.Write("Y ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }

                Console.Write("\n");
            }

            Console.WriteLine($"\nCurrent Position: {ToChessNotation(PlayerRow, PlayerColumn)} | Lives: {PlayerLives} | Moves: {PlayerMoves}\n");
        }

        private char GetPlayerMove()
        {
            Console.WriteLine("Enter move (W/S/A/D): ");
            char move = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return char.ToUpper(move);
        }

        public MF_STATUS ProcessMove(char move)
        {
            int newRow = PlayerRow;
            int newCol = PlayerColumn;

            MF_STATUS status = MF_STATUS.MF_OK;

            switch (move)
            {
                case 'W': newRow = Math.Max(0, PlayerRow - 1); break;
                case 'S': newRow = Math.Min(BoardSize - 1, PlayerRow + 1); break;
                case 'A': newCol = Math.Max(0, PlayerColumn - 1); break;
                case 'D': newCol = Math.Min(BoardSize - 1, PlayerColumn + 1); break;
                default:
                    Console.WriteLine("Invalid move! Try again.\n");
                    return MF_STATUS.MF_INVALID_MOVE;
            }

            if (newRow == PlayerRow && newCol == PlayerColumn)
            {
                Console.WriteLine("Move not allowed!\n");
                return MF_STATUS.MF_MOVE_NOT_ALLOWED;
            }

            PlayerRow = newRow;
            PlayerColumn = newCol;
            PlayerMoves++;

            if (Mines[PlayerRow, PlayerColumn])
            {
                PlayerLives--;
                Console.WriteLine($"You hit a mine at {ToChessNotation(PlayerRow, PlayerColumn)}!\n");
                status = MF_STATUS.MF_MINE_HIT;
            }

            return status;
        }

        private void PrintGameResult()
        {
            if (PlayerLives > 0)
            {
                Console.WriteLine($"\nCongratulations! You reached the other side in {PlayerMoves} moves.\n");
            }
            else
            {
                Console.WriteLine("\nGame Over! You ran out of lives.\n");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Minefield game = new Minefield(8, 10, 3);
            game.Play();
        }
    }
}