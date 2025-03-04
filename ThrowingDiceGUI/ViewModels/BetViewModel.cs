using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class BetViewModel : ReactiveObject, IDisposable
	{
		private static string _BET_BALANCE_ERROR = "Bet_Balance_Error";

		private readonly GameLogic _gameLogic;

		private bool _isBetPanelVisible;
		private bool _betButtonsEnabled;
		private int _currentBet;
		private string _inputErrorText;
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		// New bet input recived 
		public ReactiveCommand<string, Unit> InputBetCommand { get; }

		public BetViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
			IsBetPanelVisible = false;
			BetButtonsEnabled = true;
			InputErrorText = string.Empty;

			InputBetCommand = ReactiveCommand.Create<string>(inputBet =>
			{
				// a single temporary instance of the subscribed value is collected but not stored in the class
				_gameLogic.CurrentFundsObservable.Take(1).Subscribe(currentFundsValue =>
				{
					// Validates bet and sets value if valid 
					if (int.TryParse(inputBet, out int bet) && bet <= currentFundsValue)
					{
						CurrentBet = bet;
						_gameLogic.UpdateBet(CurrentBet);
						_gameLogic.ABetIsChosen();
						InputErrorText = string.Empty;
					}
					else
					{
						InputErrorText = Messages.Instance.GetMessage(_BET_BALANCE_ERROR);
					}
				});
			});

			_gameLogic.GameStateObservable.Subscribe(gameState => 
			{
				BetButtonsEnabled = !gameState.IsGameRoundStarted;
				IsBetPanelVisible = gameState.IsAwaitingBet;
			});
		}

		// Expose Validation Text for UI Binding
		public string InputErrorText
		{
			get => _inputErrorText;
			private set => this.RaiseAndSetIfChanged(ref _inputErrorText, value);
		} 

		// Changes visibility of Bet Input panel
		public bool IsBetPanelVisible
		{
			get => _isBetPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isBetPanelVisible, value);
		}

		// Disables the ability to bet when a game is started 
		public bool BetButtonsEnabled
		{
			get => _betButtonsEnabled;
			set => this.RaiseAndSetIfChanged(ref _betButtonsEnabled, value);
		}

		public int CurrentBet
		{
			get => _currentBet;
			set => this.RaiseAndSetIfChanged(ref _currentBet, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
