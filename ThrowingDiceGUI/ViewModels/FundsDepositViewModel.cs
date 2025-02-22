using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class FundsDepositViewModel : ReactiveValidationObject, IDisposable
	{
		private readonly GameLogic _gameLogic;
		private string _inputFundsDeposit;
		private int _currentFundsValue;

		private bool _isFundPanelVisible;

		private static readonly Regex InputFundsRegex = new Regex(@"^\d+$");
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		public FundsDepositViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;

			// Assignes Funds value
			// CURRENTLY BUGGED INITIALLY ACCEPTS INVALIED VALUE, after that functions fine !!!!!!!!!!!!!!!!!!!!!!!!! 
			InputFundDepositCommand = ReactiveCommand.Create<string>(deposit =>
			{
				InputFundsDeposit = deposit;

				if (this.ValidationContext.IsValid)
				{
					_gameLogic.UpdateFunds(int.Parse(InputFundsDeposit));
					IsFundPanelVisible = false;
					// AskForPlaceBet(); // FIGURE OUT HOW TO DO THIS AFTER REFACTORING
				}

			});

			// Subscribe to balance updates from the GameLogic
			_gameLogic.CurrentFundsObservable.Subscribe(Funds =>
			{
				// Update the ViewModel when the balance changes
				CurrentFunds = Funds;
			}).DisposeWith(_disposables);

			// Validates inputs
			this.ValidationRule(
				FundsDepositViewModel => FundsDepositViewModel.InputFundsDeposit,
				inputFundsDeposit => !string.IsNullOrWhiteSpace(inputFundsDeposit) && InputFundsRegex.IsMatch(inputFundsDeposit) &&
										int.TryParse(inputFundsDeposit, out int depositAmount) && (depositAmount >= 100 && depositAmount <= 5000),
				"Only integer values between 100 and 5000 are permited!"
			);
		}

		// New fund deposit recived
		public ReactiveCommand<string, Unit> InputFundDepositCommand { get; }

		// Enable or disable FundPanel visibility 
		public bool IsFundPanelVisible
		{
			get => _isFundPanelVisible;
			set => this.RaiseAndSetIfChanged(ref _isFundPanelVisible, value);
		}

		public int CurrentFunds
		{
			get => _currentFundsValue;
			set => this.RaiseAndSetIfChanged(ref _currentFundsValue, value);
		}

		// houses the inputed value for fund deposit
		public string InputFundsDeposit
		{
			get => _inputFundsDeposit;
			set => this.RaiseAndSetIfChanged(ref _inputFundsDeposit, value);
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
