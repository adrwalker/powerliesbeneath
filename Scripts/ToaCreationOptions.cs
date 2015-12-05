// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Copyright (c) Adrian Walker
// ====================================================================================================================
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using plyCommon;
using plyRPG;
using plyGame;
using plyBloxKit;

namespace PowerLiesBeneath {
	public class ToaCreationOptions : MonoBehaviour {

		// ------------------------------------------------------------------------------------------------------------
		public GameObject colorPopup;	// The color popup window	
		public Slider r;				// Color slider for Red
		public Slider g;				// Color slider for Green
		public Slider b;				// Color slider for Blue
		public InputField nameField;	// Where the player types their name
		public string colorType;
		public Text titleText;
		public Sprite[] progressImgList;
		public GameObject[] wizardPanelList;
		public Slider element;
		public Slider kanohiSlider;
		public Text elementText;		// The name of the element
		public Text kanohiText;			// The name of the Kanohi
		public Image progressImg;		// Shows the progress of the wizard
		public Text overheadTitleText;	// The player's name over the character during the Review phase
		public Button nameFieldContinueBtn;

		// Properties
		private GameObject gameCoreObj = null;
		private plyBlox gameCoreBlox = null;
		private bool initComplete = false;

		// Constants used to keep track of which page of the wizard we are on.
		private const int WIZ_INTRO = 0;
		private const int WIZ_NAME = 1;
		private const int WIZ_GENDER = 2;
		private const int WIZ_ELEM = 3;
		private const int WIZ_KANOHI = 4;
		private const int WIZ_COLORS = 5;
		private const int WIZ_REVIEW = 6;

		// Kanohi Properties
		private string[] kanohiNameList = {"Noble Huna", "Noble Komau", "Noble Mahiki", "Noble Rau", "Noble Ruru",
			"Great Hau", "Great Kakama", "Great Miru", "Great Kaukau", "Great Pakari", "Great Akaku",
			"Hau Nuva"};
		private int kanohiNumber = 0;
		private int currPanelNum;
		private int currProgressImgNum;

		private const int BEGIN_OF_LIST = 0;
		private const int END_OF_LIST = 11;
		// ------------------------------------------------------------------------------------------------------------

		// Use this for initialization
		void Start () {
			gameCoreObj = GameObject.Find("GameCore");
			if (gameCoreObj) {
				gameCoreBlox = gameCoreObj.GetComponent<plyBlox>();
				initComplete = true;
			}
			currPanelNum = WIZ_INTRO;
			currProgressImgNum = WIZ_INTRO;
			
		}
		
		// Update is called once per frame
		void Update () {
			// Keep looking for the GameCore object until we find it!
			if (!initComplete) {
				if (gameCoreObj == null) {
					gameCoreObj = GameObject.Find("GameCore");
				} else {
					gameCoreBlox = gameCoreObj.GetComponent<plyBlox>();
					initComplete = true;
				}
			}
		}

		public void OnNextPage() {
			currPanelNum++;
			currProgressImgNum++;
			RedrawWizard();
		}

		public void OnPrevPage() {
			currPanelNum--;
			currProgressImgNum--;
			RedrawWizard();
		}

		private void RedrawWizard() {
			for (int i = WIZ_INTRO; i <= WIZ_REVIEW; i++) {
				wizardPanelList[i].SetActive(false);
			}
			wizardPanelList[currPanelNum].SetActive(true);
			progressImg.sprite = progressImgList[currPanelNum];
		}

