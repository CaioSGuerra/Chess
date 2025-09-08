using ChessLogic.ChessPiece;
using ChessLogic.Enum;
using System.Text;

namespace ChessLogic
{
    public class StateString
    {
        // The purpose of this class is to create a string that stores all piece positions
        // If the same string appears three times, the game ends in a draw due to "Threefold Repetition"
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public StateString(Player currentPlayer, Board board)
        {
            AddPiecePlacement(board);
            stringBuilder.Append(' ');
            AddCurrentPlayer(currentPlayer);
            stringBuilder.Append(' ');
            AddCastlingRights(board);
            stringBuilder.Append(' ');
            AddEnPassant(board, currentPlayer);
        }

        // Returns the final string
        // Used for comparison purposes for Threefold repetition
        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        // This method creates a string based on the piece's type and color.
        private static char PieceChar(Piece piece)
        {
            // stores the first character of each piece to create a string
            char character = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' '
            };

            if (piece.Color == Player.White)
            {
                return char.ToUpper(character);
            }

            return character;
        }

        // This method scans each position in a row and writes the result in a string
        // It counts consecutive empty squares and represents them with an integer
        // For non-empty squares, it invokes 'PieceChar' to write a letter based on the piece found
        // This is required to enable checking whether a player repeats the same board positions, as per the Threefold Repetition rule
        private void AddRowData(Board board, int row)
        {
            int empty = 0;
            for (int column = 0; column < 8; column++)
            {
                // Count empty squares
                if (board[row, column] == null)
                {
                    empty++;
                    continue;
                }

                // Write the current number in 'empty' and reset it
                if (empty > 0)
                {
                    stringBuilder.Append(empty);
                    empty = 0;
                }

                // invoke the method 'PieceChar' and write the piece type returned by this method
                stringBuilder.Append(PieceChar(board[row, column]));
            }

            // Print the last value at end of the loop if there's a empty square
            if (empty > 0)
            {
                stringBuilder.Append(empty);
            }
        }

        // This method creates a single string by combining data from 'AddRowData'
        // It merges each row's data into one line.
        // This is required to implement the Threefold Repetition rule
        private void AddPiecePlacement(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0)
                {
                    stringBuilder.Append('/');
                }

                AddRowData(board, row);
            }

        }

        // a simple method to add the current player color in State String
        private void AddCurrentPlayer(Player currentPlayer)
        {
            if (currentPlayer == Player.White)
            {
                stringBuilder.Append('w');
            }
            else
            {
                stringBuilder.Append('b');
            }
        }

        // This method writes each side's castling rights into a string
        // The string allows the program to check whether either the king or rook has moved, which affects Threefold Repetition
        // For Threefold Repetition to apply, all aspects of the position must be identical — including castling rights
        private void AddCastlingRights(Board board)
        {
            bool castleWhiteKingSide = board.CastleRightKingSide(Player.White);
            bool castleWhiteQueenSide = board.CastleRightQueenSide(Player.White);
            bool castleBlackKingSide = board.CastleRightKingSide(Player.Black);
            bool castleBlackQueenSide = board.CastleRightQueenSide(Player.Black);

            if (!(castleWhiteKingSide || castleWhiteQueenSide || castleBlackKingSide || castleBlackQueenSide))
            {
                stringBuilder.Append('-');
                return;
            }

            if (castleWhiteKingSide)
            {
                stringBuilder.Append('K');
            }
            if (castleWhiteQueenSide)
            {
                stringBuilder.Append('Q');
            }
            if (castleBlackKingSide)
            {
                stringBuilder.Append('k');
            }
            if (castleBlackQueenSide)
            {
                stringBuilder.Append('q');
            }
        }
        // This method writes the en passant target square into a string
        // If en passant is not possible, it writes '-'
        // It converts the column to a file ('a'–'h') and the row to a rank ('1'–'8')
        private void AddEnPassant(Board board, Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                stringBuilder.Append('-');
                return;
            }

            Position position = board.GetPawnSkipPosition(currentPlayer.Opponent());
            char file = (char)('a' + position.Column);
            int rank = 8 - position.Row;
            stringBuilder.Append(file);
            stringBuilder.Append(rank);
        }
    }
}
