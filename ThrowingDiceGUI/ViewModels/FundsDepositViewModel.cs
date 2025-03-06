using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class FundsDepositViewModel : ReactiveValidationObject, IDisposable
	{
		private static string _DEPOSIT_ERROR = "Deposit_Error";

		private readonly GameLogic _gameLogic;
		private string _inputFundsDeposit;
		private string _inputErrorText;
		private int _currentFundsValue;
		private bool _isFundPanelVisible;
		private bool _isGameInCompleteState;

		private static readonly Regex InputFundsRegex = new Regex(@"^\d+$");
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		public FundsDepositViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
			InputErrorText = string.Empty;

			// Validates and Assignes Funds value
			InputFundDepositCommand = ReactiveCommand.Create<string>(deposit =>
			{
			
				if (InputFundsRegex.IsMatch(deposit) && int.TryParse(deposit, out int depositAmount) && (depositAmount >= 100 && depositAmount <= 5000))
				{
					_gameLogic.SetIncomingDeposit(depositAmount);
					IsFundPanelVisible = false;
					_gameLogic.AskForBet();
					InputErrorText = string.Empty;
				}
				else
				{
					InputErrorText = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
				}
			});


			// Updates gamestate values relevent for this viewmodel
			_gameLogic.GameStateObservable.Subscribe(gameState =>
			{
				IsGameInCompleteState= gameState.GameIsInCompleteState;
				CurrentFunds = gameState.CurrentFunds;
				// Fund panel is visible only when gamelogic is waiting for deposit input 
				IsFundPanelVisible = gameState.IsWaitingOnDeposit;


			}).DisposeWith(_disposables);
		}

		// New fund deposit recived
		public ReactiveCommand<string, Unit> InputFundDepositCommand { get; }

		// Expose Validation Text for UI Binding
		public string InputErrorText
		{
			get => _inputErrorText;
			private set => this.RaiseAndSetIfChanged(ref _inputErrorText, value);
		}

		// Enable or disable FundPanel visibility 
		public bool IsFundPanelVisible
		{
			get => _isFundPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isFundPanelVisible, value);
		}

		// Active if bet has been regestered and first throw of round has been conducted
		public bool IsGameInCompleteState
		{
			get => _isGameInCompleteState;
			set => this.RaiseAndSetIfChanged(ref _isGameInCompleteState, value);
		}

		public int CurrentFunds
		{
			get => _currentFundsValue;
			set => this.RaiseAndSetIfChanged(ref _currentFundsValue, value);
		}

		// houses the inputed value for fund deposit
		public string InputFundsDeposit
		{
			get => _inputFundsDeposit;
			set => this.RaiseAndSetIfChanged(ref _inputFundsDeposit, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
