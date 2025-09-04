using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    public class EnPassant : Move
    {
        // En Passant is a move that happens when a pawn move 2 squares 
        // if the opponent pawn can capture the pawn if they only move 1 square
        // En Passant will capture ignoring the 2 moves square

        public override MoveType Type => MoveType.EnPassant;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Position capturePosition;

        public EnPassant(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            // It's a diagonal move, Pawn capture in diagonal
            // in En Passant then captured position will always be in the same row of captured pawn
            capturePosition = new Position(fromPosition.Row, toPosition.Column);
        }

        // THis method move the pawn behind to the captured pawn
        public override bool Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            board[capturePosition] = null;

            // Always return true because moves a pawn
            return true;
        }
    }
}
