﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reactive.Subjects;
using System.Net.Http.Headers;
using System.Reactive.Linq;

namespace ThrowingDiceGUI.Models
{
	public class GameLogic
	{
		private static string _START_DEPOSIT = "Start_Deposit";
		private static string _WELCOME = "Welcome";

		private Player player; // should i keep this? 
		private Dice[] _playerDice;
		private Dice[] _npcDice;

		// Holds the lates values of balance funds and bet
		private int _currentFundsValue = 0;
		private int _betValue = 0;

		private string _messageValue = "";

		// Visibility boolean
		private bool _isStartButtonVisible;
		private bool _isFundPanelVisible;


		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<int> _currentFundsSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _betSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<string> _messageSubject = new BehaviorSubject<string>("");
		private BehaviorSubject<bool> _isStartButtonVisibleSubject = new BehaviorSubject<bool>(true);
		private BehaviorSubject<bool> _isFundPanelVisibleSubject = new BehaviorSubject<bool>(true);



		public Dice[] PlayerDice
		{
			get => _playerDice;
			set => _playerDice = value;
		}

		public Dice[] NpcDice
		{
			get => _npcDice;
			set => _npcDice = value;
		}
				
		public int CurrentFundsValue => _currentFundsValue;
		public int BetValue => _betValue; 
		public string MessageValue
		{
			get => _messageValue;
			set => _messageValue = value;
		} 
		public bool IsStartButtonVisible
		{
			get => _isStartButtonVisible;
			set => _isStartButtonVisible = value;
		}

		public bool IsFundPanelVisible
		{
			get => _isFundPanelVisible;
			set => _isFundPanelVisible = value;
		}

		private bool betRegistered; // still needed? 

		// This method will handel all game logic 
		public GameLogic()
		{
			// Displays welcome message on application start
			UpdateMessage(_WELCOME);
			IsStartButtonVisible = true;
			
			player = new Player();
			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_currentFundsSubject = new BehaviorSubject<int>(_currentFundsValue);
			_betSubject = new BehaviorSubject<int>(_betValue);
			_messageSubject = new BehaviorSubject<string>(_messageValue);
			_isStartButtonVisibleSubject = new BehaviorSubject<bool>(_isStartButtonVisible);
			_isFundPanelVisibleSubject = new BehaviorSubject<bool>(_isFundPanelVisible);
		}

		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<int> CurrentFundsObservable => _currentFundsSubject.AsObservable();
		public IObservable<int> BetObservable => _betSubject.AsObservable();
		public IObservable<string> MessageObservable => _messageSubject.AsObservable();
		public IObservable<bool> IsStartButtonVisibleObservable => _isStartButtonVisibleSubject.AsObservable();
		public IObservable<bool> IsFundPanelVisíbleObservable => _isFundPanelVisibleSubject.AsObservable();

		// Updates Values and notify subscibers 
		public void UpdateMessage(string message)
		{
			MessageValue = Messages.Instance.GetMessage(message);
			_messageSubject.OnNext(MessageValue);
		}
		public void UpdateFunds(int amount)
		{
			_currentFundsValue = amount;
			_currentFundsSubject.OnNext(amount);
		}

		public void UpdateBet(int amount)
		{
			_betValue = amount;
			_betSubject.OnNext(amount);
			_currentFundsValue -= amount;
			_currentFundsSubject.OnNext(amount);
		}

		public void UpdateIsStartButtonVisible(bool tf)
		{
			IsStartButtonVisible = tf;
			_isStartButtonVisibleSubject.OnNext(IsStartButtonVisible);
		}

		public void UpdateIsFundPanelVisible(bool tf)
		{
			IsFundPanelVisible = tf;
			_isFundPanelVisibleSubject.OnNext(IsFundPanelVisible);
		}

		//public bool SetAndCheckBet(int amount)
		//{
		//	if (amount > _currentFundsValue) return false;

		//	//player.Bet = amount;
		//	player.Deposit -= amount;
		//	return true;
		//}
		// Prepers for and starts a new round

		// If not enough funds, sends to "AskForDeposit" method, otherwise "PlaceBet"
		public void NewRound()
		{
			//PlayerScore = 0;
			//NpcScore = 0;
			//CurrentBet = 0;
			//IsNewRoundButtonVisible = false;

			//if (CurrentFunds < 100)
			//{
			//	AskForDeposit();
			//}
			//else
			//{
			//	AskForPlaceBet();
			//}

		}

