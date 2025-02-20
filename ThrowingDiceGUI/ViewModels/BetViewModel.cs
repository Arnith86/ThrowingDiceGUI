using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class BetViewModel
	{
		private readonly GameLogic _gameLogic;
		public BetViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
		}
	}
}
