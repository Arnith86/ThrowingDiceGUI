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
		private bool _isInputPanelVisible;
		private bool _isFundPanelVisible;
		private bool _isBetPanelVisible;
		private string? _inputFundsDeposit;
		private int _currentBalance;
		private string _inputBet;

		


		public GameViewModel()
		{
			_message = Messages.Instance.GetMessage(_WELCOME);
			_isStartButtonVisible = true;
			_isInputPanelVisible = false;
			_isBetPanelVisible = false;
			_isFundPanelVisible = false;
			_game = new Gamelogic();

			StartGameCommand = ReactiveCommand.Create(StartGameMessageAskDeposit);
			InputBetCommand = ReactiveCommand.Create<string>( bet => 
			{
				InputBet = bet;
				RegisterBet();
			});
		}

		public int PlayerScore => _game.PlayerScore; // This is shorthand for only getter
		public int NpcScore => _game.NpcScore;
		public ReactiveCommand<Unit, Unit> StartGameCommand { get; }    // Start Game button has been pressed
		public ReactiveCommand<string, Unit> InputBetCommand { get; }                                                              //public ReactiveCommand<Unit, Unit> DepositCommand { get; }		// New deposit has been registered 


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

		// Bet Input panel
		public bool IsBetPanelVisible
		{
			get => _isBetPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isBetPanelVisible, value);
		}

		// Bet Funds panel
		public bool IsFundPanelVisible
		{
			get => _isFundPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isFundPanelVisible, value);
		}



		// Updates the displayed information message 
		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}

		

		
		
		
		// Starts the game
		// Ask for deposit to funds
		private void StartGameMessageAskDeposit() 
		{
			Message = Messages.Instance.GetMessage(_START_DEPOSIT);
			IsStartButtonVisible = false;
			IsInputPanelVisible = true;
			IsFundPanelVisible = true;
		}

		// Ask which bet to place
		private void MessagePlaceBet()
		{
			Message = Messages.Instance.GetMessage(_START_BET);
		}

		public int CurrentBalance
		{
			get => _game.CurrentBalance;
			set => this.RaiseAndSetIfChanged(ref _currentBalance, value);
		}

		// houses the inputed value for fund deposit
		public string InputFundsDeposit
		{
			get => _inputFundsDeposit;
			set => this.RaiseAndSetIfChanged(ref _inputFundsDeposit, value);
		}


		// Changes if founds can be added or not ----------------------!! ! Dont know how yet though...
		public bool IsfoundsHigherThen100
		{
			get => _isBetPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isBetPanelVisible, value);
		}


		// Registers deposit to funds
		public void AddFundsDeposit()
		{
			// Converts string to int and checks if input is between 100 and 5000
			if (int.TryParse(_inputFundsDeposit, out int depositAmount) && _game.SetAndCheckDeposit(depositAmount))
			{
				CurrentBalance = depositAmount;
				IsFundPanelVisible = false;
				IsBetPanelVisible = true;
				MessagePlaceBet();
			}
			else
			{
				Message = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
			}
		}

		public int CurrentBet
		{
			get => _game.CurrentBet;
			set => this.RaiseAndSetIfChanged(ref _currentBalance, value);
		}

		// houses the inputed value for fund deposit
		public string InputBet
		{
			get => _inputBet;
			set => this.RaiseAndSetIfChanged(ref _inputBet, value);
		}



		// Registers deposit to funds
		public void RegisterBet()
		{
			// Converts string to int and checks if bet exceed funds
			if (int.TryParse(_inputBet, out int betAmount) && _game.SetAndCheckBet(betAmount))
			{
				CurrentBet = betAmount;
			}
			else
			{
				Message = Messages.Instance.GetMessage(_BET_BALANCE_ERROR);
			}
		}
	}
}
