using ChessLogic;
using ChessLogic.ChessPiece;
using ChessLogic.Moves;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        // It will provide easy access to the highlight in a certain position just like piece images array does for the pieces
        private readonly Rectangle[,] multipleHighlight = new Rectangle[8, 8];
        // Stores all available moves from a selected piece
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private GameState gameState;
        //  store position of selected Piece
        private Position selectedPosition = null;

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

                    // Create a rectangle from each position
                    Rectangle highlight = new Rectangle();
                    multipleHighlight[row, column] = highlight;
                    HighLightGrid.Children.Add(highlight);

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

        // This method is called when a player clicks somewhere in the board
        private void BoardGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);
            // Call ToSqaurePosition to turn the clicked area in a square position
            Position position = ToSquarePosition(point);

            // If no piece is selected, invoke a method called OnFromPositionSelected
            if (selectedPosition == null)
            {
                OnFromPositionSelected(position);
            }
            else
            {
                OnToPositionSelected(position);
            }
        }

        // A help method to select a square when a point inside of it is clicked
        private Position ToSquarePosition(Point point)
        {
            // Since the game time will be always at the same proportional, it can be divide by the numbers of each row and column amount 
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);

            // return the clicked point in a new position
            return new Position(row, column);
        }

        // This method is called when a empty square is clicked
        private void OnFromPositionSelected(Position position)
        {
            //  this collection will be empty if the player click in a empty square, an opponent piece of a piece which can't move 
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(position);

            // If there's at least 1 available position,
            if (moves.Any())
            {
                // Consider the given position,
                selectedPosition = position;

                // Cache the move.
                CacheMoves(moves);

                // Highlight them
                ShowHighlights();
            }
        }

        // This method tells the GameState.cs to execute the given move
        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
        }
        private void OnToPositionSelected(Position position)
        {
            // reset the selected position, then hide highlights
            selectedPosition = null;
            HideHighlights();

            // Check if 'position' exists in moveCache, and if it does, store the value in 'move'
            if (moveCache.TryGetValue(position, out Move move))
            {
                HandleMove(move);
            }
        }

        // A method to take all available moves for the selected piece and stores then in the cache
        private void CacheMoves(IEnumerable<Move> storeMoves)
        {
            // Remove previous cache
            moveCache.Clear();

            // Loop over the given moves
            foreach (Move move in storeMoves)
            {
                // Store each of them in dictionary using two position as key
                moveCache[move.ToPosition] = move;
            }
        }

        // Highlight method
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            // Loop over the keys in move cache
            foreach (Position toPosition in moveCache.Keys)
            {
                // Change color for each position
                multipleHighlight[toPosition.Row, toPosition.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            // Loops through all highlighted positions
            foreach (Position toPosition in moveCache.Keys)
            {
                // Hide all highlighted positions
                multipleHighlight[toPosition.Row, toPosition.Column].Fill = Brushes.Transparent;
            }
        }
    }
}