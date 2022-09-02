using System.Collections.Generic;
using System.Windows;

namespace Week8Project
{
    internal static class NavigationService
    {
        private static readonly Stack<Window> NavigationStack = new Stack<Window>();

        static NavigationService()
        {
            NavigationStack.Push(Application.Current.MainWindow);
        }
        // Navigate to specified window
        public static void NavigateTo(Window window)
        {
            // Hide current window
            if (NavigationStack.Count > 0)
                NavigationStack.Peek().Hide();
            // Add new window to stack and display
            NavigationStack.Push(window);
            window.Show();
        }
        // Navigate to previous window
        public static bool NavigateBack()
        {
            // Verify can navigate to previous window
            if (NavigationStack.Count <= 1)
                return false;
            // Close current window, display previous window
            NavigationStack.Pop().Close();
            NavigationStack.Peek().Show();
            return true;
        }
        // Verify window to navigate back to
        public static bool CanNavigateBack()
        {
            return NavigationStack.Count > 1;
        }
        // Get window on top of stack
        public static Window PeekStack()
        {
            return NavigationStack.Peek();
        }
    }
}
