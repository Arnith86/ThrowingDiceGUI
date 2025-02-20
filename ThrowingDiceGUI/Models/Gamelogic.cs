using System;
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
		private Player player;
		private Dice[] _playerDice;
		private Dice[] _npcDice;


		// Holds the lates values of balance funds and bet
		private int _currentFundsValue;
		private int _betValue; 
		// A BehaviorSubject holds the latest value and emits it to new subscribers.
		private BehaviorSubject<int> _currentFundsSubject = new BehaviorSubject<int>(0);
		private BehaviorSubject<int> _betSubject = new BehaviorSubject<int>(0);


		private int[] roundWinCount;
		
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
		
		 
		private bool betRegistered; // still needed? 

		// This method will handel all game logic 
		public GameLogic()
		{
			player = new Player();
			_playerDice = new Dice[] { new Dice(), new Dice() };
			_npcDice = new Dice[] { new Dice(), new Dice() };
			_currentFundsSubject = new BehaviorSubject<int>(_currentFundsValue);
			_betSubject = new BehaviorSubject<int>(_betValue);
		}

		// Expose an IObservable<int> so the ViewModel can subscribe to balance changes.
		public IObservable<int> CurrentFundsObservable => _currentFundsSubject.AsObservable();
		public IObservable<int> BetObservable => _betSubject.AsObservable();

		//public bool SetAndCheckDeposit(int amount)
		//{
		//	if (amount < 100 || amount > 5000) return false; 

		//	player.Deposit = amount;
		//	return true;
		//}

		// Updates Current Funds and notify subscibers 
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

		//public bool SetAndCheckBet(int amount)
		//{
		//	if (amount > _currentFundsValue) return false;

		//	//player.Bet = amount;
		//	player.Deposit -= amount;
		//	return true;
		//}


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
