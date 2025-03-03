using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;


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

		
		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<Dice[]> _gameDiceSubject = new BehaviorSubject<Dice[]>(Array.Empty<Dice>());
		private BehaviorSubject<int> _playerScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _npcScoreSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _currentFundsSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _betSubject = new BehaviorSubject<int>(0);

		/**
		 * Object housing the current game state
		 * bool IsGameStarted 
		 * bool IsGameRoundStarted 
		 * bool IsReadyToReceiveBet 
		 * bool IsReadyToThrow 
		 * bool NewRoundCanBeStarted
		 * string MessageValue
		 **/
		private readonly BehaviorSubject<GameState> _gameStateSubject = new BehaviorSubject<GameState>(new GameState());

				
		// This method will handel all game logic 
		public GameLogic()
		{
			// Displays welcome message on application start
			UpdateGameState( state =>
			{
				state.MessageValue = Messages.Instance.GetMessage(_WELCOME);
				state.IsGameRoundStarted = false;
				state.NewRoundCanBeStarted = false;
			});

			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_gameDice = new Dice[] { _playerDice[0], _playerDice[1], _npcDice[0], _npcDice[1] };

			_gameDiceSubject = new BehaviorSubject<Dice[]>(_gameDice);
			_playerScoreSubject = new BehaviorSubject<int>(_playerScore);
			_npcScoreSubject = new BehaviorSubject<int>(_npcScore);
			_currentFundsSubject = new BehaviorSubject<int>(_currentFundsValue);
			_betSubject = new BehaviorSubject<int>(_betValue);
		}

		
		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<Dice[]> GameDiceObservable => _gameDiceSubject.AsObservable();
		public IObservable<int> PlayerScoreObservable => _playerScoreSubject.AsObservable();
		public IObservable<int> NpcScoreObservable => _npcScoreSubject.AsObservable();
		public IObservable<int> CurrentFundsObservable => _currentFundsSubject.AsObservable();
		public IObservable<int> BetObservable => _betSubject.AsObservable();
		public IObservable<GameState> GameStateObservable => _gameStateSubject.AsObservable();

		
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

		
		// Updates GameState and notify subscribers
		// Action<GameState> means "a method that takes a GameState and modifies it, but doesn't return anything.
		private void UpdateGameState(Action<GameState> updateAction)
		{
			// Specifies the object to be updated 
			var newState = _gameStateSubject.Value;
			// Updates the object with the new state
			updateAction(newState);
			// Notify subscribers of change
			_gameStateSubject.OnNext(newState);
		}

		// If not enough funds, sends to "AskForDeposit" method, otherwise "PlaceBet"
		public void NewRound()
		{
			UpdatePlayerScore(0);
			UpdateNpcScore(0);
			UpdateBet(0);
			//UpdateGameState(state => state.NewRoundCanBeStarted = false);

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
			UpdateGameState(state =>
			{
				state.MessageValue = Messages.Instance.GetMessage(_ASK_FOR_DEPOSIT);
				state.IsGameStarted = true;
				state.NewRoundCanBeStarted = false;
			});
		}

		// Ask which bet to place
		public void AskForPlaceBet()
		{
			UpdateGameState(state =>
			{
				state.IsReadyToReceiveBet = true;
				state.IsGameRoundStarted = false;
				state.MessageValue = Messages.Instance.GetMessage(_ASK_FOR_BET);		
			});
		}

		// After successfull registration of bet, sets up for next stage of game
		public void ABetIsChosen()
		{
			UpdateGameState(state => 
			{
				state.MessageValue = Messages.Instance.GetMessage(_THROW_DIE);
				state.IsReadyToThrow = true;
			});
		}

		// Bet can no longer be changed 
		private void BetIsRegistered()
		{
			if (!_gameStateSubject.Value.IsGameRoundStarted)
			{
				UpdateGameState(state => { state.IsGameRoundStarted = true;  });
				UpdateFunds(_currentFundsValue - _betValue);
				_currentFundsSubject.OnNext(_currentFundsValue);
			}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		public void StartRound()
		{
			BetIsRegistered();
			UpdateGameState(state => { state.IsReadyToThrow = false; });
			ThrowDiceSet(_playerDice);
			ThrowDiceSet(_npcDice);
			_playerDice = SorByDescending(_playerDice);
			_npcDice = SorByDescending(_npcDice);

			UpdateGameDice();

			// Both player and npc dice are equal, a new throw will be conducted 
			if (CheckIdenticalDiceSet(_playerDice, _npcDice))
			{
				UpdateGameState(state => { state.MessageValue = Messages.Instance.GetMessage(_NEW_THROW);  });
			}
			else if (RoundEvaluation(_playerDice, _npcDice))
			{
				UpdateGameState(state => { state.MessageValue = Messages.Instance.GetMessage(_PLAYER_ROUND_WIN); });
				UpdatePlayerScore( ++_playerScore );
			}
			else
			{
				UpdateGameState(state => { state.MessageValue = Messages.Instance.GetMessage(_NPC_ROUND_WIN); });
				UpdateNpcScore( ++_npcScore );
			}

			if (_playerScore != 2 && _npcScore != 2)
			{
				UpdateGameState(state => { state.IsReadyToThrow = true; });
			}
			else
			{
				UpdateGameState(state => { state.IsGameRoundStarted = false; });
				CurrentGameEnded(_playerScore); 
			}
		}

		private void CurrentGameEnded(int playerScore)
		{
			UpdateGameState(state => 
			{
				// If not enough funds are available a new round cannot be started
				state.NewRoundCanBeStarted = _currentFundsSubject.Value < 100 ? false : true;

				state.IsReadyToThrow = false;
				state.IsReadyToReceiveBet = false;
				
				string winnerMessage = "";

				// If playerscore == 2, display player win, else npc win
				winnerMessage = playerScore == 2 ? _PLAYER_GAME_WIN : _NPC_GAME_WIN;
				
				state.MessageValue = Messages.Instance.GetMessage(winnerMessage);
			});

			// Player Wins, update funds
			if (playerScore == 2) UpdateFunds(_currentFundsValue + (_betValue * 2));
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
			foreach (Dice dice in dices) dice.ThrowDice();

			return dices;
		}
	}
}
