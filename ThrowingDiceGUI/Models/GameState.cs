using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowingDiceGUI.Models
{
	// Holds the current state of the game
	public class GameState
	{

		public int CurrentFunds { get; set; } = 0;
		public int Bet { get; set; } = 0;
		//public bool IsAwaitingDeposit {  get; set; }
		//public bool IsAwaitingBet { get; set; }
		public bool IsBetLockedIn { get; set; }
		public bool IsAwaitingThrow { get; set; }
		public bool IsGameStarted { get; set; }
		public string MessageValue { get; set; } = string.Empty;
		public bool EnoughFundsForBet { get; set; }
		public bool GameRoundIsActive { get; set; }


		

	}
}
