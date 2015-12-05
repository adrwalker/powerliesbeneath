// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Adrian Walker
// ====================================================================================================================
using UnityEngine;
using System.Collections;
using plyGame;
using plyBloxKit;

namespace PowerLiesBeneath {
	public class TitleMenu : MonoBehaviour {
		// ------------------------------------------------------------------------------------------------------------
		class IntEvent : UnityEngine.Events.UnityEvent<int> { }
		// ------------------------------------------------------------------------------------------------------------

		public GameObject titleMenuPanel;					// Holds two buttons: Start and Quit
		public AudioClip clickSFX;							// Button sound FX
		GameObject gameCoreObj;								// GameCore object, holding our Blox
		plyBlox gameCoreBlox;								// The blox to run our events

		private const int NEXT_SCENE_ID = 2;				// The index of the Le-Wahi level in build settings
		private plyVar isCustomizationScreen;

		void Start() {
			gameCoreObj = GameObject.Find("GameCore");
			if (gameCoreObj) {
				gameCoreBlox = gameCoreObj.GetComponent<plyBlox>();
			}
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

		public void OnStartButton() {
			GetComponent<AudioSource>().clip = clickSFX;
			GetComponent<AudioSource>().Play();
			StartCoroutine(StartButton());
		}

		IEnumerator StartButton() {
			Fader.Instance.FadeIn(0.25f);
			yield return new WaitForSeconds(0.25f);
			gameCoreBlox.RunEvent("On Demo Le-Wahi");
		}

		public void OnLoadButton() {
			
		}

		public void OnToaCreationButton() {
			isCustomizationScreen = plyBloxGlobal.Instance.SetVarValue("IsCustomizationScreen", true);

			GetComponent<AudioSource>().clip = clickSFX;
			GetComponent<AudioSource>().Play();
			plyBloxGlobal.Instance.SetVarValue("nextScene", "02_toa_creation");
			Application.LoadLevel("00_loading");

		}

		public void OnQuitButton() {
			Application.Quit();
		}
	}
}