		// Starts the game
		// Ask for deposit to funds
		public void AskForDeposit()
		{
			UpdateMessage(_START_DEPOSIT);
			UpdateIsStartButtonVisible(false);
			UpdateIsFundPanelVisible(true);
		}

		// Ask which bet to place
		public void AskForPlaceBet()
		{
			//IsBetPanelVisible = true;
			//BetButtonsEnabled = true;
			//Message = Messages.Instance.GetMessage(_START_BET);
		}

		//// Registers deposit to funds
		//public void AddFundsDeposit(int deposit)
		//{
		//	CurrentFunds = deposit;
		//	IsFundPanelVisible = false;
		//	AskForPlaceBet();
		//}

		//// Registers deposit to funds
		//public void AddFundsDeposit()
		//{
		//	// Converts string to int and checks if input is between 100 and 5000
		//	if (int.TryParse(InputFundsDeposit, out int depositAmount) && _game.SetAndCheckDeposit(depositAmount))
		//	{
		//		CurrentBalance = depositAmount;
		//		IsFundPanelVisible = false;
		//		AskForPlaceBet();
		//	}
		//	else
		//	{
		//		Message = Messages.Instance.GetMessage(_DEPOSIT_ERROR);
		//	}
		//}

		// Registers chosen bet
		public void RegisterBet()
		{
			//// Converts string to int and checks if bet exceed funds
			//if (int.TryParse(InputBet, out int betAmount) && _gameLogic.SetAndCheckBet(betAmount))
			//{
			//	CurrentBet = betAmount;
			//	CurrentFunds = _gameLogic.CurrentFundsValue;
			//	Message = Messages.Instance.GetMessage(_THROW_DIE); 
			//	IsThrowButtonVisible = true;
			//}
			//else
			//{
			//	Message = Messages.Instance.GetMessage(_BET_BALANCE_ERROR);
			//}
		}

		// Initiates a new round
		// throws all dices and evaluates results 
		public void StartRound()
		{
			//IsThrowButtonVisible = false;
			//IsRoundStarted = true; // when true, bet buttons are disabled
			//_gameLogic.ThrowDiceSet(_gameLogic.PlayerDice);
			//_gameLogic.ThrowDiceSet(_gameLogic.NpcDice);
			//_gameLogic.PlayerDice = _gameLogic.SorByDescending(_gameLogic.PlayerDice);
			//_gameLogic.NpcDice = _gameLogic.SorByDescending(_gameLogic.NpcDice);

			//updateDiceImages();

			//// Both player and npc dice are equal, a new throw will be conducted 
			//if (_gameLogic.CheckIdenticalDiceSet(_gameLogic.PlayerDice, _gameLogic.NpcDice))
			//{
			//	Message = Messages.Instance.GetMessage(_NEW_THROW);
			//}
			//else if (_gameLogic.RoundEvaluation(_gameLogic.PlayerDice, _gameLogic.NpcDice))
			//{
			//	Message = Messages.Instance.GetMessage(_PLAYER_ROUND_WIN);
			//	PlayerScore++;
			//}
			//else
			//{
			//	Message = Messages.Instance.GetMessage(_NPC_ROUND_WIN);
			//	NpcScore++;
			//}

			//if (PlayerScore != 2 && NpcScore != 2) IsThrowButtonVisible = true;  
		}

		// Handles a single round of dice throws 
		public bool RoundEvaluation(Dice[] playerDice, Dice[] npcDice)
		{
			int playerHighest = 0;
			int npcHighest = 0;
						
			playerHighest = playerDice[0].DiceValue;
			npcHighest = npcDice[0].DiceValue;
							
			// Highest valued dice of both players equal, use the other die
			if (playerDice[0].DiceValue == npcDice[0].DiceValue)
			{
				playerHighest = playerDice[1].DiceValue;
				npcHighest = npcDice[1].DiceValue;
			}
		
			// Has player won?
			return playerHighest > npcHighest;
		}

		// Check if dice set are identical (they must be sorted first)
		public bool CheckIdenticalDiceSet(Dice[] playerDice, Dice[] npcDice)
		{
			return (playerDice[0].DiceValue == npcDice[0].DiceValue && playerDice[1].DiceValue == npcDice[1].DiceValue);	
		}

		// Sort the array with highest valued element first 
		public Dice[] SorByDescending(Dice[] dices)
		{
			return dices.OrderByDescending(n => n.DiceValue).ToArray();
		}

		// Throws both dices 
		public Dice[] ThrowDiceSet(Dice[] dices)
		{
			foreach (Dice dice in dices)
			{
				dice.ThrowDice();
			}

			return dices;
		}
	}
}
