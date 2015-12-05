// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Adrian Walker
// ====================================================================================================================
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using plyBloxKit;
using plyGame;

namespace PowerLiesBeneath {
	public class LoadingScreen : BaseLoader {

		private Image loadingImg;
		private float loadAmount;
		private string sceneToLoad;
		private plyBlox gameCoreBlox = null;
		private const string SCENE_ASSET = "scenes/air.dlc";

		// Use this for initialization
		IEnumerator Start () {
			// Initialize the asset bundle loader
			yield return StartCoroutine(Initialize() );

			// Continue
			DontDestroyOnLoad(transform.gameObject);
			GameObject gameCoreObj = GameObject.Find("GameCore");
			if (gameCoreObj) {
				gameCoreBlox = gameCoreObj.GetComponent<plyBlox>();
			}

			// Find the right image by loading them all (there are only three) and choosing
			// the correct image based on its Image Type.
			Image[] loadingImages = gameObject.GetComponentsInChildren<Image>();
			foreach (Image i in loadingImages) {
				if (i.type == Image.Type.Filled) {
					loadingImg = i;
					break;
				}
			}

			// Keep track of the scene we want to load
			sceneToLoad = (string)plyBloxGlobal.Instance.GetVarValue("nextScene");

			// If there is a scene to load then load it
			if (sceneToLoad != null) {
				// Also check for Toa Creation Screen. We need to change GameCore states for that
				if (sceneToLoad == "02_toa_creation" && gameCoreBlox != null) {
					gameCoreBlox.RunEvent("On Start Toa Creation Screen");
				}
				// Screen Fade
				Fader.Instance.FadeOut(0.25f);
				yield return new WaitForSeconds(0.25f);

				// Start loading animation
				StartCoroutine(InDeterminiteLoadingImage());

				// // Start async operation to load the scene
				AsyncOperation async = Application.LoadLevelAsync(sceneToLoad);
		        yield return async;
		        // Screen Fade
				Fader.Instance.FadeIn(0.25f);
				// yield return new WaitForSeconds(1.5f);
		    } else {
		    	Debug.LogError("Error loading scene: Scene name is null");
		    }
			
		}

		IEnumerator InDeterminiteLoadingImage() {
			while(loadingImg != null) {
				yield return new WaitForSeconds(0.25f);
				loadingImg.fillAmount -= 0.25f;
				yield return new WaitForSeconds(0.25f);
				loadingImg.fillAmount -= 0.25f;
				yield return new WaitForSeconds(0.25f);
				loadingImg.fillAmount -= 0.25f;
				yield return new WaitForSeconds(0.25f);
				loadingImg.fillAmount -= 0.25f;
				yield return new WaitForSeconds(0.25f);
				loadingImg.fillAmount = 0f;
				yield return new WaitForSeconds(0.25f);
			}
		}
	}
}