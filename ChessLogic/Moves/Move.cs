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

        // This method use a 2° board to execute the move then check if the king is in check after the move, if yes, then it disallow the main board to execute this movement
        public virtual bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color; // Get moving player by checking the color of piece that will move
            Board boardCopy = board.Copy();  // Create a 2° board using 'Copy' Method
            Execute(boardCopy); // Execute the move on the 2° board
            return !boardCopy.IsInCheck(player); // return true if the current players king is not in check

            // this method is a simple method, but is inefficient, for this project won't be any delay, but shouldn't use in more complex project
        }
    }
}
