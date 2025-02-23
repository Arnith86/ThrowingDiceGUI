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





namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ReactiveValidationObject
	{
		private readonly CompositeDisposable _disposables = new CompositeDisposable();
		
		private static string _DEPOSIT_ERROR = "Deposit_Error";
		private static string _CURRENT_BALANCE = "Current_Balance";
		private static string _START_BET = "Start_Bet";
		private static string _BET_ERROR_INT = "Bet_Error_Int";
		private static string _CURRENT_BET = "Current_Bet";
		private static string _BET_BALANCE_ERROR = "Bet_Balance_Error";
		private static string _THROW_DIE = "Throw_Die";
		private static string _NEW_THROW = "New_Throw";
		private static string _SHOW_DIE = "Show_Die";
		private static string _PLAYER_ROUND_WIN = "Player_Round_Win";
		private static string _NPC_ROUND_WIN = "Npc_Round_Win";
		private static string _PLAYER_GAME_WIN = "Player_Game_Win";
		private static string _NPC_GAME_WIN = "Npc_Game_Win";
		private static string _BUTTON_PRESS = "Button_Press";
		private static string _CONTINUE = "Continue";
		private static string _FOUNDS_ADDED = "Founds_Added";
		private static string _YES_NO_ERROR = "Yes_No_Error";
		private static string _END_GAME = "End_Game";


		private readonly GameLogic _gameLogic;
		private string _message; 
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
			IsNewRoundButtonVisible = false;
	
			_gameLogic = gameLogic;

			StartGameCommand = ReactiveCommand.Create(_gameLogic.AskForDeposit);
			NewRoundCommand = ReactiveCommand.Create(_gameLogic.NewRound);

			_gameLogic.MessageObservable.Subscribe( message =>
				Message = message
			).DisposeWith(_disposables);
			
			_gameLogic.IsStartButtonVisibleObservable.Subscribe(isStartButtonVisible =>
				IsStartButtonVisible = isStartButtonVisible
			).DisposeWith(_disposables);

			_gameLogic.IsNewRoundButtonVisibleObservable.Subscribe(isNewRoundButtonVisible =>
				IsNewRoundButtonVisible = isNewRoundButtonVisible
			).DisposeWith(_disposables);

			// Subscribes to current results, when player or npc reach 2 wins game ends.
			this.WhenAnyValue(GameViewModel => GameViewModel.PlayerScore, GameViewModel => GameViewModel.NpcScore).
			Subscribe(scores =>
			{
				// IMPLEMENT THIS WHEN READY
				//if (scores.Item1 == 2) // Player Wins
				//{
				//	IsThrowButtonVisible = false;
				//	IsNewRoundButtonVisible = true;
				//	IsBetPanelVisible = false;
				//	Message = Messages.Instance.GetMessage(_PLAYER_GAME_WIN);
				//	//CurrentBalance = _game.CurrentBalance += (CurrentBet * 2);
				//	//_game.UpdateFunds(CurrentFunds);
				//	_gameLogic.UpdateFunds(_gameLogic.CurrentFundsValue + (_gameLogic.BetValue * 2));
				//}
				//else if (scores.Item2 == 2) // Npc Wins
				//{
				//	IsThrowButtonVisible = false;
				//	IsNewRoundButtonVisible = true;
				//	IsBetPanelVisible = false;
				//	Message = Messages.Instance.GetMessage(_NPC_GAME_WIN);
				//}
			});

			// IMPLEMENT THIS WHEN READY
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





		public int PlayerScore
		{
			get => _playerScore;
			set => this.RaiseAndSetIfChanged(ref _playerScore, value) ;
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
