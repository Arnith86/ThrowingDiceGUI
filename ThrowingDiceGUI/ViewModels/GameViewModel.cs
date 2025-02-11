using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private readonly Gamelogic _game;
		private string _message = "GAME VIEW";

		public GameViewModel()
		{
			_game = new Gamelogic();

		}

		public int PlayerScore => _game.PlayerScore;
		public int NpcScore => _game.NpcScore;
		public int CurrentBalance => _game.CurrentBalance;
		public int CurrentBet => _game.CurrentBet;

		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}

	}
}
