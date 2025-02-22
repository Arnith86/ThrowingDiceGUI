using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class BetViewModel : ReactiveValidationObject, IDisposable
	{
		private readonly GameLogic _gameLogic;
		private bool _isBetPanelVisible;
		private bool _betButtonsEnabled;
		private int _currentBet;
		private string _inputBet;

		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		// New bet input recived 
		public ReactiveCommand<string, Unit> InputBetCommand { get; }

		public BetViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
			IsBetPanelVisible = false;
			BetButtonsEnabled = true;

			InputBetCommand = ReactiveCommand.Create<string>(bet =>
			{
				InputBet = bet;
				//RegisterBet(); // IMPLÖEMENT THIS LATER 
			});


			// IMPLEMENT WHEN READY 
			//// Disables the bet buttons after first throw of new game 
			//this.WhenAnyValue(GameViewModel => GameViewModel.IsRoundStarted).Subscribe(
			//	isRoundStarted =>
			//	{
			//		if (isRoundStarted)
			//		{
			//			BetButtonsEnabled = false;
			//		}
			//	}
			//);
		}

		// Bet Input panel
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

		// houses the inputed value for fund deposit
		public string InputBet
		{
			get => _inputBet;
			set => this.RaiseAndSetIfChanged(ref _inputBet, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
