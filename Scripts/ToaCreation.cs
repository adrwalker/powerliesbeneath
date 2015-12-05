using UnityEngine;
using System.Collections;
using plyCommon;
using plyRPG;
using plyGame;

namespace PowerLiesBeneath {
	public class ToaCreation : MonoBehaviour {

		// ------------------------------------------------------------------------------------------------------------
		class IntEvent : UnityEngine.Events.UnityEvent<int> { }
		// ------------------------------------------------------------------------------------------------------------
		// Properties
		public GameObject toaCreation;						// The panel for the player to design their character

		// ------------------------------------------------------------------------------------------------------------
		public static ToaCreation _instance = null;
		public static ToaCreation Instance 
		{ 
			get 
			{
				if (_instance == null) Debug.LogError("The Toa Creation panel is not yet loaded.");
				return _instance;
			}
		}
		// ------------------------------------------------------------------------------------------------------------

		// ------------------------------------------------------------------------------------------------------------
		protected void Awake() {
			_instance = this;
		}

		protected void Update()
		{
			// mouse over GUI element?
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
			{	
				// tell plyGame's Input Manager that the mouse was handled
				plyInput.MouseInputHandled = true;
			}
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
				Application.LoadLevelAdditive("02_tc_options");
			}
			else
			{	
				// reset to default state
				_instance.ResetUI();
			}
		}

		private void ResetUI() {

		}
	}
}
	
