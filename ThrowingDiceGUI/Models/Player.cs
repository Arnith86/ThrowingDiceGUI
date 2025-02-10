using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowingDiceGUI.ViewModels
{
	/** 
	 * Player class: keeps track of current balance and bet.
	 **/
	class Player
	{
		private int deposit;
		private int bet; 

		public Player()
		{
			deposit = 0;
			bet = 0;
		}

		public void SetBet(int bet)
		{
			this.bet = bet;
		}

		public int GetBet() 
		{
			return this.bet;
		}

		public void SetDeposit(int deposit) 
		{
			this.deposit = deposit;
		}

		public int GetDeposit() 
		{
			return this.deposit;
		}
	}
}
