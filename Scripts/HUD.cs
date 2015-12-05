// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Copyright (c) Adrian Walker
// ====================================================================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using plyCommon;
using plyRPG;
using plyGame;
using plyBloxKit;

namespace PowerLiesBeneath {
	public class HUD : MonoBehaviour {

		// ------------------------------------------------------------------------------------------------------------
		class IntEvent : UnityEngine.Events.UnityEvent<int> { }
		// ------------------------------------------------------------------------------------------------------------
		// Properties
		public GameObject hud;								// shows info, like Health, of player and targeted
		public bool isCutscene = false;
		public bool isToaCreationScreen = false;
		private plyVar testIfCutscene;
		private plyVar testIfToaCreationScreen;

		// ------------------------------------------------------------------------------------------------------------
		public static HUD _instance = null;
		public static HUD Instance 
		{ 
			get 
			{
				if (_instance == null) Debug.LogError("The HUD is not yet loaded.");
				return _instance;
			}
		}
		// ------------------------------------------------------------------------------------------------------------
		private UnityEngine.UI.Text coinText;				// cache a reference to the Coin Text object
		private ActorClass plrClass;						// Instance of the player actor's class
		private const int NUM_OF_CELLS_IN_HP_BAR = 3;		// This is the number of "cells" in the HP & MP bars. 
		// ------------------------------------------------------------------------------------------------------------
		protected void Awake() {
			_instance = this;
			DontDestroyOnLoad(transform.gameObject);
			DontDestroyOnLoad(hud);
			coinText = GameObject.Find("CoinText").GetComponent<UnityEngine.UI.Text>();
			plrClass = Player.Instance.actor.actorClass;
			
		}

		protected void Update()
		{
			testIfCutscene = plyBloxGlobal.Instance.GetVariable("IsCutscene");
			isToaCreationScreen = (bool)plyBloxGlobal.Instance.GetVarValue("IsCustomizationScreen");
			if (testIfCutscene != null && testIfToaCreationScreen != null) {
				testIfCutscene.TryGetBool(out isCutscene);
				//testIfCutscene.TryGetBool(out isToaCreationScreen);
			}
			// Don't display HUD in these circumstances
			if (isCutscene || isToaCreationScreen) {
				if (hud.active) {
					hud.SetActive(false);
				}
			} else {
				if (!hud.active) {
					hud.SetActive(true);
				}
			}
			// mouse over GUI element?
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
			{	
				// tell plyGame's Input Manager that the mouse was handled
				plyInput.MouseInputHandled = true;
			}
		}

		protected void LateUpdate()
		{

		}

		protected void OnEnable()
		{
			ResetUI();
		}

		public static void Load()
		{
			if (_instance == null)
			{
				// UI scene is not yet loaded, do it now
				Application.LoadLevelAdditive("00_main_hud");
			}
			else
			{	
				// reset to default state
				_instance.ResetUI();
			}
		}

		private void ResetUI() {

		}

		public void DecreaseHealth() {
			plrClass.HP.ChangeBaseValueBy(-1);
		}

		public void DecreaseMagic() {
			// now access the attribute via the ActorClass reference
			ActorAttribute att = plrClass.GetAttribute("MP", plyGameObjectIdentifyingType.ident);
			att.ChangeBaseValueBy(-1);
		}

		public void GenerateCoinsNumber(int coins) {
			int[] numOfCoins = GetIntArray(coins);
			string numOfCoinsStr = "";
			
			for (int i = 0; i < numOfCoins.Length; i++) {
				numOfCoinsStr += numOfCoins[i].ToString();
			}
			coinText.text = "x "+numOfCoinsStr;
		}

		private int[] GetIntArray(int num) {
		    List<int> listOfInts = new List<int>();
		    for (int i = 0; i < 3; i++) {
		    	listOfInts.Add(num % 10);
		        num = num / 10;
		    }
		    listOfInts.Reverse();
		    return listOfInts.ToArray();
		}
	}
}
	
