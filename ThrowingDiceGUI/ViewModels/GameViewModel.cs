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
	public class GameViewModel : ReactiveValidationObject, IDisposable
	{
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private static string _WELCOME = "Welcome";
		private static string _START_DEPOSIT = "Start_Deposit";
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
		private bool _isInputPanelVisible;
		private bool _isThrowButtonVisible;
		private bool _isNewRoundButtonVisible;
		private bool _isRoundStarted;
		private int _playerScore;
		private int _npcScore;
		private Bitmap _playerDiceImage1;
		private Bitmap _playerDiceImage2;
		private Bitmap _npcDiceImage1;
		private Bitmap _npcDiceImage2;


		// Start Game button has been pressed
		public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
		// Throw button pressed
		public ReactiveCommand<Unit, Unit> ThrowCommand { get; }
		// New round button pressed
		public ReactiveCommand<Unit, Unit> NewRoundCommand { get; }


		public GameViewModel(GameLogic gameLogic)
		{

			

			// Sets the initial settings
			_message = Messages.Instance.GetMessage(_WELCOME);

			IsStartButtonVisible = true;
			IsInputPanelVisible = false;
			IsThrowButtonVisible = false;
			IsNewRoundButtonVisible = false;
	
			_gameLogic = gameLogic;

			// Display initial values
			updateDiceImages();

			
			StartGameCommand = ReactiveCommand.Create(AskForDeposit);

			ThrowCommand = ReactiveCommand.Create(StartRound);

			NewRoundCommand = ReactiveCommand.Create(NewRound);

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

		// The whole Input panel 
		public bool IsInputPanelVisible
		{
			get => _isInputPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isInputPanelVisible, value);
		}
		
		// Dice throw button 
		public bool IsThrowButtonVisible
		{
			get => _isThrowButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isThrowButtonVisible, value);
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


		// When a round is started the bet buttons will be disabled
		public bool IsRoundStarted
		{
			get => _isRoundStarted;
			set => this.RaiseAndSetIfChanged(ref _isRoundStarted, value);
		}

		
		// binding and updating of the Dice UI elements
		public Bitmap PlayerDiceImage1
		{
			get => _playerDiceImage1;
			set => this.RaiseAndSetIfChanged(ref _playerDiceImage1, value);
		}

		public Bitmap PlayerDiceImage2
		{
			get => _playerDiceImage2;
			set => this.RaiseAndSetIfChanged(ref _playerDiceImage2, value);
		}

		public Bitmap NpcDiceImage1
		{
			get => _npcDiceImage1;
			set => this.RaiseAndSetIfChanged(ref _npcDiceImage1, value);
		}

		public Bitmap NpcDiceImage2
		{
			get => _npcDiceImage2;
			set => this.RaiseAndSetIfChanged(ref _npcDiceImage2, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}

		// Starts the game
		// Ask for deposit to funds
		private void AskForDeposit()
		{
			//Message = Messages.Instance.GetMessage(_START_DEPOSIT);
			//IsStartButtonVisible = false;
			//IsInputPanelVisible = true;
			//FundsDeposit IsFundPanelVisible = true; // Figure out how to seperate ( put in FundsDepositViewModel)
		}

		// Ask which bet to place
		private void AskForPlaceBet()
		{
			//IsBetPanelVisible = true;
			//BetButtonsEnabled = true;
			//Message = Messages.Instance.GetMessage(_START_BET);
		}

		//// Registers deposit to funds
		//public void AddFundsDeposit(int deposit)
		//{
		//	CurrentFunds = deposit;
		//	IsFundPanelVisible = false;
		//	AskForPlaceBet();
		//}

		//// Registers deposit to funds
		//public void AddFundsDeposit()
		//{
		//	// Converts string to int and checks if input is between 100 and 5000
		//	if (int.TryParse(InputFundsDeposit, out int depositAmount) && _game.SetAndCheckDeposit(depositAmount))
		//	{
		//		CurrentBalance = depositAmount;
		//		IsFundPanelVisible = false;
		//		AskForPlaceBet();
		//	}
		//	else
		//	{
		//		Message = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
		//	}
		//}

		// Registers chosen bet
		public void RegisterBet()
		{
			//// Converts string to int and checks if bet exceed funds
			//if (int.TryParse(InputBet, out int betAmount) && _gameLogic.SetAndCheckBet(betAmount))
			//{
			//	CurrentBet = betAmount;
			//	CurrentFunds = _gameLogic.CurrentFundsValue;
			//	Message = Messages.Instance.GetMessage(_THROW_DIE); 
			//	IsThrowButtonVisible = true;
			//}
			//else
			//{
			//	Message = Messages.Instance.GetMessage(_BET_BALANCE_ERROR);
			//}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		private void StartRound()
		{
			IsThrowButtonVisible = false;
			IsRoundStarted = true; // when true, bet buttons are disabled
			_gameLogic.ThrowDiceSet(_gameLogic.PlayerDice);
			_gameLogic.ThrowDiceSet(_gameLogic.NpcDice);
			_gameLogic.PlayerDice = _gameLogic.SorByDescending(_gameLogic.PlayerDice);
			_gameLogic.NpcDice = _gameLogic.SorByDescending(_gameLogic.NpcDice);
			
			updateDiceImages();

			// Both player and npc dice are equal, a new throw will be conducted 
			if (_gameLogic.CheckIdenticalDiceSet(_gameLogic.PlayerDice, _gameLogic.NpcDice))
			{
				Message = Messages.Instance.GetMessage(_NEW_THROW);
			}
			else if (_gameLogic.RoundEvaluation(_gameLogic.PlayerDice, _gameLogic.NpcDice))
			{
				Message = Messages.Instance.GetMessage(_PLAYER_ROUND_WIN);
				PlayerScore++;
			}
			else
			{
				Message = Messages.Instance.GetMessage(_NPC_ROUND_WIN);
				NpcScore++;
			}

			if (PlayerScore != 2 && NpcScore != 2) IsThrowButtonVisible = true;  
		}

		private void updateDiceImages()
		{
			PlayerDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.PlayerDice[0].DiceImagePath)));
			PlayerDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.PlayerDice[1].DiceImagePath)));

			NpcDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.NpcDice[0].DiceImagePath)));
			NpcDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.NpcDice[1].DiceImagePath)));
		}

		// Prepers for and starts a new round
		// If not enough funds, sends to "AskForDeposit" method, otherwise "PlaceBet"
		private void NewRound()
		{
			//PlayerScore = 0;
			//NpcScore = 0;
			//CurrentBet = 0;
			//IsNewRoundButtonVisible = false;

			//if (CurrentFunds < 100)
			//{
			//	AskForDeposit();
			//}
			//else
			//{
			//	AskForPlaceBet();
			//}

		}
	}
}
