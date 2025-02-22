using ReactiveUI;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels;

public class ViewModelBase : ReactiveObject
{
	protected readonly GameLogic _gameLogic;

	public ViewModelBase()
	{
		_gameLogic = new GameLogic();
	}
}
