namespace ChessLogic.Enum
{
    public enum Player
    {
        None = 0,
        White = 1,
        Black = 2
    }

    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            return player switch
            {
                Player.White => Player.Black,
                Player.Black => Player.White,
                _ => Player.None
            };
        }
    }
}
