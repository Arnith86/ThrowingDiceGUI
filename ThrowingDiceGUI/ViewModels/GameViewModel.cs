using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using ReactiveUI.Validation;
using ReactiveUI.Validation.Abstractions;
using System.ComponentModel.DataAnnotations;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Linq;





namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ReactiveValidationObject
	{
		private readonly CompositeDisposable _disposables = new CompositeDisposable();
		
		private static string _DEPOSIT_ERROR = "Deposit_Error";
		private static string _CURRENT_BALANCE = "Current_Balance";
		private static string _CURRENT_BET = "Current_Bet";
		private static string _SHOW_DIE = "Show_Die";
		private static string _BUTTON_PRESS = "Button_Press";
		private static string _CONTINUE = "Continue";
		private static string _FOUNDS_ADDED = "Founds_Added";
		private static string _YES_NO_ERROR = "Yes_No_Error";
		private static string _END_GAME = "End_Game";


		private readonly GameLogic _gameLogic;
		private string _message;
		private bool _isNewGameButtonVisible;
		private bool _isNextRoundButtonVisible;
		private int _playerScore;
		private int _npcScore;
	
		// Start a new round in current game
		public ReactiveCommand<Unit, Unit> NextRoundCommand { get; }
		
		// New round button pressed
		public ReactiveCommand<Unit, Unit> NewGameCommand { get; }


		public GameViewModel(GameLogic gameLogic)
		{
			// Sets the initial settings
			IsNewGameButtonVisible = false;
			IsNextRoundButtonVisible = false; 
			_gameLogic = gameLogic;


			// The "New Game" button has been pressed, Initiating a deposit to funds.
			NewGameCommand = ReactiveCommand.Create(() => {
				_gameLogic.StartNewGame();
			});
			
			// The "Next Round" button has been pressed 
			NextRoundCommand = ReactiveCommand.Create(_gameLogic.AskForBet);   

			_gameLogic.GameStateObservable.Subscribe(gameState =>
			{
				Message = gameState.MessageValue;
				// "New Game" button will be visible when game starts and after game was lost
				IsNewGameButtonVisible = !gameState.IsGameStarted;
				// "Next Round" button is visible only after a winner of current game has been chosen 
				IsNextRoundButtonVisible = gameState.GameIsInCompleteState && gameState.FundsAreSet && gameState.IsGameStarted;

			}).DisposeWith(_disposables);

			_gameLogic.PlayerScoreObservable.Subscribe(playerScore =>
			{
				PlayerScore = playerScore;
			}).DisposeWith(_disposables);

			_gameLogic.NpcScoreObservable.Subscribe(npcScore =>
			{
				NpcScore = npcScore;
			}).DisposeWith(_disposables);
		}

		public int PlayerScore
		{
			get => _playerScore;
			set => this.RaiseAndSetIfChanged(ref _playerScore, value);
		}

		public int NpcScore
		{
			get => _npcScore;
			set => this.RaiseAndSetIfChanged(ref _npcScore, value);
		}


		// Changes the visibility of UI elements
		// Start new round button
		public bool IsNewGameButtonVisible
		{
			get => _isNewGameButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isNewGameButtonVisible, value);
		}

		// Start next round button
		public bool IsNextRoundButtonVisible
		{
			get => _isNextRoundButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isNextRoundButtonVisible, value);
		}

		// Updates the displayed information message 
		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}
	}
}
