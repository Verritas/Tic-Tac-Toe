using System;
using System.Linq;

namespace Tic_Tac_Toe
{
    public enum State { Undecided, X, O };

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var board = new Board();
            var winChecker = new WinChecker();
            var renderer = new Renderer();
            while(!WinChecker.IsDraw(board) && WinChecker.Check(board) == State.Undecided)
            {
                Renderer.Render(board);
                var nextMove = Player.GetPosition();

                if (!board.SetState(nextMove, board.NextTurn))
                {
                    Console.WriteLine("Please enter a correct turn");
                }
            }

            Renderer.Render(board);
            renderer.RenderResults(WinChecker.Check(board));

            Console.ReadKey();
        }
    }

    public class Position
    {
        public int Row { get; }
        public int Column { get; }
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public class Board
    {
        private readonly State[,] _state;
        public State NextTurn { get; private set; }

        public Board()
        {
            _state = new State[3, 3];
            NextTurn = State.X;
        }

        public State GetState(Position position)
        {
            return _state[position.Row, position.Column];
        }

        public bool SetState(Position position, State newState)
        {
            if (newState != NextTurn)
            {
                return false;
            }
            if (_state[position.Row, position.Column] != State.Undecided)
            {
                return false;
            }
            _state[position.Row, position.Column] = newState;
            SwitchNextTurn();
            return true;
        }
         private void SwitchNextTurn()
         {
             NextTurn = NextTurn == State.X ? State.O : State.X;
         }
    }

    public class WinChecker
    {
        public static State Check(Board board)
        {
            if (CheckForWin(board, State.X)) return State.X;
            return CheckForWin(board, State.O) ? State.O : State.Undecided;
        }

        private static bool CheckForWin(Board board, State player)
        {
            for (var row = 0; row < 3; row++)
                if (AreAll(board, new[] {
                    new Position(row, 0),
                    new Position(row, 1),
                    new Position(row, 2) }, player))
                    return true;
            for (var column = 0; column < 3; column++)
                if (AreAll(board, new[] {
                    new Position(0, column),
                    new Position(1, column),
                    new Position(2, column) }, player))
                    return true;
            if (AreAll(board, new[] {
                    new Position(0, 0),
                    new Position(1, 1),
                    new Position(2, 2) }, player))
                return true;
            
            return AreAll(board, new[] {
                new Position(2, 0),
                new Position(1, 1),
                new Position(0, 2) }, player);
        }

        private static bool AreAll(Board board, Position[] positions, State state)
        {
            return positions.All(position => board.GetState(position) == state);
        }

        public static bool IsDraw(Board board)
        {
            for (var row = 0; row<3;row++)
            {
                for (var column = 0; column < 3; column++)
                {
                    if (board.GetState(new Position(row, column)) == State.Undecided) return false;
                }
            }

            return true;
        }
    }

    public class Renderer
    {
        public static void Render(Board board)
        {
            var symbols = new char[3, 3];

            for (var row = 0; row < 3; row++)
            {
                for (var column = 0; column < 3; column++)
                {
                    symbols[row, column] = SymbolFor(board.GetState(new Position(row, column)));
                }
            }

            Console.WriteLine($" {symbols[0,0]} | {symbols[0, 1]} | {symbols[0, 2]}");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {symbols[1, 0]} | {symbols[1, 1]} | {symbols[1, 2]}");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {symbols[2, 0]} | {symbols[2, 1]} | {symbols[2, 2]}");
        }

        private static char SymbolFor(State state)
        {
            return state switch
            {
                State.O => 'O',
                State.X => 'X',
                _ => ' '
            };
        }

        public void RenderResults(State winner)
        {
            switch(winner)
            {
                case State.O:
                case State.X:
                    Console.WriteLine(SymbolFor(winner) + " wins!");
                    break;
                case State.Undecided:
                    Console.WriteLine("Draw!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(winner), winner, null);
            }
        }
    }
    public static class Player
    {
        public static Position GetPosition()
        {
            var position = Convert.ToInt32(Console.ReadLine());
            var desiredCoordinate = PositionForNumber(position);
            return desiredCoordinate;
        }

        private static Position PositionForNumber(int position)
        {
            return position switch
            {
                1 => new Position(2, 0),
                2 => new Position(2, 1),
                3 => new Position(2, 2),
                4 => new Position(1, 0),
                5 => new Position(1, 1),
                6 => new Position(1, 2),
                7 => new Position(0, 0),
                8 => new Position(0, 1),
                9 => new Position(0, 2),
                _ => null
            };
        }
    }
}
