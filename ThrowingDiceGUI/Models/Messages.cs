using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThrowingDiceGUI.Models
{
	/** 
	 * This class contains all the console messages used in the program 
	 **/
	public class Messages
	{
		private static Messages instance;
		Dictionary<string, string> messages;
		private string lastMessage = "";

		private Messages() 
		{
			messages = new Dictionary<string, string>
			{
				{ "Welcome", "Welcome! \n\nThe game involves the player and the computer rolling two dice at a time for three rounds. \n\n" +
								"The player who rolls the highest individual die in a round wins that round. \n\n" +
								"The best out of three rounds determines the winner.\n" },
				{ "Start_Deposit", "How much money do you want do deposit? \nRegister a value between 100 - 5000kr.\n" },
				{ "Deposit_Error", "Only integer values between 100 and 5000 are permited! \nTry again!\n" },
				{ "Current_Balance", "kr in account! \n" },
				{ "Start_Bet", "How much will you bet? \n 1: 100kr \n 2: 300kr \n 3: 500kr" },
				{ "Bet_Error_Int", "Provide an integer between 1 and 3!" },
				{ "Bet_Balance_Error", "Bet exceeds your current funds. Please try again!" },
				{ "Current_Bet", "kr bet!" },
				{ "Show_Die", "\nResults of throw:" },
				{ "New_Throw", "This round ended in a draw. Press a button to throw the dices!\n" },
				{ "Button_Press", "Press a button to throw the dices!\n" },
				{ "Player_Round_Win", "You won this round!\n" },
				{ "Npc_Round_Win", "You lost this round!\n" },
				{ "Player_Game_Win", "You won this game!\n" },
				{ "Npc_Game_Win", "Sadly, you lost this game..!\n" },
				{ "Continue", "Do you wish to continue the game?\n1: Yes, 2: No" },
				{ "Founds_Added", "kr Added to your account!\n" },
				{ "Yes_No_Error", "Provide an integer between 1 and 2!\n" },
				{ "End_Game", "Thank you for playing!\n" }
			};
		}


		// creates the singleton instance 
		public static Messages Instance 
		{ 
			get 
			{
				if (instance == null)
				{
					instance = new Messages();
				}
				return instance; 
			} 
		}
				
		// Read-Only Property (only a getter, No set Method)
		public string LastMessage => lastMessage;

		// Retrives the desired message, if it exists.
		public string GetMessage(string key, int value = -1)
		{
			if (messages.TryGetValue(key, out string message))
			{
				lastMessage = value == -1 ? message : $"{value} {message}";
				return lastMessage;
			}
			return $"Message for key '{key}' not found.";
		}


		// Retrives the desired message, if it exists.
		//public string GetMessage(string key)
		//{
		//	if (messages.TryGetValue(key, out string message))
		//	{
		//		return message;
		//	}

		//	return $"Message for key '{key}' not found.";
		//}

		//public void DisplayMessage(string key, int value = -1)
		//{
		//	if (value == -1)
		//	{
		//		Console.WriteLine(GetMessage(key));
		//	}
		//	else
		//	{
		//		Console.WriteLine(value + GetMessage(key));
		//	}
		//}

	}
}
