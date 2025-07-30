using ChessLogic.ChessPiece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8,8];

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Enum.Player.Black);
            this[0, 1] = new Knight(Enum.Player.Black);
            this[0, 2] = new Bishop(Enum.Player.Black);
            this[0, 3] = new Queen(Enum.Player.Black);
            this[0, 4] = new King(Enum.Player.Black);
            this[0, 5] = new Bishop(Enum.Player.Black);
            this[0, 6] = new Knight(Enum.Player.Black);
            this[0, 7] = new Rook(Enum.Player.Black);

            this[7, 0] = new Rook(Enum.Player.White);
            this[7, 1] = new Knight(Enum.Player.White);
            this[7, 2] = new Bishop(Enum.Player.White);
            this[7, 3] = new Queen(Enum.Player.White);
            this[7, 4] = new King(Enum.Player.White);
            this[7, 5] = new Bishop(Enum.Player.White);
            this[7, 6] = new Knight(Enum.Player.White);
            this[7, 7] = new Rook(Enum.Player.White);

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(Enum.Player.Black);
                this[6, i] = new Pawn(Enum.Player.White);
            }
        }

        //return true is the new position is inside the board
        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        //check if there's a piece on the new position
        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }
    }
}
