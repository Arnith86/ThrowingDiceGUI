using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reactive.Subjects;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive;
using ReactiveUI;

namespace ThrowingDiceGUI.Models
{
	public class GameLogic
	{
		private static string _WELCOME = "Welcome";
		private static string _ASK_FOR_DEPOSIT = "Ask_For_Deposit";
		private static string _ASK_FOR_BET = "Ask_For_Bet";
		private static string _THROW_DIE = "Throw_Die";
		private static string _NEW_THROW = "New_Throw";
		private static string _PLAYER_ROUND_WIN = "Player_Round_Win";
		private static string _NPC_ROUND_WIN = "Npc_Round_Win";
		private static string _PLAYER_GAME_WIN = "Player_Game_Win";
		private static string _NPC_GAME_WIN = "Npc_Game_Win";

		private Player player; // should i keep this? 
		private Dice[] _playerDice;
		private Dice[] _npcDice;

		private int _playerScore = 0;
		private int _npcScore = 0;

		// Holds the lates values of balance funds and bet
		private int _currentFundsValue = 0;
		private int _betValue = 0;

		private bool _isGameRoundStarted;

		private string _messageValue = "";




		// Visibility boolean
		/// TODO: THESE ARE TO BE REMOVED does not follow MVVM
		private bool _isStartButtonVisible;
		private bool _isNewRoundButtonVisible;
		private bool _isFundPanelVisible;
		private bool _isBetPanelVisible;
		private bool _isThrowButtonVisible;

	


		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<int> _playerScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _npcScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _currentFundsSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _betSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<bool> _isGameRoundStartedSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<string> _messageSubject = new BehaviorSubject<string>("");

		/// TODO: THESE ARE TO BE REMOVED does not follow MVVM
		private BehaviorSubject<bool> _isStartButtonVisibleSubject = new BehaviorSubject<bool>(true);
		private BehaviorSubject<bool> _isNewRoundButtonVisibleSubject = new BehaviorSubject<bool> (false);
		private BehaviorSubject<bool> _isFundPanelVisibleSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isBetPanelVisibleSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isThrowButtonVisibleSubject = new BehaviorSubject<bool>(false);
		
		private Subject<Unit> _diceThrownSubject = new Subject<Unit>();

		// Getters and Setters 
		//public Dice[] PlayerDice => _playerDice;
		//public Dice[] NpcDice => _npcDice;
		//public int PlayerScore => _playerScore;
		//public int NpcScore => _npcScore;
		//public int CurrentFundsValue => _currentFundsValue;
		//public int BetValue => _betValue;
		// public bool IsGameRoundStarted => _isGameRoundStarted;
		
		//public string MessageValue
		//{
		//	get => _messageValue;
		//	set => _messageValue = value;
		//} 
		//public bool IsStartButtonVisible
		//{
		//	get => _isStartButtonVisible;
		//	set => _isStartButtonVisible = value;
		//}

		//public bool IsNewRoundButtonVisible
		//{
		//	get => _isNewRoundButtonVisible;
		//	set => _isNewRoundButtonVisible = value;
		//}

		//public bool IsFundPanelVisible
		//{
		//	get => _isFundPanelVisible;
		//	set => _isFundPanelVisible = value;
		//}
		//public bool IsBetPanelVisible
		//{
		//	get => _isBetPanelVisible;
		//	set => _isBetPanelVisible = value;
		//}

		//public bool IsThrowButtonVisible
		//{
		//	get => _isThrowButtonVisible;
		//	set => _isThrowButtonVisible = value;
		//}


