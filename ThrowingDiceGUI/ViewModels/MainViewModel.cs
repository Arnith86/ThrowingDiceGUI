using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels;

public class MainViewModel : ViewModelBase
{
	public GameViewModel GameVM { get; }
	public FundsDepositViewModel FundsDepositVM { get; }
	public BetViewModel BetVM { get; }
	public RoundViewModel RoundVM { get; }

	public bool isGameStarted;

	public MainViewModel()
	{
		isGameStarted = false;

		GameVM = new GameViewModel(_gameLogic);
		FundsDepositVM = new FundsDepositViewModel(_gameLogic);
		BetVM = new BetViewModel(_gameLogic);
		RoundVM = new RoundViewModel(_gameLogic);
	}
}
