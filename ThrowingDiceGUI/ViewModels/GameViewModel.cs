using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

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
		static string _THROW_DIE = "Throw_Die";
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
		private bool _isThrowButtonVisible;
		private bool _isNewRoundButtonVisible;
		private bool _isRoundStarted;
		private bool _betButtonsEnabled;
		private int _currentBalance;
		private string? _inputFundsDeposit;
		private int _currentBet;
		private string _inputBet;
		private int _playerScore;
		private int _npcScore;
		private Bitmap _playerDiceImage1;
		private Bitmap _playerDiceImage2;
		private Bitmap _npcDiceImage1;
		private Bitmap _npcDiceImage2;
		




		public GameViewModel()
		{	
			// Sets the initial settings
			_message = Messages.Instance.GetMessage(_WELCOME);

			IsStartButtonVisible = true;
			IsInputPanelVisible = false;
			IsBetPanelVisible = false;
			IsFundPanelVisible = false;
			IsThrowButtonVisible = false;
			IsNewRoundButtonVisible = false;
			BetButtonsEnabled = true; 

			_game = new Gamelogic();

			// Display initial values
			updateDiceImages();

			StartGameCommand = ReactiveCommand.Create(StartGameMessageAskDeposit);
			
			InputBetCommand = ReactiveCommand.Create<string>( bet => 
			{
				InputBet = bet;
				RegisterBet();
			});

			ThrowCommand = ReactiveCommand.Create(StartRound);

			// Subscribes to current results, when player or npc reach 2 wins game ends.
			this.WhenAnyValue(GameViewModel => GameViewModel.PlayerScore, GameViewModel => GameViewModel.NpcScore).
			Subscribe(scores =>
			{
				if (scores.Item1 == 2) // Player Wins
				{
					IsThrowButtonVisible = false;
					IsNewRoundButtonVisible = true;
					IsBetPanelVisible = false;
					Message = Messages.Instance.GetMessage(_PLAYER_GAME_WIN);
					_game.CurrentBalance = +(CurrentBet * 2);
				}
				else if (scores.Item2 == 2) // Npc Wins
				{
					IsThrowButtonVisible = false;
					IsNewRoundButtonVisible = true;
					IsBetPanelVisible = false;
					Message = Messages.Instance.GetMessage(_NPC_GAME_WIN);
				}
			});

			// Disables the bet buttons after first throw of new game 
			this.WhenAnyValue(GameViewModel => GameViewModel.IsRoundStarted).Subscribe(
				isRoundStarted =>
				{
					if (isRoundStarted)
					{
						BetButtonsEnabled = false;
					}
				}
			);

		}

		public ReactiveCommand<Unit, Unit> StartGameCommand { get; }    // Start Game button has been pressed
		public ReactiveCommand<string, Unit> InputBetCommand { get; }   
		public ReactiveCommand<Unit, Unit> ThrowCommand { get; }

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

		// Disables the ability to bet when a game is started 
		public bool BetButtonsEnabled
		{
			get => _betButtonsEnabled;
			set => this.RaiseAndSetIfChanged(ref _betButtonsEnabled, value);
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
			get => _currentBalance;
			set => this.RaiseAndSetIfChanged(ref _currentBalance, value);
		}

		// houses the inputed value for fund deposit
		public string InputFundsDeposit
		{
			get => _inputFundsDeposit;
			set => this.RaiseAndSetIfChanged(ref _inputFundsDeposit, value);
		}


		// When a round is started the bet buttons will be disabled
		public bool IsRoundStarted
		{
			get => _isRoundStarted;
			set => this.RaiseAndSetIfChanged(ref _isRoundStarted, value);
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
			get => _currentBet;
			set => this.RaiseAndSetIfChanged(ref _currentBet, value);
		}

		// houses the inputed value for fund deposit
		public string InputBet
		{
			get => _inputBet;
			set => this.RaiseAndSetIfChanged(ref _inputBet, value);
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




		// Registers chosen bet
		public void RegisterBet()
		{
			// Converts string to int and checks if bet exceed funds
			if (int.TryParse(_inputBet, out int betAmount) && _game.SetAndCheckBet(betAmount))
			{
				CurrentBet = betAmount;
				CurrentBalance = _game.CurrentBalance;
				Message = Messages.Instance.GetMessage(_THROW_DIE); 
				IsThrowButtonVisible = true;
			}
			else
			{
				Message = Messages.Instance.GetMessage(_BET_BALANCE_ERROR);
			}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		private void StartRound()
		{
			IsThrowButtonVisible = false;
			IsRoundStarted = true; // when true, bet buttons are disabled
			_game.ThrowDiceSet(_game.PlayerDice);
			_game.ThrowDiceSet(_game.NpcDice);
			_game.PlayerDice = _game.SorByDescending(_game.PlayerDice);
			_game.NpcDice = _game.SorByDescending(_game.NpcDice);
			
			updateDiceImages();

			// Both player and npc dice are equal, a new throw will be conducted 
			if (_game.CheckIdenticalDiceSet(_game.PlayerDice, _game.NpcDice))
			{
				Message = Messages.Instance.GetMessage(_NEW_THROW);
			}
			else if (_game.RoundEvaluation(_game.PlayerDice, _game.NpcDice))
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
			PlayerDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_game.PlayerDice[0].DiceImagePath)));
			PlayerDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_game.PlayerDice[1].DiceImagePath)));

			NpcDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_game.NpcDice[0].DiceImagePath)));
			NpcDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_game.NpcDice[1].DiceImagePath)));
		}

		private void ResetGameRound()
		{
			PlayerScore = 0;
			NpcScore = 0;
			CurrentBet = 0;
		}
	}
}
