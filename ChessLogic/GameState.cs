
using ChessLogic.ChessPiece;
using ChessLogic.Enum;
using ChessLogic.Moves;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; set; }
        public Player CurrentPLayer { get; private set; }
        public Result Result { get; private set; } = null;

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
            // This line disables any En Passant move by clearing the skip position.
            // The En Passant rule allows a pawn capture only on the immediately following move.
            // If any other piece moves, the En Passant opportunity is lost.
            // Therefore, clear the skip position on the next turn of the vulnerable pawn’s player.
            Board.SetPawnSkipPosition(CurrentPLayer, null);

            move.Execute(Board);
            CurrentPLayer = CurrentPLayer.Opponent();
            CheckForGameOver(); // Use 'CheckForGameOver' method to check after each move has been made
        }

        // A Method to generate all moves the player can make
        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            // Create a variable of all candidate moves
            IEnumerable<Move> moveCandidates =
                Board.PiecePositionsFor(player) // Get all positions containing a selected player position
                .SelectMany(position => // Collect moves for each piece
                {
                    Piece piece = Board[position];
                    return piece.GetMoves(position, Board); // This give a collection of all moves the player can make including the illegal ones
                });

            return moveCandidates.Where(move => move.IsLegal(Board));  // This remove all illegal moves
        }

        // This method is to check every end of turn if the current player can move
        private void CheckForGameOver()
        {
            // If the new player does have not any legal moves
            if (!AllLegalMovesFor(CurrentPLayer).Any())
            {
                // Then check if it's a Checkmate or Stalemate
                if (Board.IsInCheck(CurrentPLayer))
                {
                    // If the current player is in  check, then it's in Checkmate so the previous player won
                    Result = Result.Win(CurrentPLayer.Opponent());
                }
                else
                {
                    // Otherwise the match is in a stalemate and the game ends with draw
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            // If there's not enough piece to let a king in checkmate then it's a draw
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial); ;
            }

        }

        public bool IsGameOver()
        {
            // Ends the game if Result have a EndReason reason
            return Result != null;
        }
    }
}
