using ChessLogic.ChessPiece;
using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        public NormalMove(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }

        public override void Execute(Board board)
        {
            // Get a piece from position
            Piece piece = board[FromPosition];
            // Place in a To position
            board[ToPosition] = piece;
            // Remove from the original position
            board[FromPosition] = null;
            // Assign the Method 'Has Moved' to true
            piece.HasMoved = true;
        }
    }
}
