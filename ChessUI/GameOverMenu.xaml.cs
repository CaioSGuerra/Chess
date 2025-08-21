using ChessLogic;
using ChessLogic.Enum;
using ChessUI.Enum;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        // The main window register an event handler for this game, when a player clicks on either button invoke the event handler passing in the selected option
        public event Action<Option> OptionSelected;

        public GameOverMenu(GameState gameState) // Takes the final game state as parameter
        {
            InitializeComponent();

            Result result = gameState.Result;    // Get the result of the game
            WinnerText.Text = GetWinnerText(result.Winner);    // Display the winning player
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPLayer);  // Dis0play the reason of the player win
        }

        // A Method to return a text message showing the winner or a draw
        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "WHITE WINS",
                Player.Black => "BLACK WINS!",
                _ => "IT'S A DRAW"
            };
        }

        // THis method turn the Enum Players to string
        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "White",
                Player.Black => "Black",
                _ => ""
            };
        }

        // A Method to return a string showing which is the reason of the end o match and which player won
        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                EndReason.Checkmate => $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                EndReason.FiftyMoveRule => "FIFTY-MOVE RULE ",
                EndReason.InsufficientMaterial => "INSUFFICIENT MATERIAL",
                EndReason.ThreefoldRepetition => "THREEFOLD REPETITION",
                _ => ""
            };
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            // If restart button is clicked, pass event 'Restart'
            OptionSelected?.Invoke(Option.Restart);
            // the '?' ensures that the event is only raised if there's an even handler registered
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // If exit button is clicked, pass event 'Exit'
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
