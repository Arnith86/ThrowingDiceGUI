using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ThrowingDiceGUI.Models
{
	class Gamelogic
	{
		//ConsoleMessages consoleMessages;

		//static string _WELCOME = "Welcome";
		//static string _START_DEPOSIT = "Start_Deposit";
		//static string _DEPOSIT_ERROR = "Deposit_Error";
		//static string _CURRENT_BALANCE = "Current_Balance";
		//static string _START_BET = "Start_Bet";
		//static string _BET_ERROR_INT = "Bet_Error_Int";
		//static string _CURRENT_BET = "Current_Bet";
		//static string _BET_BALANCE_ERROR = "Bet_Balance_Error";
		//static string _NEW_THROW = "New_Throw"; 
		//static string _SHOW_DIE = "Show_Die";
		//static string _PLAYER_ROUND_WIN = "Player_Round_Win";
		//static string _NPC_ROUND_WIN = "Npc_Round_Win";
		//static string _PLAYER_GAME_WIN = "Player_Game_Win";
		//static string _NPC_GAME_WIN = "Npc_Game_Win";
		//static string _BUTTON_PRESS = "Button_Press";
		//static string _CONTINUE = "Continue";
		//static string _FOUNDS_ADDED = "Founds_Added";
		//static string _YES_NO_ERROR = "Yes_No_Error";
		//static string _END_GAME = "End_Game";


		private Player player;

		private Dice[] playerDice;
		private Dice[] npcDice;

		private bool betRegistered; 

		// This method will handel all game logic 
		public Gamelogic() 
		{
			//consoleMessages = ConsoleMessages.Instance;
			
			// Regex pattern, only positive integers
			//string integerPattern = @"^\d+$";
			//string oneTwoPattern = @"^[1-2]+$";
			//string oneTwoThreePattern = @"^[1-3]+$";

			Dictionary<int, int> betValues = new Dictionary<int, int>
			{
				{1, 100},
				{2, 300},
				{3, 500}
			};

			// Index 1: player, Index 2: Npc
			int[] roundWinCount = new int[] { 0, 0 };

			player = new Player();

			// Player and Npc dice hands
			playerDice = new Dice[2];
			playerDice[0] = new Dice();
			playerDice[1] = new Dice();
			npcDice = new Dice[2];
			npcDice[0] = new Dice();
			npcDice[1] = new Dice();		
			
			betRegistered = false; 

			// Displays welcome message 
			//consoleMessages.DisplayMessage(_WELCOME);

		//	while (true) 
		//	{
		//		// Empties the console on new game
		//		Console.Clear();
				
		//		// Shows current balance, on program start = 0, otherwise current balace.
		//		consoleMessages.DisplayMessage(_CURRENT_BALANCE, player.GetDeposit());

		//		// Asks how much player wants to place in account
		//		if (player.GetDeposit() == 0) consoleMessages.DisplayMessage(_START_DEPOSIT);

		//		// Regesters player deposit sum
		//		while (player.GetDeposit() == 0) 
		//		{
		//			string inputedDeposit = Console.ReadLine();

		//			// Only Integer values between 100 and 5000 is allowed
		//			if (!Regex.IsMatch(inputedDeposit, integerPattern) ||
		//				!int.TryParse(inputedDeposit, out int tempDepositValue) ||
		//				tempDepositValue < 100 || tempDepositValue > 5000)
		//			{
		//				consoleMessages.DisplayMessage(_DEPOSIT_ERROR);
		//			}
		//			else
		//			{
		//				player.SetDeposit(tempDepositValue);
		//			}
		//		}


		//		// Displays current balance to player
		//		Console.Clear();
		//		consoleMessages.DisplayMessage(_CURRENT_BALANCE, player.GetDeposit());

		//		// Register player bet
		//		consoleMessages.DisplayMessage(_START_BET);

		//		while (!betRegistered)
		//		{
		//			// Get player bet
		//			string inputBet = Console.ReadLine();

		//			// Bet error handeling
		//			// Integer outside of range
		//			if (!Regex.IsMatch(inputBet, oneTwoThreePattern))
		//			{
		//				consoleMessages.DisplayMessage(_BET_ERROR_INT);
		//				continue;
		//			}

		//			int tempBetValue = betValues[int.Parse(inputBet)];
					
		//			// Bet exceeds current founds 
		//			if (tempBetValue > player.GetDeposit())
		//			{
		//				consoleMessages.DisplayMessage(_BET_BALANCE_ERROR);
		//				continue;
		//			}

		//			player.SetBet(tempBetValue);
		//			player.SetDeposit(player.GetDeposit() - tempBetValue);

		//			betRegistered = true;
					
		//			// Displays current balance and bet to player
		//			Console.Clear();
		//			consoleMessages.DisplayMessage(_CURRENT_BALANCE, player.GetDeposit());
		//			consoleMessages.DisplayMessage(_CURRENT_BET, player.GetBet());
					

		//		}

		//		// Will conduct new rounds untill either Player or Npc has two wins
		//		do
		//		{
		//			// Registers winner of round 
		//			if (GameRound(playerDice, npcDice)) roundWinCount[0]++;
		//			else roundWinCount[1]++;

		//			consoleMessages.DisplayGameStats(roundWinCount);
					
		//			// If game has not been won, force player to press button to proceed to next throw
		//			// On press current account balance and chosen bet is displayed 
		//			if (roundWinCount[0] != 2 && roundWinCount[1] != 2)
		//			{
		//				consoleMessages.DisplayMessage(_BUTTON_PRESS);
		//				Console.ReadKey();

		//				Console.Clear();
		//				consoleMessages.DisplayMessage(_CURRENT_BALANCE, player.GetDeposit());
		//				consoleMessages.DisplayMessage(_CURRENT_BET, player.GetBet());
		//			}
		//			else
		//			{
		//				// Player won current game
		//				if (roundWinCount[0] == 2)
		//				{
		//					consoleMessages.DisplayMessage(_PLAYER_GAME_WIN);
							
		//					// Calculate and set the new account balance baseed on current bet 
		//					int foundsAdded = player.GetBet() * 2;
		//					player.SetDeposit(player.GetDeposit() + foundsAdded);

		//					// Displays current balance and bet to player
		//					consoleMessages.DisplayMessage(_FOUNDS_ADDED, foundsAdded);
		//					consoleMessages.DisplayMessage(_CURRENT_BALANCE, player.GetDeposit());
							
		//				}
		//				// Npc won the game
		//				else
		//				{
		//					consoleMessages.DisplayMessage(_NPC_GAME_WIN);
		//				}

		//				// Player given the choice to continue or quit
		//				consoleMessages.DisplayMessage(_CONTINUE);
		//			} 
					
		//			// Enables regestering of a new bet 
		//			betRegistered = false;

		//		} while (!(roundWinCount[0] == 2) && !(roundWinCount[1] == 2));
						
				
		//		bool yesNoRegistered = false;
		//		string yesNo = "";

		//		// End game choice registerd end error handeling
		//		while (!yesNoRegistered) 
		//		{
		//			// Read player continue choice input
		//			yesNo = Console.ReadLine();

		//			if (!Regex.IsMatch(yesNo, oneTwoPattern))
		//			{
		//				consoleMessages.DisplayMessage(_YES_NO_ERROR);
		//				continue;
		//			}
					
		//			yesNoRegistered = true;
		//		}
				
		//		roundWinCount[0] = 0;
		//		roundWinCount[1] = 0;


		//		// yes (1) was chosen a new game loop is started
		//		// no (2) breaks out of the loop and exits the game.
		//		if (yesNo == "2") 
		//		{
		//			consoleMessages.DisplayMessage(_END_GAME);
		//			break; 
		//		}

		//	}
		//}


		//// Handles a single round of dice throws 
		//private bool GameRound(Dice[] playerDice, Dice[] npcDice)
		//{
		//	int playerHighest = 0;
		//	int npcHighest = 0; 

		//	while (playerHighest == npcHighest) 
		//	{
		//		ThrowDiceSet(playerDice);
		//		ThrowDiceSet(npcDice);

		//		playerDice = SorByDescending(playerDice);
		//		npcDice = SorByDescending(npcDice);

		//		// Present results
		//		consoleMessages.DisplayMessage(_SHOW_DIE);
		//		consoleMessages.DisplayDieResults(playerDice, npcDice);

		//		playerHighest = playerDice[0].DiceValue;
		//		npcHighest = npcDice[0].DiceValue;

		//		// Handles instances where highest dice or all pairs are equal
		//		if (playerDice[0].DiceValue == npcDice[0].DiceValue && 
		//			playerDice[1].DiceValue == npcDice[1].DiceValue) 
		//		{
		//			// Both pair of dice are equal, perform a new throw
		//			consoleMessages.DisplayMessage(_NEW_THROW);
		//			// wait for buttonpress 
		//			Console.ReadKey();
		//			continue;
					
		//		}

		//		// Highest valued dice of both players equal, use the other die
		//		else if (playerDice[0].DiceValue == npcDice[0].DiceValue) 
		//		{
		//			playerHighest = playerDice[1].DiceValue;
		//			npcHighest = npcDice[1].DiceValue;
		//		}

		//		// Player won
		//		if (playerHighest > npcHighest) 
		//		{ 
		//			consoleMessages.DisplayMessage(_PLAYER_ROUND_WIN); 
		//			return true; 
		//		}
		//	}

		//	// Npc won
		//	consoleMessages.DisplayMessage(_NPC_ROUND_WIN);
		//	return false;
		}

		// Sort the array with highest valued element first 
		private Dice[] SorByDescending(Dice[] dices)
		{
			return dices.OrderByDescending(n => n.DiceValue).ToArray();
		}

		// Throws both dices 
		private Dice[] ThrowDiceSet(Dice[] dices)
		{
			foreach (Dice dice in dices)
			{
				dice.ThrowDice();
			}

			return dices;
		}
	}
}