		public void OnGenderChange(int param1) {
			if (gameCoreBlox != null) {
				plyEvent ev = gameCoreBlox.GetEvent("Change Gender");
				ev.SetTempVarValue("param1", param1);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnNameChange(string param2) {
			if (gameCoreBlox != null) {
				plyEvent ev = gameCoreBlox.GetEvent("Change Name");
				ev.SetTempVarValue("param2", nameField.text);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnLettersEntered(string input) {
			if (nameField.text.Length > 0) {
				nameFieldContinueBtn.interactable = true;
			} else {
				nameFieldContinueBtn.interactable = false;
			}
		}

		public void OnElementChange(float param) {
			int elementNum = (int)element.value;
			switch(elementNum) {
				case 1:
					elementText.text = "Fire";
					break;
				case 2:
					elementText.text = "Earth";
					break;
				case 3:
					elementText.text = "Water";
					break;
				case 4:
					elementText.text = "Air";
					break;
				case 5:
					elementText.text = "Ice";
					break;
				case 6:
					elementText.text = "Stone";
					break;
			}
			if (gameCoreBlox != null) {
				plyEvent ev = gameCoreBlox.GetEvent("Change Element");
				ev.SetTempVarValue("element", elementText.text);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnKanohiChange(float param) {
			kanohiNumber = (int)kanohiSlider.value;
			if (gameCoreBlox != null) {
				plyEvent ev = gameCoreBlox.GetEvent("Kanohi Change");
				ev.SetTempVarValue("num", kanohiNumber);
				gameCoreBlox.RunEvent(ev);
				kanohiText.text = kanohiNameList[kanohiNumber];
			}
		}	

		public void OnKanohiColorButtonClick() {
			if (gameCoreBlox != null) {
				Color color = Color.white;
				plyVar globalColor = plyBloxGlobal.Instance.GetVariable("plr_kanohi_clr");
				globalColor.TryGetColor(out color);
				r.value = color[0];
				g.value = color[1];
				b.value = color[2];

				plyEvent ev = gameCoreBlox.GetEvent("Color Button Click");
				ev.SetTempVarValue("param3", "kanohi");
				colorType = "kanohi";
				ev.SetTempVarValue("popup", colorPopup);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnPrimaryColorButtonClick() {
			if (gameCoreBlox != null) {
				Color color = Color.white;
				plyVar globalColor = plyBloxGlobal.Instance.GetVariable("plr_prim_clr");
				globalColor.TryGetColor(out color);
				r.value = color[0];
				g.value = color[1];
				b.value = color[2];

				plyEvent ev = gameCoreBlox.GetEvent("Color Button Click");
				ev.SetTempVarValue("param3", "primary");
				colorType = "primary";
				ev.SetTempVarValue("popup", colorPopup);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnSecondaryColorButtonClick() {
			if (gameCoreBlox != null) {
				Color color = Color.white;
				plyVar globalColor = plyBloxGlobal.Instance.GetVariable("plr_secondary_clr");
				globalColor.TryGetColor(out color);
				r.value = color[0];
				g.value = color[1];
				b.value = color[2];

				plyEvent ev = gameCoreBlox.GetEvent("Color Button Click");
				ev.SetTempVarValue("param3", "secondary");
				colorType = "secondary";
				ev.SetTempVarValue("popup", colorPopup);
				gameCoreBlox.RunEvent(ev);
			}
		}

		public void OnRedColorChange(float value) {
			if (gameCoreBlox != null) {
				Color color = Color.white;
				plyVar plyColor = getCorrectColorObj();

				if (plyColor != null) {
					plyColor.TryGetColor(out color);
					color[0] = r.value;
					plyEvent ev = gameCoreBlox.GetEvent("Change Color");
					ev.SetTempVarValue("type", colorType);
					ev.SetTempVarValue("value", color);
					gameCoreBlox.RunEvent(ev);
				} 
			}
		}

		public void OnGreenColorChange(float value) {
			if (gameCoreBlox != null) {

				Color color = Color.white;
				plyVar plyColor = getCorrectColorObj();

				if (plyColor != null) {
					plyColor.TryGetColor(out color);
					color[1] = g.value;
					plyEvent ev = gameCoreBlox.GetEvent("Change Color");
					ev.SetTempVarValue("type", colorType);
					ev.SetTempVarValue("value", color);
					gameCoreBlox.RunEvent(ev);
				} 
			}
		}

		public void OnBlueColorChange(float value) {
			if (gameCoreBlox != null) {
				Color color = Color.white;
				plyVar plyColor = getCorrectColorObj();

				if (plyColor != null) {
					plyColor.TryGetColor(out color);
					color[2] = b.value;
					plyEvent ev = gameCoreBlox.GetEvent("Change Color");
					ev.SetTempVarValue("type", colorType);
					ev.SetTempVarValue("value", color);
					gameCoreBlox.RunEvent(ev);
				} 
			}
		}

		public void OnCloseColorPopup() {
			if (gameCoreBlox != null) {
				gameCoreBlox.RunEvent("Close Color Popup");
				colorType = "";
			}
		}

		public void SetOverheadTitle(bool active) {
			plyVar plyName = null;
			plyVar plyElement = null;
			if (active) {
				plyName = plyBloxGlobal.Instance.GetVariable("plr_name");
				plyElement = plyBloxGlobal.Instance.GetVariable("plr_element");

				// overheadTitleText.text = nameField.text+"\n"+"Toa of "+elementText.text;
				overheadTitleText.text = (string)plyName.GetValue()+"\n"+"Toa of "+(string)plyElement.GetValue();
			} else {
				overheadTitleText.text = "";
			}
		}

		public void OnCharacterSave() {
			if (gameCoreBlox != null) {
				gameCoreBlox.RunEvent("Save Character");
			}
		}

		private plyVar getCorrectColorObj() {
			plyVar plyColor = null;
			if (colorType == "kanohi") {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_kanohi_clr");
			} else if (colorType == "primary") {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_prim_clr");
			} else if (colorType == "secondary") {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_secondary_clr");
			}
			return plyColor;
		}
	}
}
