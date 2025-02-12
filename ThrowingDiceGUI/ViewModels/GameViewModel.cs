using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
	
		static string _WELCOME = "Welcome";
		static string _START_DEPOSIT = "Start_Deposit";
		static string _DEPOSIT_ERROR = "Deposit_Error";
		static string _CURRENT_BALANCE = "Current_Balance";
		static string _START_BET = "Start_Bet";
		static string _BET_ERROR_INT = "Bet_Error_Int";
		static string _CURRENT_BET = "Current_Bet";
		static string _BET_BALANCE_ERROR = "Bet_Balance_Error";
		static string _NEW_THROW = "New_Throw";
		static string _SHOW_DIE = "Show_Die";
		static string _PLAYER_ROUND_WIN = "Player_Round_Win";
		static string _NPC_ROUND_WIN = "Npc_Round_Win";
		static string _PLAYER_GAME_WIN = "Player_Game_Win";
		static string _NPC_GAME_WIN = "Npc_Game_Win";
		static string _BUTTON_PRESS = "Button_Press";
		static string _CONTINUE = "Continue";
		static string _FOUNDS_ADDED = "Founds_Added";
		static string _YES_NO_ERROR = "Yes_No_Error";
		static string _END_GAME = "End_Game";


		private readonly Gamelogic _game;
		private string _message; 
		private bool _isStartButtonVisible;
		private int _currentBalance;

		public GameViewModel()
		{
			_message = Messages.Instance.GetMessage(_WELCOME);
			_isStartButtonVisible = true;
			_game = new Gamelogic();

			StartGameCommand = ReactiveCommand.Create(StartGame);
			//DepositCommand = ReactiveCommand.Create(RegisterFunds);
		}

		public int PlayerScore => _game.PlayerScore;
		public int NpcScore => _game.NpcScore;
		
		public int CurrentBet => _game.CurrentBet;
		public ReactiveCommand<Unit, Unit> StartGameCommand { get; }	// Start Game button has been pressed
		//public ReactiveCommand<Unit, Unit> DepositCommand { get; }		// New deposit has been registered 


		// Updates the displayed information message 
		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}

		// Changes the visibility of the start button
		public bool IsStartButtonVisible
		{
			get => _isStartButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isStartButtonVisible, value);
		}

		// Starts the game by checking account balance
		private void StartGame() 
		{
			if (CurrentBalance < 100) 
			{
				Message = Messages.Instance.GetMessage(_START_DEPOSIT);
			}
		}

		public int CurrentBalance
		{
			get => _game.CurrentBalance;
			set => this.RaiseAndSetIfChanged(ref _currentBalance, value);
		}

		// Registers deposit to funds
		public void SubmitDeposit(string input)
		{
			// Converts string to int and checks if input is between 100 and 5000
			if (int.TryParse(input, out int depositAmount) && _game.SetAndCheckDeposit(depositAmount)) 
			{
				CurrentBalance = depositAmount;
			}
			else
			{
				Message = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
			}
		}
	}
}
