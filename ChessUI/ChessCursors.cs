using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChessUI
{
    public static class ChessCursors
    {
        // White cursor file local
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");
        // Black cursor file local
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");

        // a method to create a cursor image
        private static Cursor LoadCursor(string filePath)
        {
            // Open a Stream to read a local file inside the project directory
            Stream stream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;

            // Apply the image data from the stream to the cursor
            // Setting dpi = true makes Windows automatically scale the cursor for the user's display settings
            return new Cursor(stream, true);
        }
    }
}
