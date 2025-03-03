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
		//private readonly GameViewModel _gameViewModel;
		private string _inputFundsDeposit;
		private string _inputErrorText;
		private int _currentFundsValue;
		private bool _isFundPanelVisible;
		private bool _isGameStarted;
		private bool _isGameRoundStarted;

		private static readonly Regex InputFundsRegex = new Regex(@"^\d+$");
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		public FundsDepositViewModel(GameLogic gameLogic, GameViewModel gameViewModel) 
		{
			_gameLogic = gameLogic;
			//_gameViewModel = gameViewModel;
			InputErrorText = string.Empty;

			// Validates and Assignes Funds value
			InputFundDepositCommand = ReactiveCommand.Create<string>(deposit =>
			{
			
				if (InputFundsRegex.IsMatch(deposit) && int.TryParse(deposit, out int depositAmount) && (depositAmount >= 100 && depositAmount <= 5000))
				{
					_gameLogic.UpdateFunds(depositAmount);
					IsFundPanelVisible = false;
					_gameLogic.AskForPlaceBet();
					InputErrorText = string.Empty;
				}
				else
				{
					InputErrorText = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
				}
			});


			// Subscribe to balance updates from the GameLogic
			_gameLogic.CurrentFundsObservable.Subscribe(Funds =>
			{
				// Update the ViewModel when the balance changes
				CurrentFunds = Funds;
			}).DisposeWith(_disposables);


			//// Keeps track on if a gameround in under way.
			//// If active, funds cannot be added and no bets can be placed
			_gameLogic.GameStateObservable.Subscribe(gameState =>
			{
				IsGameRoundStarted = gameState.IsGameRoundStarted;
				IsGameStarted = gameState.IsGameStarted;
			}).DisposeWith(_disposables);


			// Funds panel is only visible if funds are less than 100, game has started and no gameround is active.
			this.WhenAnyValue(
				FundsDepositViewModel => FundsDepositViewModel.CurrentFunds,
				FundsDepositViewModel => FundsDepositViewModel.IsGameRoundStarted,
				FundsDepositViewModel => FundsDepositViewModel.IsGameStarted)
				.Subscribe(Values =>
			{
				IsFundPanelVisible = Values.Item1 < 100 && !Values.Item2 && Values.Item3;

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
		public bool IsGameStarted
		{
			get => _isGameStarted;
			set => this.RaiseAndSetIfChanged(ref _isGameStarted, value);
		}

		// Active if bet has been regestered and first throw of round has been conducted
		public bool IsGameRoundStarted
		{
			get => _isGameRoundStarted;
			set => this.RaiseAndSetIfChanged(ref _isGameRoundStarted, value);
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
