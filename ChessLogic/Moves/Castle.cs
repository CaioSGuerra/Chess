using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Direction kingMoveDirection;
        private readonly Position rookFromPosition;
        private readonly Position rookToPosition;

        public Castle(MoveType type, Position kingPosition)
        {
            Type = type;
            FromPosition = kingPosition;

            // Check which side the player will do the castle move
            // If is in King side (right side of the board) move king 2 squares to the right
            // also moves the right side rook to the left side of king
            if (type == MoveType.CastleKingSide)
            {
                kingMoveDirection = Direction.East;                // Set which direction king can move
                ToPosition = new Position(kingPosition.Row, 6);      // the King position will be the side of the board
                rookFromPosition = new Position(kingPosition.Row, 7);      //if it's white the value should be 7, if it's black should be 0
                rookToPosition = new Position(kingPosition.Row, 5);          // it's a fixed column value because castle move is a horizontal only move
            }
            // if is in the queen side (left side of the board) move king 2 squares to the left
            // also moves the left rook to the right side of the king
            else if (type == MoveType.CastleQueenSide)
            {
                kingMoveDirection = Direction.West;
                ToPosition = new Position(kingPosition.Row, 2);
                rookFromPosition = new Position(kingPosition.Row, 0);
                rookToPosition = new Position(kingPosition.Row, 3);
            }
        }

        // this method move both King and rook, sending 2 positions to the 'NormalMove' class
        public override void Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            new NormalMove(rookFromPosition, rookToPosition).Execute(board);
        }

        // In castle move, you can't move if king in check, put in check situation and any position between origin and destiny can put King in check 
        public override bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color;

            // Check if King is in check
            if (board.IsInCheck(player))
            {
                return false;
            }

            // create a copy of board to make each king move
            Board copy = board.Copy();
            Position kingPositionInCopy = FromPosition;

            // in the copy body, put King piece in each position and check if any of that position is in check
            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPositionInCopy, kingPositionInCopy + kingMoveDirection).Execute(copy);
                kingPositionInCopy += kingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            // If in these step, none put King in check, enables the Castle Move
            return true;
        }
    }
}
