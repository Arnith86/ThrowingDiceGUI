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
		public bool IsGameStarted { get; set; }
		public bool IsGameRoundStarted { get; set; }
		public bool IsReadyToReceiveBet { get; set; }
		public bool IsReadyToThrow { get; set; }
		public bool NewRoundCanBeStarted { get; set; }
		public string MessageValue { get; set; } = string.Empty;
	}
}
