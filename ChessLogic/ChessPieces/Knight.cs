using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // A help method to "jump" pieces, it show's all potential moves ignoring all pieces on the way
        private static IEnumerable<Position> PotentialToPositions(Position fromPosition)
        {
            // All vertical positions
            foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
            {
                // All horizontal positions
                foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                {
                    yield return fromPosition + (2 * verticalDirection) + horizontalDirection;
                    yield return fromPosition + (2 * horizontalDirection) + verticalDirection;
                }
            }
        }

        // a method to return all positions knight can actually move
        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            return PotentialToPositions(fromPosition).Where(position => Board.IsInside(position)
            && (board.IsEmpty(position) || board[position].Color != Color));
        }

        // The Method to move the piece
        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}
