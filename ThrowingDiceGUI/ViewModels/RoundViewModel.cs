﻿using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ThrowingDiceGUI.Models;

namespace ThrowingDiceGUI.ViewModels
{
	public class RoundViewModel: ReactiveObject
	{
		private readonly GameLogic _gameLogic;
		
		private bool _isRoundStarted;
		private bool _isThrowButtonVisible;
		private Bitmap _playerDiceImage1;
		private Bitmap _playerDiceImage2;
		private Bitmap _npcDiceImage1;
		private Bitmap _npcDiceImage2;
		
		// Throw button pressed
		public ReactiveCommand<Unit, Unit> ThrowCommand { get; }

		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		public RoundViewModel(GameLogic gameLogic) 
		{
			_gameLogic = gameLogic;
			IsThrowButtonVisible = false;

			// Display initial values
			UpdateDiceImages();

			// "Throw" button has been pressed
			ThrowCommand = ReactiveCommand.Create(_gameLogic.StartRound);

			_gameLogic.IsThrowButtonVisibleObject.Subscribe(readyToThrow =>
			{
				IsThrowButtonVisible = readyToThrow;
			});

			_gameLogic.DiceThrownObservable.Subscribe( _ => UpdateDiceImages() ).DisposeWith(_disposables);
		}

		// Dice throw button 
		public bool IsThrowButtonVisible
		{
			get => _isThrowButtonVisible;
			set => this.RaiseAndSetIfChanged(ref _isThrowButtonVisible, value);
		}

		// When a round is started the bet buttons will be disabled
		public bool IsRoundStarted
		{
			get => _isRoundStarted;
			set => this.RaiseAndSetIfChanged(ref _isRoundStarted, value);
		}

		// binding and updating of the Dice UI elements
		public Bitmap PlayerDiceImage1
		{
			get => _playerDiceImage1;
			set => this.RaiseAndSetIfChanged(ref _playerDiceImage1, value);
		}

		public Bitmap PlayerDiceImage2
		{
			get => _playerDiceImage2;
			set => this.RaiseAndSetIfChanged(ref _playerDiceImage2, value);
		}

		public Bitmap NpcDiceImage1
		{
			get => _npcDiceImage1;
			set => this.RaiseAndSetIfChanged(ref _npcDiceImage1, value);
		}

		public Bitmap NpcDiceImage2
		{
			get => _npcDiceImage2;
			set => this.RaiseAndSetIfChanged(ref _npcDiceImage2, value);
		}

		private void UpdateDiceImages()
		{
			PlayerDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.PlayerDice[0].DiceImagePath)));
			PlayerDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.PlayerDice[1].DiceImagePath)));

			NpcDiceImage1 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.NpcDice[0].DiceImagePath)));
			NpcDiceImage2 = new Bitmap(AssetLoader.Open(new Uri(_gameLogic.NpcDice[1].DiceImagePath)));
		}

		// Cleans up all subscriptions when the ViewModel is no longer needed.
		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}
