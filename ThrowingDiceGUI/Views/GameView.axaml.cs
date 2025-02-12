using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ThrowingDiceGUI.ViewModels;

namespace ThrowingDiceGUI.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    private void OnDepositButtonClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is GameViewModel viewModel)
        {
			string input = DepositTextBox.Text;
            viewModel.SubmitDeposit(input);
		}
        
        
    }
}