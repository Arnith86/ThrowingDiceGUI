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
		private bool _isGameStarted;
		private bool _isStartButtonVisible;
		private bool _isNewRoundButtonVisible;
		private int _playerScore;
		private int _npcScore;
	
		// Start Game button has been pressed
		public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
		
		// New round button pressed
		public ReactiveCommand<Unit, Unit> NewRoundCommand { get; }


		public GameViewModel(GameLogic gameLogic)
		{
			// Sets the initial settings
			IsGameStarted = false;
			
			IsNewRoundButtonVisible = false; // Remove when proper solution is found
	
			_gameLogic = gameLogic;

	
			// The "Start" button has been pressed, starting the game proper.
			StartGameCommand = ReactiveCommand.Create(() => { 
				IsGameStarted = true; 
				_gameLogic.NewRound(); 
			});

			NewRoundCommand = ReactiveCommand.Create(_gameLogic.NewRound);

			_gameLogic.MessageObservable.Subscribe( message =>
				Message = message
			).DisposeWith(_disposables);

			_gameLogic.IsGameStartedObservable.Subscribe(isGameStarted =>
				IsStartButtonVisible = !isGameStarted
			).DisposeWith(_disposables);

			_gameLogic.IsNewRoundButtonVisibleObservable.Subscribe(isNewRoundButtonVisible =>
				IsNewRoundButtonVisible = isNewRoundButtonVisible
			).DisposeWith(_disposables);	

			_gameLogic.PlayerScoreObservable.Subscribe(playerScore =>
			{
				PlayerScore = playerScore;
			}).DisposeWith(_disposables);

			_gameLogic.NpcScoreObservable.Subscribe(npcScore =>
			{
				NpcScore = npcScore;
			}).DisposeWith(_disposables);

		}

		public bool IsGameStarted
		{
			get => _isGameStarted;
			set => this.RaiseAndSetIfChanged(ref _isGameStarted, value);
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
		// Start Button
		public bool IsStartButtonVisible
		{
			get => _isStartButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isStartButtonVisible, value);
		}

		// Start new round button
		public bool IsNewRoundButtonVisible
		{
			get => _isNewRoundButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isNewRoundButtonVisible, value);
		}

		// Updates the displayed information message 
		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}
	}
}
