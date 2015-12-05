// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Adrian Walker
// ====================================================================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using plyCommon;
using plyRPG;
using plyGame;
using plyBloxKit;

//	This class is responsible for rendering the Heads Up Display in the Unity game engine. A GameObject is a Unity-specific class that ties it to an object
//	in the game. This can be any object: a person, a sound, or a user interface panel.
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
		// This method is called automatically by the Unity game engine at the start of the game. This method is reserved for initialization
		protected void Awake() {
			_instance = this;
			DontDestroyOnLoad(transform.gameObject);		//Objects are usually removed from memory when moving from level to level. We don't want that to happen to this object
			DontDestroyOnLoad(hud);							//Objects are usually removed from memory when moving from level to level. We don't want that to happen to this object
			coinText = GameObject.Find("CoinText").GetComponent<UnityEngine.UI.Text>(); // Coin text is the number of coins that the player has collected
			plrClass = Player.Instance.actor.actorClass;	// This object caches the player data from a global object
			
		}

		protected void Update()
		{
			testIfCutscene = plyBloxGlobal.Instance.GetVariable("IsCutscene");
			isToaCreationScreen = (bool)plyBloxGlobal.Instance.GetVarValue("IsCustomizationScreen");
			if (testIfCutscene != null && testIfToaCreationScreen != null) {
				testIfCutscene.TryGetBool(out isCutscene);
			}
			// Don't display HUD if we're in a game cutscene or if we are in the player initialization screen
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

		// This method is called so that the level data for the HUD is loaded. This data holds the graphics and their positioning. 
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

		// This creates the number of coins a player has and renders it to the screen
		public void GenerateCoinsNumber(int coins) {
			int[] numOfCoins = GetIntArray(coins);
			string numOfCoinsStr = "";
			
			for (int i = 0; i < numOfCoins.Length; i++) {
				numOfCoinsStr += numOfCoins[i].ToString();
			}
			coinText.text = "x "+numOfCoinsStr;
		}

		// General utility method
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
	
