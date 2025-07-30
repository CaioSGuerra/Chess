
using ChessLogic.Enum;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; set; }
        public Player CurrentPLayer { get; private set; }

        public GameState(Player player, Board board)
        {
            CurrentPLayer = player;
            Board = board;
        }
    }
}
