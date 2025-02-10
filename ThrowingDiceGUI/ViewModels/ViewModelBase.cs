using ReactiveUI;
using System;
using System.Reactive;
using ThrowingDiceGUI.Models; // Imports the Dice Class (among others)

namespace ThrowingDiceGUI.ViewModels;

public class ViewModelBase : ReactiveObject
{
	private Dice _dice;
	private int _diceValue; // Store dice value for UI binding


}
