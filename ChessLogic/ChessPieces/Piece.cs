using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position fromPosition, Board board);

        //Given the position of piece and a direction to find all reachable positions in that direction
        //If is the opposite color, turn into to reachable, if is not unreachable
        protected IEnumerable<Position> MovePositionsInDirection(Position fromPosition, Board board, Direction direction)
        {
            for (Position position = fromPosition + direction; Board.IsInside(position); position += direction)
            {
                // Check if the new position is empty
                if (board.IsEmpty(position))
                {
                    yield return position;
                    continue;
                }

                Piece piece = board[position];

                // Check if the piece in the new position have a diferent color
                if (piece.Color != Color)
                {
                    yield return position;
                }

                yield break;
            }
        }

        // A Method which takes an array of directions
        protected IEnumerable<Position> MovePositionsInDirection(Position fromPosition, Board board, Direction[] directionVector)
        {
            // Collect all reachable positions for all ggiven directions
            return directionVector.SelectMany(direction => MovePositionsInDirection(fromPosition, board, direction));
        }
    }
}