		// This method will handel all game logic 
		public GameLogic()
		{
			// Displays welcome message on application start
			UpdateMessage(_WELCOME);
			UpdateIsStartButtonVisible(true);
			UpdateIsNewRoundButtonVisible(false);
			

			player = new Player();
			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_playerScoreSubject = new BehaviorSubject<int>(_playerScore);
			_npcScoreSubject = new BehaviorSubject<int>(_npcScore);
			_currentFundsSubject = new BehaviorSubject<int>(_currentFundsValue);
			_betSubject = new BehaviorSubject<int>(_betValue);
			_isGameRoundStartedSubject = new BehaviorSubject<bool>(_isGameRoundStarted);
			_messageSubject = new BehaviorSubject<string>(_messageValue);


			_isStartButtonVisibleSubject = new BehaviorSubject<bool>(_isStartButtonVisible);
			_isNewRoundButtonVisibleSubject = new BehaviorSubject<bool>(_isNewRoundButtonVisible);
			_isFundPanelVisibleSubject = new BehaviorSubject<bool>(_isFundPanelVisible);
			_isBetPanelVisibleSubject = new BehaviorSubject<bool>(_isBetPanelVisible);
			_isThrowButtonVisibleSubject = new BehaviorSubject<bool>(_isThrowButtonVisible);

		
		}

		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<int> PlayerScoreObservable => _playerScoreSubject.AsObservable();
		public IObservable<int> NpcScoreObservable => _npcScoreSubject.AsObservable();
		public IObservable<int> CurrentFundsObservable => _currentFundsSubject.AsObservable();
		public IObservable<int> BetObservable => _betSubject.AsObservable();
		public IObservable<bool> IsGameRoundStartedObject => _isGameRoundStartedSubject.AsObservable();
		public IObservable<string> MessageObservable => _messageSubject.AsObservable();


		public IObservable<bool> IsStartButtonVisibleObservable => _isStartButtonVisibleSubject.AsObservable();
		public IObservable<bool> IsNewRoundButtonVisibleObservable => _isNewRoundButtonVisibleSubject.AsObservable();
		public IObservable<bool> IsFundPanelVisíbleObservable => _isFundPanelVisibleSubject.AsObservable();
		public IObservable<bool> IsBetPanelVisibleObject => _isBetPanelVisibleSubject.AsObservable();
		public IObservable<bool> IsThrowButtonVisibleObject => _isThrowButtonVisibleSubject.AsObservable();
		public IObservable<Unit> DiceThrownObservable => _diceThrownSubject.AsObservable();


		// Updates Values and notify subscibers 	

		public void UpdatePlayerScore(int score) 
		{
			_playerScore = score;
			_playerScoreSubject.OnNext(score);
		}

		public void UpdateNpcScore(int score)
		{
			_npcScore = score;
			_npcScoreSubject.OnNext(score);
		}

		public void UpdateFunds(int amount)
		{
			_currentFundsValue = amount;
			_currentFundsSubject.OnNext(amount);
		}

		public void UpdateBet(int amount)
		{
			_betValue = amount;
			_betSubject.OnNext(amount);
		}

		public void UpdateIsGameRoundStarted(bool tf)
		{
			_isGameRoundStarted = tf;
			_isGameRoundStartedSubject.OnNext(tf);
		}

		public void UpdateMessage(string message)
		{
			_messageValue = Messages.Instance.GetMessage(message);
			_messageSubject.OnNext(_messageValue);
		}

		public void UpdateIsStartButtonVisible(bool tf)
		{
			_isStartButtonVisible = tf;
			_isStartButtonVisibleSubject.OnNext(_isStartButtonVisible);
		}

		public void UpdateIsNewRoundButtonVisible(bool tf)
		{
			_isNewRoundButtonVisible = tf;
			_isNewRoundButtonVisibleSubject.OnNext(_isNewRoundButtonVisible);
		}

		public void UpdateIsFundPanelVisible(bool tf)
		{
			_isFundPanelVisible = tf;
			_isFundPanelVisibleSubject.OnNext(_isFundPanelVisible);
		}

		public void UpdateIsBetPanelVisible(bool tf)
		{
			_isBetPanelVisible = tf;
			_isBetPanelVisibleSubject.OnNext(_isBetPanelVisible);
		}

		public void UpdateIsThrowButtonVisible(bool tf)
		{
			_isThrowButtonVisible = tf;
			_isThrowButtonVisibleSubject.OnNext(_isThrowButtonVisible);
		}

		

		// If not enough funds, sends to "AskForDeposit" method, otherwise "PlaceBet"
		public void NewRound()
		{
			UpdatePlayerScore(0);
			UpdateNpcScore(0);
			UpdateBet(0);
			UpdateIsNewRoundButtonVisible(false);

			if (_currentFundsValue < 100)
			{
				AskForDeposit();
			}
			else
			{
				AskForPlaceBet();
			}
		}

