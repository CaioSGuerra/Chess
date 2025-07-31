using ChessLogic;
using ChessLogic.ChessPiece;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];

        private GameState gameState;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            gameState = new GameState(ChessLogic.Enum.Player.White, Board.Initial());
            DrawBoard(gameState.Board);
        }

        private void InitializeBoard()
        {
            //Matrix for positions
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    //Creating a image control
                    Image image = new Image();

                    //store in a 2d array
                    pieceImages[row, column] = image;

                    //add as a child to uniform grid
                    PieceGrid.Children.Add(image);

                }
            }
        }

        //This set the source of all image controls so they match the pieces on that board
        private void DrawBoard(Board board)
        {
            //Matrix for positions
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    //Take the piece of the position
                    Piece piece = board[row, column];
                    //Update the source of correspond image control
                    pieceImages[row, column].Source = Images.GetImage(piece);
                }
            }
        }
    }
}