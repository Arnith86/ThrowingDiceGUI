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
		private Player player;
		private Dice[] playerDice;
		private Dice[] npcDice;
		private Dictionary<int, int> betValues;
		private int[] roundWinCount;


		public int CurrentBalance => player.Deposit;
		public int CurrentBet => player.Bet;
		public int PlayerScore => roundWinCount[0];
		public int NpcScore => roundWinCount[1];
		private bool betRegistered; // still needed? 

		// This method will handel all game logic 
		public Gamelogic()
		{
			// Regex pattern, only positive integers
			//string integerPattern = @"^\d+$";
			//string oneTwoPattern = @"^[1-2]+$";
			//string oneTwoThreePattern = @"^[1-3]+$";
			player = new Player();
			playerDice = new Dice[] { new Dice(), new Dice() };
			npcDice = new Dice[] { new Dice(), new Dice() };

			betValues = new Dictionary<int, int>
			{
				{1, 100},{2, 300},{3, 500}
			};

			// Index 1: player, Index 2: Npc
			roundWinCount = new int[] { 0, 0 };
		}

		public bool setDeposit(int amount)
		{
			if (amount < 100 || amount > 5000) return false; 
			
			player.Deposit = amount;
			return true;
		}

		public bool setBet(int amount)
		{
			if (amount < 0 || amount > 3) return false;
			
			player.Bet = amount;
			return true;
		}

		// betRegistered = false; 

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


		// Handles a single round of dice throws 
		private bool GameRound(Dice[] playerDice, Dice[] npcDice)
		{
			int playerHighest = 0;
			int npcHighest = 0;


			do
			{
				ThrowDiceSet(playerDice);
				ThrowDiceSet(npcDice);

				playerDice = SorByDescending(playerDice);
				npcDice = SorByDescending(npcDice);


				playerHighest = playerDice[0].DiceValue;
				npcHighest = npcDice[0].DiceValue;

				// Handles instances where highest dice or all pairs are equal
				if (playerDice[0].DiceValue == npcDice[0].DiceValue &&
					playerDice[1].DiceValue == npcDice[1].DiceValue)
				{
					// Both pair of dice are equal, perform a new throw 
					// !!!!!!!!!!!!!!!!!! Must be handled with UI ----------	
					// wait for buttonpress ------- HOW? 
					Console.ReadKey();
					continue;

				}

				// Highest valued dice of both players equal, use the other die
				else if (playerDice[0].DiceValue == npcDice[0].DiceValue)
				{
					playerHighest = playerDice[1].DiceValue;
					npcHighest = npcDice[1].DiceValue;
				}
				
			} while (playerHighest == npcHighest);

			// Has player won?
			return playerHighest > npcHighest;
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
