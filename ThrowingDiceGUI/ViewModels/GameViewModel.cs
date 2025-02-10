using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowingDiceGUI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		private string _message = "GAME VIEW!";
		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);  // Notifies UI when changed
		}
	}
}
