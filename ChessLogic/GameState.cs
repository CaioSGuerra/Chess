
using ChessLogic.ChessPiece;
using ChessLogic.Enum;
using ChessLogic.Moves;

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

        // Take a position as a parameter and take all moves that piece can make
        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            // Check if you choose an legal position 
            if (Board.IsEmpty(position) || Board[position].Color != CurrentPLayer)
            {
                // Return an empty sequence of Move to indicate no possible moves from this position
                return Enumerable.Empty<Move>();
            }

            // Get the piece from chosen position
            Piece piece = Board[position];
            // return all moves it can make
            // Instead of make a move, now store a move then check if is a legal move by method 'IsLegal'
            IEnumerable<Move> moveCandidates = piece.GetMoves(position, Board); // Stores all moves
            return moveCandidates.Where(move => move.IsLegal(Board)); // Return all legal ones
        }

        // Create a method to make the piece move
        public void MakeMove(Move move)
        {
            move.Execute(Board);
            CurrentPLayer = CurrentPLayer.Opponent();
        }

    }
}
