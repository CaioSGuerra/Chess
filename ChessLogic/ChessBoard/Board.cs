using ChessLogic.ChessPiece;
using ChessLogic.Enum;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] _pieces = new Piece[8, 8];

        public Piece this[int row, int column]
        {
            get { return _pieces[row, column]; }
            set { _pieces[row, column] = value; }
        }

        public Piece this[Position position]
        {
            get { return this[position.Row, position.Column]; }
            set { this[position.Row, position.Column] = value; }
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
        public static bool IsInside(Position position)
        {
            return position.Row >= 0 && position.Row < 8 && position.Column >= 0 && position.Column < 8;
        }

        //check if there's a piece on the new position
        public bool IsEmpty(Position position)
        {
            return this[position] == null;
        }

        // A help Method to return all non-empty positions
        public IEnumerable<Position> PiecePositions()
        {
            // Loops through all the positions
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    // Return non-empty positions
                    Position position = new Position(row, column);

                    if (!IsEmpty(position))
                    {
                        yield return position;
                    }

                }
            }
        }

        // A help method to get all positions containing a piece of a certain color
        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            // Call 'PiecePosition' method
            return PiecePositions().Where(position => this[position].Color == player); // Use where to pick only the positions of the player color
        }

        // Check if a king is in check
        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()) // Get the positions of the opponent pieces
            .Any(position => // Then check if any of then can capture the players king
            {
                Piece piece = this[position]; // Get the opponent piece
                return piece.CanCaptureTheKing(position, this); // Call 'CanCaptureKing' Method
            });
        }

        // To filter any moves that leaves the king in check, this method will help by Copying the board
        public Board Copy()
        {
            Board copy = new Board(); // Create a new empty board

            foreach (Position position in PiecePositions()) // Loop through all positions containing a piece
            {
                copy[position] = this[position].Copy();// Then copy their positions new board
            }

            return copy; // Then return the copy
        }

    }
}
