using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic.ChessPiece
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }

        // A direction array that contains all vertical and horizontal directions
        private static readonly Direction[] directionVector = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.West,
            Direction.South
        };
        public Rook(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
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
