using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowingDiceGUI.Models
{

	/**
	 * The class represents a six sided die, and the rolling of said die. 
	 **/

	public class Dice
	{
		private int _DiceValue;
		public int DiceValue 
		{
			get => _DiceValue;
			set => _DiceValue = value;
		}
		
		public Dice() 
		{
			DiceValue = 1; 
		}

		public int ThrowDice()
		{
			Random randomSeed = new Random();
			
			return DiceValue = randomSeed.Next(1, 7);
		}
	}
}
