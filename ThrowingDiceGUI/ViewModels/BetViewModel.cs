﻿using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class BetViewModel : ReactiveObject, IDisposable
	{
		private static string _BET_BALANCE_ERROR = "Bet_Balance_Error";

		private readonly GameLogic _gameLogic;

		private bool _isBetPanelVisible;
		private bool _betButtonsEnabled;
		private bool _isBetLockedIn;
		private int _currentFunds = 0;
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
				// Validates bet and sets value if valid 
				if (int.TryParse(inputBet, out int bet) && bet <= CurrentFunds)
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

			_gameLogic.GameStateObservable.Subscribe(gameState => 
			{
				CurrentBet = gameState.Bet;
				CurrentFunds = gameState.CurrentFunds;
				IsBetLockedIn = gameState.IsBetLockedIn;

				// Bet buttons are enabled as long as bet is not locked in
				BetButtonsEnabled = !gameState.IsBetLockedIn;
				// Bet panel is visible when funds are set and a game round is active (asking of bet -> winner has been chosen)
				IsBetPanelVisible = !gameState.GameIsInCompleteState && gameState.FundsAreSet;
			});

		}

		// Expose Validation Text for UI Binding
		public string InputErrorText
		{
			get => _inputErrorText;
			private set => this.RaiseAndSetIfChanged(ref _inputErrorText, value);
		}

		public int CurrentBet
		{
			get => _currentBet;
			set => this.RaiseAndSetIfChanged(ref _currentBet, value);
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

		public bool IsBetLockedIn
		{
			get => _isBetLockedIn;
			set => this.RaiseAndSetIfChanged(ref _isBetLockedIn, value);
		}

		public int CurrentFunds
		{
			get => _currentFunds;
			set => this.RaiseAndSetIfChanged(ref _currentFunds, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