		// Starts the game
		// Ask for deposit to funds
		public void AskForDeposit()
		{
			UpdateMessage(_ASK_FOR_DEPOSIT);
			UpdateIsStartButtonVisible(false);
			UpdateIsFundPanelVisible(true);
		}

		// Ask which bet to place
		public void AskForPlaceBet()
		{
			UpdateIsBetPanelVisible(true);
			UpdateIsGameRoundStarted(false);
			UpdateMessage(_ASK_FOR_BET);
		}

		// After successfull registration of bet, sets up for next stage of game
		public void ABetIsChosen()
		{
			UpdateMessage(_THROW_DIE);
			UpdateIsThrowButtonVisible(true);
		}

		private void BetIsRegistered()
		{
			if (!_isGameRoundStarted)
			{
				UpdateIsGameRoundStarted(true);// when true, bet buttons are disabled
				UpdateFunds(_currentFundsValue - _betValue);
				_currentFundsSubject.OnNext(_currentFundsValue);
			}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		public void StartRound()
		{
			BetIsRegistered();
			UpdateIsThrowButtonVisible(false);
			ThrowDiceSet(_playerDice);
			ThrowDiceSet(_npcDice);
			_playerDice = SorByDescending(_playerDice);
			_npcDice = SorByDescending(_npcDice);

			// Notification that "something" has happend. In this case dice have been thrown
			_diceThrownSubject.OnNext(Unit.Default);

			// Both player and npc dice are equal, a new throw will be conducted 
			if (CheckIdenticalDiceSet(_playerDice, _npcDice))
			{
				UpdateMessage(_NEW_THROW);
			}
			else if (RoundEvaluation(_playerDice, _npcDice))
			{
				UpdateMessage(_PLAYER_ROUND_WIN);
				UpdatePlayerScore( ++_playerScore );
			}
			else
			{
				UpdateMessage(_NPC_ROUND_WIN);
				UpdateNpcScore( ++_npcScore );
			}

			if (_playerScore != 2 && _npcScore != 2) UpdateIsThrowButtonVisible(true);
			else CurrentGameEnded(_playerScore);
		}

		private void CurrentGameEnded(int playerScore)
		{
			if (playerScore == 2) // Player Wins
			{
				UpdateIsThrowButtonVisible(false);
				UpdateIsNewRoundButtonVisible(true);
				UpdateIsBetPanelVisible(false);
				UpdateMessage(_PLAYER_GAME_WIN);
				UpdateFunds(_currentFundsValue + (_betValue * 2));
			}
			else// Npc Wins
			{
				UpdateIsThrowButtonVisible(false);
				UpdateIsNewRoundButtonVisible(true);
				UpdateIsBetPanelVisible(false);
				UpdateMessage(_NPC_GAME_WIN);
			}
		}
		
		// Handles a single round of dice throws 
		public bool RoundEvaluation(Dice[] playerDice, Dice[] npcDice)
		{
			int playerHighest = 0;
			int npcHighest = 0;
						
			playerHighest = playerDice[0].DiceValue;
			npcHighest = npcDice[0].DiceValue;
							
			// Highest valued dice of both players equal, use the other die
			if (playerDice[0].DiceValue == npcDice[0].DiceValue)
			{
				playerHighest = playerDice[1].DiceValue;
				npcHighest = npcDice[1].DiceValue;
			}
		
			// Has player won?
			return playerHighest > npcHighest;
		}

		// Check if dice set are identical (they must be sorted first)
		public bool CheckIdenticalDiceSet(Dice[] playerDice, Dice[] npcDice)
		{
			return (playerDice[0].DiceValue == npcDice[0].DiceValue && playerDice[1].DiceValue == npcDice[1].DiceValue);	
		}

		// Sort the array with highest valued element first 
		public Dice[] SorByDescending(Dice[] dices)
		{
			return dices.OrderByDescending(n => n.DiceValue).ToArray();
		}

		// Throws both dices 
		public Dice[] ThrowDiceSet(Dice[] dices)
		{
			foreach (Dice dice in dices)
			{
				dice.ThrowDice();
			}

			return dices;
		}
	}
}
