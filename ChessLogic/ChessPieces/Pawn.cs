using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private readonly Direction _forward;

        public Pawn(Player color)
        {
            Color = color;
            // The chess logic to make both pawn to move only in one direction
            if (color == Player.White)
            {
                _forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                _forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // Method to make pawn not capture any piece in a forward direction
        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }

        // Method for enabling diagonal capture by pawns
        private bool CanCaptureAt(Position position, Board board)
        {
            if (!Board.IsInside(position) || board.IsEmpty(position))
            {
                return false;
            }
            return board[position].Color != Color;
        }

        // Method of all forward and non-capture moves
        private IEnumerable<Move> ForwardMoves(Position fromPosition, Board board)
        {
            // Create a position immediately in front of pawn
            Position oneMovePosition = fromPosition + _forward;

            //Check if the pawn can move there
            if (CanMoveTo(oneMovePosition, board))
            {
                yield return new NormalMove(fromPosition, oneMovePosition);

                Position twoMovesPosition = oneMovePosition + _forward;

                // a method to check if it's the first pawn move, then can move 2 squares
                if (!HasMoved && CanMoveTo(twoMovesPosition, board))
                {
                    yield return new NormalMove(fromPosition, twoMovesPosition);
                }
            }

        }

        // A Method to capture in diagonal
        private IEnumerable<Move> DiagonalMoves(Position fromPosition, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                // create the diagonal position in a single direction
                Position toPosition = fromPosition + _forward + direction;

                // check if can capture a diagonal piece then move to this new position
                if (CanCaptureAt(toPosition, board))
                {
                    yield return new NormalMove(fromPosition, toPosition);
                }
            }
        }

        // Using Piece abstract class
        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return ForwardMoves(fromPosition, board).Concat(DiagonalMoves(fromPosition, board));
        }

        // Override the 'CanCaptureKing' method to only check fewer positions
        public override bool CanCaptureTheKing(Position fromPosition, Board board)
        {
            return DiagonalMoves(fromPosition, board)
                .Any(move =>
                {
                    Piece piece = board[move.ToPosition];
                    return piece != null && piece.Type == PieceType.King;
                });
        }
    }


}
