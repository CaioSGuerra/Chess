using ChessLogic.Enum;

namespace ChessLogic.Moves
{
    //base class for all moves
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPosition { get; }
        public abstract Position ToPosition { get; }

        //A method to make a move to happen
        public abstract void Execute(Board board);
    }
}
