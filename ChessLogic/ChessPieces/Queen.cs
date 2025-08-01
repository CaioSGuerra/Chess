using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
        public override Player Color { get; }

        public Queen(Player color)
        {
            Color = color;
        }

        // A direction array that contains all directions
        private static readonly Direction[] directionVector = new Direction[]
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
        public override Piece Copy()
        {
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // A method to implement 'GetMoves' por super class
        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return MovePositionsInDirection(fromPosition, board, directionVector).Select(toPosition => new NormalMove(fromPosition, toPosition));
        }
    }
}
