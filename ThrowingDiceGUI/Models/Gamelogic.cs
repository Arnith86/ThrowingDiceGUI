using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Tasks;


namespace ThrowingDiceGUI.Models
{
	public class GameLogic
	{
		private static string _WELCOME = "Welcome";
		private static string _ASK_FOR_DEPOSIT = "Ask_For_Deposit";
		private static string _FOUNDS_ADDED = "Founds_Added";
		private static string _ASK_FOR_BET = "Ask_For_Bet";
		private static string _BET_CHOSEN = "Bet_Chosen";
		private static string _BET_SET = "Bet_Set";
		private static string _THROW_DIE = "Throw_Die";
		private static string _NEW_THROW = "New_Throw";
		private static string _PLAYER_ROUND_WIN = "Player_Round_Win";
		private static string _NPC_ROUND_WIN = "Npc_Round_Win";
		private static string _PLAYER_GAME_WIN = "Player_Game_Win";
		private static string _NPC_GAME_WIN = "Npc_Game_Win";

		private Dice[] _playerDice;
		private Dice[] _npcDice;
		private Dice[] _gameDice;

		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<Dice[]> _gameDiceSubject = new BehaviorSubject<Dice[]>(Array.Empty<Dice>());

		/**
		 * Object housing the current game state
		 * int CurrentFunds				// The current funds.
		 * int Bet						// The current bet. 
		 * int PlayerScore				// Player Current game round wins
		 * int NpcScore					// Npc Current game round wins
		 * bool IsWaitingOnDeposit		// is TRUE, when this class is waiting for deposit input from ViewModel, otherwise FALSE.
		 * bool FundsAreSet				// is TRUE, After ViewModel provides fund input and until game is over. Is FALSE when funds < 100. 
		 * bool IsBetLockedIn			// is TRUE, after the first throw of gameround, FALSE when either perticipant reach two won game rounds.
		 * bool IsAwaitingThrow			// is TRUE, when FundsAreSet is true, otherwise FALSE.
		 * bool IsGameStarted			// is TRUE, when After "New Game" button has been clicked, until funds < 100, then FALSE.
		 * bool GameIsInCompleteState	// is TRUE, when waiting for bet, FALSE, when either perticipant reach 2 wins.
		 * string MessageValue			// Message to present for the player.
		 **/
		private readonly BehaviorSubject<GameState> _gameStateSubject = new BehaviorSubject<GameState>(new GameState());

		// Filed to hold the pending deposit input
		private TaskCompletionSource<int> _depositTcs;
		
		// This method will handel all game logic 
		public GameLogic()
		{
			// Displays welcome message on application start
			UpdateGameState( state =>
			{
				state.MessageValue = Messages.Instance.GetMessage(_WELCOME);
				state.IsBetLockedIn = false;
				state.FundsAreSet = false;
			});

			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_gameDice = new Dice[] { _playerDice[0], _playerDice[1], _npcDice[0], _npcDice[1] };

			_gameDiceSubject = new BehaviorSubject<Dice[]>(_gameDice);
		}

		
		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<Dice[]> GameDiceObservable => _gameDiceSubject.AsObservable();
		public IObservable<GameState> GameStateObservable => _gameStateSubject.AsObservable();

		public void ClearSecondaryMessage() 
		{
			UpdateGameState(state => state.SecondaryMessageValue = string.Empty);
		}

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
		
