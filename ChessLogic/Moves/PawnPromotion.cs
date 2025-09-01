using ChessLogic.ChessPiece;
using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly PieceType _newType;  // This variable will contain the type of piece the pawn will be promoted to

        public PawnPromotion(Position fromPosition, Position toPosition, PieceType newType)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            _newType = newType;
        }

        // Returns a new chess piece of the chosen type for the specified player color.
        private Piece CreatePromotionPiece(Player color)
        {
            return _newType switch
            {
                PieceType.Knight => new Knight(color),
                PieceType.Bishop => new Bishop(color),
                PieceType.Rook => new Rook(color),
                _ => new Queen(color)
            };
        }

        // Executes a pawn promotion: removes the pawn and places the chosen promotion piece on the board
        public override void Execute(Board board)
        {
            // Remove the pawn from its current position
            Piece pawn = board[FromPosition];
            board[FromPosition] = null;

            // Create the promotion piece with the same color as the removed pawn
            // and place it at the target position
            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPosition] = promotionPiece;
        }
    }
}
