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
using System.Threading;





namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ReactiveValidationObject
	{
		private readonly CompositeDisposable _disposables = new CompositeDisposable();
		
		private readonly GameLogic _gameLogic;
		private string _message;
		private string _secondaryMessage;
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
			NextRoundCommand = ReactiveCommand.Create(_gameLogic.NextRound);   

			_gameLogic.GameStateObservable.Subscribe(gameState =>
			{
				Message = gameState.MessageValue;
				SecondaryMessage = gameState.SecondaryMessageValue;

				if (SecondaryMessage != string.Empty) 
				{
					Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(_ => 
						gameLogic.ClearSecondaryMessage()
					).DisposeWith(_disposables);
				} 

				PlayerScore = gameState.PlayerScore;
				NpcScore = gameState.NpcScore;

				// "New Game" button will be visible when game starts and after game was lost
				IsNewGameButtonVisible = !gameState.IsGameStarted && !gameState.FundsAreSet;//gameState.GameIsOver;
				// "Next Round" button is visible only after a winner of current game has been chosen 
				IsNextRoundButtonVisible = gameState.GameIsInCompleteState && gameState.FundsAreSet && gameState.IsGameStarted;

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

		public string SecondaryMessage
		{
			get => _secondaryMessage;
			set => this.RaiseAndSetIfChanged(ref _secondaryMessage, value);  // Notifies UI when changed
		}
	}
}