		public void UpdateBet(int amount)
		{
			UpdateGameState( state => state.Bet = amount);
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

		// A New Game is started, Funds < 100 
		public void StartNewGame()
		{
			UpdateGameState( state =>
			{
				state.IsGameStarted = true;
				state.PlayerScore = 0; 
				state.NpcScore = 0;
				state.Bet = 0;
			});
			AskForDeposit();
		}

		// Next Round is started, Funds >= 100
		public void NextRound()
		{
			UpdateGameState(state =>
			{
				state.PlayerScore = 0;
				state.NpcScore = 0;
				state.Bet = 0;
			});
			
			AskForBet();
		}


		// Async method that ask for deposit to funds, waits for input, then updates current funds
		public async Task AskForDeposit()
		{
			UpdateGameState( state => 
			{
				state.MessageValue = Messages.Instance.GetMessage(_ASK_FOR_DEPOSIT);
				state.IsWaitingOnDeposit = true;
			});

			// Creates a new TaskCompletionSource for deposit input 
			_depositTcs = new TaskCompletionSource<int>();

			// Asynchronoisly wait until the deposit is provided
			int deposit = await _depositTcs.Task;

			// Updates funds and ends fund deposit sequence
			UpdateGameState( state =>
			{
				state.CurrentFunds = deposit;
				state.IsWaitingOnDeposit = false;
				state.FundsAreSet = true;
				state.SecondaryMessageValue = Messages.Instance.GetSecondaryMessageValue(_FOUNDS_ADDED, deposit);
			});
		}

		// this method is called by the viewmodel when deposit input is ready
		// Completes the TasKCompletionSource so AskForDepositAsync can continue 
		public void SetIncomingDeposit(int deposit)
		{
			_depositTcs?.TrySetResult(deposit);
		}

		// Ask which bet to place
		public async void AskForBet()
		{
			UpdateGameState(state =>
			{
				state.IsBetLockedIn = false;
				state.GameIsInCompleteState = false;
				state.MessageValue = Messages.Instance.GetMessage(_ASK_FOR_BET);		
			});
		}

		// After successfull registration of bet, sets up for next stage of game
		public void ABetIsChosen()
		{
			UpdateGameState(state => 
			{
				state.SecondaryMessageValue = Messages.Instance.GetSecondaryMessageValue(_BET_CHOSEN, state.Bet);
				state.MessageValue = Messages.Instance.GetMessage(_THROW_DIE);
				state.IsAwaitingThrow = true;
			});
		}

		// Bet can no longer be changed 
		private void BetIsRegistered()
		{
			if (!_gameStateSubject.Value.IsBetLockedIn)
			{
				UpdateGameState( state => 
				{
					state.IsBetLockedIn = true;
					state.CurrentFunds = ( state.CurrentFunds - state.Bet );
					state.SecondaryMessageValue = Messages.Instance.GetSecondaryMessageValue(_BET_SET, state.Bet);
				});
			}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		public void StartRound()
		{
			// Bet gets registered and is non-refundable 
			BetIsRegistered();
			
			UpdateGameState( state =>
			{
				// A Throw has been performed, and the game round is in an active state
				state.IsAwaitingThrow = false;
				
				ThrowDiceSet(_playerDice);
				ThrowDiceSet(_npcDice);
			
				_playerDice = SorByDescending(_playerDice);
				_npcDice = SorByDescending(_npcDice);

				UpdateGameDice();
							
				// Both player and npc dice are equal, a new throw will be conducted 
				if (CheckIdenticalDiceSet(_playerDice, _npcDice))
				{
					state.MessageValue = Messages.Instance.GetMessage(_NEW_THROW);
				}
				// Who won this round?
				else if (RoundEvaluation(_playerDice, _npcDice))
				{
					state.MessageValue = Messages.Instance.GetMessage(_PLAYER_ROUND_WIN);
					//state.SecondaryMessageValue = Messages.Instance.GetSecondaryMessage(//////////////////// MESSAGE HERE);
					state.PlayerScore++;
				}
				else
				{
					state.MessageValue = Messages.Instance.GetMessage(_NPC_ROUND_WIN);
					state.NpcScore++;
				}


				// Has winner been decided?
				if (state.PlayerScore != 2 && state.NpcScore != 2)
				{
					state.IsAwaitingThrow = true;
				}
				else
				{
					//CurrentGameEnded(_playerScore); 
					CurrentGameEnded(state.PlayerScore);
				}
			});
		}

		// Player or NPC reached 2 wins cuurent game complete
		private void CurrentGameEnded(int playerScore)
		{
			string winnerMessage = "";

			UpdateGameState(state =>
			{
				state.IsBetLockedIn = false;

				// If playerscore == 2, display player win, else npc win
				winnerMessage = playerScore == 2 ? _PLAYER_GAME_WIN : _NPC_GAME_WIN;

				state.MessageValue = Messages.Instance.GetMessage(winnerMessage);

				// Player Wins, update funds
				if (playerScore == 2) 
				{ 
					state.CurrentFunds += (state.Bet * 2);
					state.SecondaryMessageValue = Messages.Instance.GetSecondaryMessageValue(_FOUNDS_ADDED, (state.Bet * 2) );
				}

				state.GameIsInCompleteState = true;


				// Game was lost
				if (state.CurrentFunds < 100)
				{ 
					state.FundsAreSet = false;
					state.IsGameStarted = false;
				}
			});
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
