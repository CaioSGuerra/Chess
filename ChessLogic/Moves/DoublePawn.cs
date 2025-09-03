using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Position skippedPosition;

        public DoublePawn(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            // get the numbers of positions moved, then divide by 1 to get the skipped position
            // the skipped position will be always in the same column of move
            skippedPosition = new Position((fromPosition.Row + toPosition.Row) / 2, fromPosition.Column);
        }

        // This method stores the player color and the skipped position
        // Make 1 move, stores this move and move another move
        // With this you get the stored and actual position
        // This is a requirement for En Passant move which will capture the pawn on the stored position
        public override void Execute(Board board)
        {
            Player player = board[FromPosition].Color;
            board.SetPawnSkipPosition(player, skippedPosition);
            new NormalMove(FromPosition, ToPosition).Execute(board);
        }
    }
}
