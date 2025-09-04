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

        public override bool Execute(Board board)
        {
            // Get a piece from position
            Piece piece = board[FromPosition];

            // Checks if the destination square contains a piece. If true, it means a piece was captured during the move
            bool capture = !board.IsEmpty(ToPosition);

            // Place in a To position
            board[ToPosition] = piece;
            // Remove from the original position
            board[FromPosition] = null;
            // Assign the Method 'Has Moved' to true
            piece.HasMoved = true;

            // The 50-move rule allows a draw if no pawn moves or captures occur in 50 consecutive moves
            // This condition resets the move counter when a pawn is moved or a piece is captured
            return capture || piece.Type == PieceType.Pawn;
        }
    }
}
