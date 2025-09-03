using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        // All directions the piece can move
        private static readonly Direction[] _directionVector = new Direction[]
        {

            Direction.North,
            Direction.East,
            Direction.West,
            Direction.South,
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast

        };

        public King(Player color)
        {
            Color = color;
        }

        // this method check if the Rook piece has moved, this is a requirement for castle move
        private static bool IsUnmovedRook(Position position, Board board)
        {
            if (board.IsEmpty(position))
            {
                return false;
            }

            Piece piece = board[position];
            return piece.Type == PieceType.Rook && !piece.HasMoved;

        }

        // This method is a requirement for Castle move, check if all positions given are empty
        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(position => board.IsEmpty(position));
        }

        // This method use both 'IsUnmovedRook' and 'AllEmpty' to check is Castle move conditions are met on the king side (right)
        private bool CanCastleKingSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 7);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 5), new(fromPosition.Row, 6) }; // This give all arguments to AllEmpty method

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
        }

        // This method use both 'IsUnmovedRook' and 'AllEmpty' to check is Castle move conditions are met on the queen side (left)
        private bool CanCastleQueenSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPosition = new Position(fromPosition.Row, 0);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 1), new(fromPosition.Row, 2), new(fromPosition.Row, 3) };

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // A method to loop through word directions and for each of them take a single position
        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            foreach (Direction direction in _directionVector)
            {
                // for each of them it take a single step from king's current position 
                Position toPosition = fromPosition + direction;

                // Check if the position is outside the board
                if (!Board.IsInside(toPosition))
                {
                    // Skip to next foreach
                    continue;
                }

                // Check if the position is a empty space or opponent piece
                if (board.IsEmpty(toPosition) || board[toPosition].Color != Color)
                {
                    yield return toPosition;
                }
            }
        }


        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            // Loops through the allowed positions
            foreach (Position toPosition in MovePositions(fromPosition, board))
            {
                // Return a normal move from each of them
                yield return new NormalMove(fromPosition, toPosition);
            }

            // if the move is a Castle Move, return the positions according which side
            if (CanCastleKingSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleKingSide, fromPosition);
            }

            if (CanCastleQueenSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleQueenSide, fromPosition);
            }
        }

        public override bool CanCaptureTheKing(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board) // Generate the move positions
                .Any(toPosition => // Check if the opponent king is in any of these positions
                {
                    Piece piece = board[toPosition];
                    return piece != null && piece.Type == PieceType.King;
                });
        }
    }
}
