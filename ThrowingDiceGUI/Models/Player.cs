using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowingDiceGUI.Models
{
	/** 
	 * Player class: keeps track of current balance and bet.
	 **/
	class Player
	{
		private int _Deposit;
		private int _Bet; 
		public int Deposit
		{
			get => _Deposit;
			set => _Deposit = value;
		}

		public int Bet
		{
			get => _Bet;
			set => _Bet = value;
		}

		public Player()
		{
			Deposit = 0;
			Bet = 0;
		}
	}
}
