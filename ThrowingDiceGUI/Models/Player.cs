using ReactiveUI;
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
		private int _Deposit;
		private int _Bet; 
		public int Deposit
		{
			get
			{
				return _Deposit;
			}
			set
			{
				_Deposit = value;
			}
		}
		public int Bet
		{
			get
			{
				return _Bet;
			}
			set
			{
				_Bet = value;
			}
		}

		public Player()
		{
			Deposit = 0;
			Bet = 0;
		}

		//public void SetBet(int bet)
		//{
		//	this.bet = bet;
		//}

		//public int GetBet() 
		//{
		//	return this.bet;
		//}

		//public void SetDeposit(int deposit) 
		//{
		//	this.deposit = deposit;
		//}

		//public int GetDeposit() 
		//{
		//	return this.deposit;
		//}
	}
}
