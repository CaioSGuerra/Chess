using ChessLogic.Enum;

namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; } // Stores the player which win or a draw
        public EndReason Reason { get; }

        public Result(Player winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }


        // A Method for a win, it takes the winning player and set the winning reason
        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate);
        }

        // A Method for a draw result, return the winner player as none
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
