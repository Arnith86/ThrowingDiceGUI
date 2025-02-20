using ReactiveUI;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels;

public class ViewModelBase : ReactiveObject
{
	public GameViewModel GameVM { get; }
	public FundsDepositViewModel FundsDepositVM { get; }

	private readonly GameLogic _gameLogic;

	public ViewModelBase()
	{
		_gameLogic = new GameLogic();
		//GameVM = new GameViewModel(_gameLogic);
		FundsDepositVM = new FundsDepositViewModel(_gameLogic);
	}
}
