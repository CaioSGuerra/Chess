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
        // MainWindow register an event handler for this event and when the player click on any button Invoke the event handler passing the selected option
        public event Action<Option> OptionSelected;


        // This will open the game over window when the Game State return a result value, then send the reason or/and Player 
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPLayer);
        }

        // This method get the winner player color and return a text according to the winner's color
        // Also return a Draw if none the method don't get any color
        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "WHITE WINS!",
                Player.Black => "BLACK WINS!",
                _ => "IT'S A DRAW"
            };
        }

        // Turn player color into a string
        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "WHITE",
                Player.Black => "BLACK",
                _ => ""
            };
        }

        // Use turned string on end reason text
        private static string GetReasonText(EndReason endReason, Player currentPLayer)
        {
            return endReason switch
            {
                EndReason.Stalemate => $"STALEMATE - {PlayerString(currentPLayer)} CAN'T MOVE",
                EndReason.Checkmate => $"CHECKMATE - {PlayerString(currentPLayer)} CAN'T MOVE",
                EndReason.FiftyMoveRule => $"FIFTY-MOVE RULE",
                EndReason.InsufficientMaterial => $"INSUFICIENT MATERIAL",
                EndReason.ThreefoldRepetition => $"THREEFOLD REPETITION",
                _ => ""
            };
        }

        // Passing event 'Restart' in Restart button is clicked
        // '?' ensure the event only raise if there's an event handler registered
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Enum.Option.Restart);
        }

        // Passing event 'Exit' in Exit button is clicked
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Enum.Option.Exit);
        }
    }
}
