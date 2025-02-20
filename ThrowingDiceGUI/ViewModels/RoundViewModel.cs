using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class RoundViewModel
	{
		private readonly GameLogic _gameLogic;
		public RoundViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
		}
	}
}
