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
using DynamicData;

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

		//private Player player; // should i keep this? 
		private Dice[] _playerDice;
		private Dice[] _npcDice;
		private Dice[] _gameDice;

		private int _playerScore = 0;
		private int _npcScore = 0;

		// Holds the lates values of balance funds and bet
		private int _currentFundsValue = 0;
		private int _betValue = 0;

		private bool _isGameStarted;
		private bool _isGameRoundStarted;
		//private bool _isGameRoundCompleted;
		private bool _isReadyToReceiveBet;
		private bool _isReadyToThrow;

		private string _messageValue = "";

		// Visibility boolean
		/// TODO: THESE ARE TO BE REMOVED does not follow MVVM
		private bool _isNewRoundButtonVisible;
		




		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<Dice[]> _gameDiceSubject = new BehaviorSubject<Dice[]>(Array.Empty<Dice>());
		private BehaviorSubject<int> _playerScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _npcScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _currentFundsSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _betSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<bool> _isGameStartedSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isGameRoundStartedSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isGameRoundEndedSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isReadyToReceivBetSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<bool> _isReadyToThrowSubject = new BehaviorSubject<bool>(false);
		private BehaviorSubject<string> _messageSubject = new BehaviorSubject<string>("");

		/// TODO: THESE ARE TO BE REMOVED does not follow MVVM
		private BehaviorSubject<bool> _isNewRoundButtonVisibleSubject = new BehaviorSubject<bool> (false);
		
		
		// This method will handel all game logic 
		public GameLogic()
		{
			// Displays welcome message on application start
			UpdateMessage(_WELCOME);
			UpdateIsGameRoundStarted(false);
			UpdateIsNewRoundButtonVisible(false);
			
			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_gameDice = new Dice[] { _playerDice[0], _playerDice[1], _npcDice[0], _npcDice[1] };

			_gameDiceSubject = new BehaviorSubject<Dice[]>(_gameDice);
			_playerScoreSubject = new BehaviorSubject<int>(_playerScore);
			_npcScoreSubject = new BehaviorSubject<int>(_npcScore);
			_currentFundsSubject = new BehaviorSubject<int>(_currentFundsValue);
			_betSubject = new BehaviorSubject<int>(_betValue);
			_isGameStartedSubject = new BehaviorSubject<bool>(_isGameStarted);
			_isGameRoundStartedSubject = new BehaviorSubject<bool>(_isGameRoundStarted);
			_isReadyToReceivBetSubject = new BehaviorSubject<bool>(_isReadyToReceiveBet);
			_isReadyToThrowSubject = new BehaviorSubject<bool>(_isReadyToThrow);
			_messageSubject = new BehaviorSubject<string>(_messageValue);


			_isNewRoundButtonVisibleSubject = new BehaviorSubject<bool>(_isNewRoundButtonVisible);
			
		}

		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<Dice[]> GameDiceObservable => _gameDiceSubject.AsObservable();
		public IObservable<int> PlayerScoreObservable => _playerScoreSubject.AsObservable();
		public IObservable<int> NpcScoreObservable => _npcScoreSubject.AsObservable();
		public IObservable<int> CurrentFundsObservable => _currentFundsSubject.AsObservable();
		public IObservable<int> BetObservable => _betSubject.AsObservable();
		public IObservable<bool> IsGameStartedObservable => _isGameStartedSubject.AsObservable();
		public IObservable<bool> IsGameRoundStartedObject => _isGameRoundStartedSubject.AsObservable();
		public IObservable<bool> IsGameRoundEndedObject => _isGameRoundEndedSubject.AsObservable();
		public IObservable<bool> IsReadyToReceiveBetObject => _isReadyToReceivBetSubject.AsObservable();
		public IObservable<bool> IsReadyToThrowObject => _isReadyToThrowSubject.AsObservable();
		public IObservable<string> MessageObservable => _messageSubject.AsObservable();


		public IObservable<bool> IsNewRoundButtonVisibleObservable => _isNewRoundButtonVisibleSubject.AsObservable();
		


		// Updates Values and notify subscibers
		private void UpdateGameDice()
		{
			int indexCounter = 0;

			for (int i = 0; i < 2; i++)
			{
				_gameDice[indexCounter++] = _playerDice[i];	
			}
			for (int j = 0; j < 2; j++)
			{
				_gameDice[indexCounter++] = _npcDice[j];
			}

			_gameDiceSubject.OnNext(_gameDice);
		}

		private void UpdatePlayerScore(int score) 
		 {
			_playerScore = score;
			_playerScoreSubject.OnNext(score);
		}

		private void UpdateNpcScore(int score)
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

		private void UpdateIsGameStarted(bool tf)
		{
			_isGameStarted = tf;
			_isGameStartedSubject.OnNext(tf);
		}

		private void UpdateIsGameRoundStarted(bool tf)
		{
			_isGameRoundStarted = tf;
			_isGameRoundStartedSubject.OnNext(tf);
		}

		private void UpdateIsGameRoundEnded(bool tf)
		{
			_isGameRoundStarted = tf;
			_isGameRoundStartedSubject.OnNext(tf);
		}

		private void UpdateIsReadyToReceiveBet(bool tf)
		{
			_isReadyToReceiveBet = tf;
			_isReadyToReceivBetSubject.OnNext(tf);
		}

		private void UpdateMessage(string message)
		{
			_messageValue = Messages.Instance.GetMessage(message);
			_messageSubject.OnNext(_messageValue);
		}

		private void UpdateIsReadyToThrow(bool tf)
		{
			_isReadyToThrow = tf;
			_isReadyToThrowSubject.OnNext(_isReadyToThrow);
		}

		/// TODO: THESE ARE TO BE REMOVED does not follow MVVM
		private void UpdateIsNewRoundButtonVisible(bool tf)
		{
			_isNewRoundButtonVisible = tf;
			_isNewRoundButtonVisibleSubject.OnNext(_isNewRoundButtonVisible);
		}

		

		

		// If not enough funds, sends to "AskForDeposit" method, otherwise "PlaceBet"
		public void NewRound()
		{
			UpdatePlayerScore(0);
			UpdateNpcScore(0);
			UpdateBet(0);
			UpdateIsGameRoundEnded(false);
			UpdateIsNewRoundButtonVisible(false);// FIX this

			if (_currentFundsValue < 100)
			{
				///TODO: add secondery message stating that funds are insufficiant 
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
			UpdateIsGameStarted(true);
		}

		// Ask which bet to place
		public void AskForPlaceBet()
		{
			UpdateIsReadyToReceiveBet(true);
			UpdateIsGameRoundStarted(false);
			UpdateMessage(_ASK_FOR_BET);
		}

		// After successfull registration of bet, sets up for next stage of game
		public void ABetIsChosen()
		{
			UpdateMessage(_THROW_DIE);
			UpdateIsReadyToThrow(true);
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
			UpdateIsReadyToThrow(false);
			ThrowDiceSet(_playerDice);
			ThrowDiceSet(_npcDice);
			_playerDice = SorByDescending(_playerDice);
			_npcDice = SorByDescending(_npcDice);

			UpdateGameDice();

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

			if (_playerScore != 2 && _npcScore != 2)
			{
				UpdateIsReadyToThrow(true);
			}
			else
			{
				UpdateIsGameRoundStarted(false);
				UpdateIsGameRoundEnded(true);
				CurrentGameEnded(_playerScore); 
			}
		}

		private void CurrentGameEnded(int playerScore)
		{
			if (playerScore == 2) // Player Wins
			{
				UpdateIsReadyToThrow(false);
				UpdateIsNewRoundButtonVisible(true);
				UpdateIsReadyToReceiveBet(false);
				UpdateMessage(_PLAYER_GAME_WIN);
				UpdateFunds(_currentFundsValue + (_betValue * 2));
			}
			else// Npc Wins
			{
				UpdateIsReadyToThrow(false);
				UpdateIsNewRoundButtonVisible(true);
				UpdateIsReadyToReceiveBet(false);
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
