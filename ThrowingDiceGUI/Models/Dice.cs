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
		private string _DiceImagePath;
		
		public int DiceValue 
		{
			get => _DiceValue;
			set 
			{
				_DiceValue = value;
				UpdateDiceImage();
			}
		}

		// Declares DiceImagePath as a read-only property (only a getter, no set method).				
		public string DiceImagePath => _DiceImagePath;  

		public Dice() 
		{
			DiceValue = 1;
		}


		private void UpdateDiceImage()
		{
			_DiceImagePath = DiceValue switch
			{
				1 => "avares://ThrowingDiceGUI/Assets/dice (1).png",
				2 => "avares://ThrowingDiceGUI/Assets/dice (2).png",
				3 => "avares://ThrowingDiceGUI/Assets/dice (3).png",
				4 => "avares://ThrowingDiceGUI/Assets/dice (4).png",
				5 => "avares://ThrowingDiceGUI/Assets/dice (5).png",
				6 => "avares://ThrowingDiceGUI/Assets/dice (6).png",
				_ => "avares://ThrowingDiceGUI/Assets/dice (Error).png",
			};
		}

		public int ThrowDice()
		{
			Random randomSeed = new Random();
			DiceValue = randomSeed.Next(1, 7);


			return DiceValue;
		}
	}
}
