using System.Collections.Generic;


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
								"The best out of three rounds determines the winner." },
				{ "Ask_For_Deposit", "What amount do you want to start with? \nRegister a value between 100 - 5000kr." },
				{ "Deposit_Error", "Only integer values between 100 and 5000 are permited! \nTry again!" },
				{ "Ask_For_Bet", "How much will you bet? \n 100kr, 300kr or 500kr" },
				{ "Bet_Balance_Error", "Bet exceeds your current funds. Please try again!" },
				{ "Throw_Die", "Click on \"Throw\" to throw the dices!\n"},
				{ "New_Throw", "This round ended in a draw. Press \"Throw\" to try again!\n" },
				{ "Player_Round_Win", "You won this round!\n" },
				{ "Npc_Round_Win", "You lost this round!\n" },
				{ "Player_Game_Win", "Congratulations, you won this game!\n" },
				{ "Npc_Game_Win", "Sadly, you lost this game..\nDo you want to try again?\n" },
				{ "Bet_Chosen", "A bet of *kr was chosen!" },
				{ "Bet_Set", "Bet is now set, *kr deducted from your funds!" },
				{ "Founds_Added", "*kr Added to your funds!\n" }
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
				lastMessage = value == -1 ? message : message.Replace("*", $"{value}");
				return lastMessage;
			}
			return $"Message for key '{key}' not found.";
		}
	}
}
