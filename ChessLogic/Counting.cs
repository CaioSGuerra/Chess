using ChessLogic.Enum;

namespace ChessLogic
{
    public class Counting
    {
        // This class purpose is to store the amount of piece each player have
        // Dictionary stores the piece type and 'int' amount of a pieces stored
        private readonly Dictionary<PieceType, int> whiteCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();

        public int TotalCount { get; private set; }

        public Counting()
        {
            // To initialize 'Counting' class, the builder will loop through all pieceTypes and set them to 0
            foreach (PieceType type in System.Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        // This method will add a piece in each 'Dictionary' and to 'TotalCount'
        public void Increment(Player color, PieceType type)
        {
            if (color == Player.White)
            {
                whiteCount[type]++;
            }
            else if (color == Player.Black)
            {
                blackCount[type]++;
            }

            TotalCount++;
        }

        //  Shows the amount of white pieces
        public int White(PieceType type)
        {
            return whiteCount[type];
        }

        // Shows the amount of black pieces
        public int Black(PieceType type)
        {
            return blackCount[type];
        }
    }
}
