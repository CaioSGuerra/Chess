using ChessLogic.ChessPiece;
using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] _pieces = new Piece[8, 8];

        // When a pawn move 2 square, stores the skip position, the position behind the pawn
        // This is a pre-requisite to En Passant move
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>()
        {
            { Player.White, null },
            { Player.Black, null }
        };

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

        // Return the position of skipped square when a player move 2 square in one move
        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        // Get the player and the skipped positions, then stores in the dictionary 
        public void SetPawnSkipPosition(Player player, Position position)
        {
            pawnSkipPositions[player] = position;
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

        // This method will count how many pieces and types each player have in game
        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position position in PiecePositions())
            {
                Piece piece = this[position];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        // This method checks if there are enough pieces left in the game to allow a win.
        // For example, if there are only two kings, the game should not continue infinitely.
        // In that case, this method will return true to indicate a draw.
        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVsKing(counting) || IsKingBishopVsKing(counting) ||
                IsKingKnightVsKing(counting) || IsKingBishopVsKingBishop(counting);
        }
        // Next methods will handle each case of draw.

        // This method checks the special case of King vs. King.
        private static bool IsKingVsKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        // This method checks the special case of King and Bishop vs King
        private static bool IsKingBishopVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        // This method checks the special case of King and Knight vs King
        private static bool IsKingKnightVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        // This method checks the special case of King and Bishop vs King and Bishop 
        // Unlike the previous methods, this one can't be static because it calls 'FindPiece', which accesses instance data via the indexer (this[position])
        private bool IsKingBishopVsKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position whiteBishopPosition = FindPiece(Player.White, PieceType.Bishop);
            Position blackBishopPosition = FindPiece(Player.Black, PieceType.Bishop);

            return whiteBishopPosition.SquareColor() == blackBishopPosition.SquareColor();
        }


        // This method find the first 'PieceType' of a 'Player' color given
        // This is a help method for 'IsKingBishopVsKingBishop' method to work
        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(position => this[position].Type == type);
        }

        // This method checks if Castle move is available
        // this is needed for Threefold Repetition end of game reason
        // Gets 
        private bool IsUnmovedKingAndRook(Position kingPosition, Position rookPosition)
        {
            // Skip the next step if conditions are met, since one of the respective pieces have already been moved
            if (IsEmpty(kingPosition) || IsEmpty(rookPosition))
            {
                return false;
            }


            Piece king = this[kingPosition];
            Piece rook = this[rookPosition];

            // Returns true if the pieces at the selected positions are a King and a Rook, and neither has moved
            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;
        }

        //
        // The following methods are used to implement the Threefold Repetition rule.
        //

        // This method provides the arguments to 'IsUnmovedKingAndRook' using the initial King and Rook positions on the King side
        public bool CastleRightKingSide(Player player)
        {
            // Manually sets the initial King and Rook positions for each player
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        // Same as above, but using the initial King and Rook positions on the Queen side
        public bool CastleRightQueenSide(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        // Helper method for 'CanCaptureEnPassant'
        // Iterates through all pawn positions provided by 'CanCaptureEnPassant'
        // Creates an 'EnPassant' move and checks if it's legal (doesn't leave the king in check)
        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPosition)
        {
            foreach (Position position in pawnPositions.Where(IsInside))
            {
                Piece piece = this[position];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(position, skipPosition);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }

        // This method checks whether a pawn can perform an En Passant capture, which may affect Threefold Repetition
        // It retrieves the opponent's pawn skip position — the square passed over when moving two spaces forward
        // From that skip position, it calculates the diagonal squares where a capture could occur
        // If the current player's pawn is located in one of those diagonal squares, the method should return true
        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPosition = GetPawnSkipPosition(player.Opponent());

            // IF it's null, it means the opponent player's pawn didn't move 2 squares
            if (skipPosition == null)
            {
                return false;
            }

            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPosition + Direction.SouthWest, skipPosition + Direction.SouthEast },
                Player.Black => new Position[] { skipPosition + Direction.NorthWest, skipPosition + Direction.NorthEast },
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPosition);
        }

        // End of Threefold repetitions methods
    }
}
