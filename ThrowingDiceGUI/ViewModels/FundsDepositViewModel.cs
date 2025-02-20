using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class FundsDepositViewModel : ReactiveValidationObject
	{
		private readonly GameLogic _gameLogic; 
		private static readonly Regex InputFundsRegex = new Regex(@"^\d+$");

		public FundsDepositViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
		}
	}
}
