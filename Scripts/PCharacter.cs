using UnityEngine;
using System.Collections;
using plyGame;
using plyBloxKit;
using com.ootii.Input;
using com.ootii.AI.Controllers;

namespace PowerLiesBeneath {
	[RequireComponent(typeof (MotionController))]
	public class PCharacter : PlayerBaseController {

		public CharacterController characterController;
		public Camera playerCamera;

		private Transform camTr;
		private ActorClass actorClass;
		private ActorAttribute att;
		private MotionController mController;
		private RingMenu ringMenu;

		private Material kanohiMaterial;
		private Material primaryMaterial;
		private Material secondaryMaterial;
		private plyBlox blox;

		new protected void Awake()
		{
			base.Awake();

			if (characterController == null) characterController = GetComponent<CharacterController>();
			if (characterController == null)
			{
				Debug.LogError("[ThirdPersonController] No CharacterController assigned. Character will now be disabled.");
				enabled = false;
				return;
			}
			mController = GetComponent<MotionController>();
			
			_tr = characterController.transform;
			blox = GetComponent<plyBlox>();
		}

		new protected void Start() {
			base.Start();
			GameObject go = GameObject.FindWithTag("MainCamera");
			// tell plyGame this is the player camera
    		Player.Camera = go.GetComponent<Camera>();
		}

		new protected void Update()
		{
			base.Update();
			if (Player.Camera == null) Debug.Log("Player Camera is null");
			if (camTr == null)
			{
				if (Player.Camera != null)
				{
					camTr = Player.Camera.transform;
				}
				else return;
			}

			// --------------------------------------------------------------------------------
			// Call the Main Menu
			// --------------------------------------------------------------------------------
			if (ootiiInputStub.IsJustPressed("CallMenu")) {
				GameObject mainMenu = GameObject.Find("Main Menu Canvas/Main Menu");
				if (mainMenu != null) {
					ringMenu = mainMenu.GetComponent<RingMenu>();
					mainMenu = null;
				}

				if (ringMenu.currentMenu == RingMenu.MenuState.Main) {
					mController.enabled = true;
					plyBloxGlobal.Instance.SetVarValue("IsMenuActive", false);
					plyEvent ev = blox.GetEvent("On Menu Call");
					ev.SetTempVarValue("param1", false);
					blox.RunEvent(ev);
				} else {
					mController.enabled = false;
					plyBloxGlobal.Instance.SetVarValue("IsMenuActive", true);
					plyEvent ev = blox.GetEvent("On Menu Call");
					ev.SetTempVarValue("param1", true);
					blox.RunEvent(ev);
				}
			}
			// Choose which element skill to wield
			SkillChange();
		}

		// --------------------------------------------------------------------------------
		// Choose which elemental skill to wield
		// --------------------------------------------------------------------------------
		private void SkillChange() {
			if (ootiiInputStub.IsJustPressed("Action1")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 1);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action2")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 2);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action3")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 3);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action4")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 4);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action5")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 5);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action6")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 6);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action7")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 7);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action8")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 8);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action9")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 9);
				blox.RunEvent(ev);
			}
			if (ootiiInputStub.IsJustPressed("Action10")) {
				plyEvent ev = blox.GetEvent("On Skill Change");
				ev.SetTempVarValue("skillNum", 10);
				blox.RunEvent(ev);
			}
		}

		public void Reward(string npcAndLootName) {
			string[] parts = npcAndLootName.Split('|');
			string npc = parts[0];
			string loot = parts[1];
			GameObject go = GameObject.Find(npc+"/Reward");
			if (go) {
				Vector3 p = go.transform.position + new Vector3(0,2.0f,0);
				if (LootAsset.Instance.DropLoot(loot, p, go)) {
					//Debug.Log("Loot "+loot+" dropped successfully");
				} else {
					//Debug.LogError("Loot drop for "+loot+" failed");
				}
			}
		}
	}
}

