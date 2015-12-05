using UnityEngine;
using System.Collections;

namespace PowerLiesBeneath {
	/// <summary>
    /// Handles all of the events related to player animations. It plays audio, manipulates character data,
    /// and emits particles as necessary
    /// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class PCAnimationEvents : MonoBehaviour {

		private Transform sword;
		private Transform swordSheathed;
		private AudioSource audioSource;
		private GameObject swordTrail;

		private string artTransform;

		public AudioClip getSword;
		public AudioClip swordAtk1;
		public AudioClip grassFootsteps;
		public Avatar femaleAvatar;

		// Use this for initialization
		void Start () {
			sword = GetComponent<Transform>().Find("Art - Male(Clone)/Armature/root/spine/spine_2/arm_R/elbow_R/hand_R/Cyan Sword/Sword");
			swordSheathed = GetComponent<Transform>().Find("Art - Male(Clone)/Armature/root/spine/spine_2/Cyan Sword/Sword");

			// If the model is female then find the female version
			if (sword == null || swordSheathed == null) {
				sword = GetComponent<Transform>().Find("Art - Female(Clone)/Armature/root/spine/spine_2/arm_R/elbow_R/hand_R/Cyan Sword/Sword");
				swordSheathed = GetComponent<Transform>().Find("Art - Female(Clone)/Armature/root/spine/spine_2/Cyan Sword/Sword");
				
				// Use the female avatar model and set the Female parameter
				Animator a = GetComponent<Animator>();
				a.avatar = femaleAvatar;
				a.SetBool("Female", true);
			}
			audioSource = GetComponent<AudioSource>();
			if (sword) {
				swordTrail = sword.FindChild("Trail 28").gameObject;
				sword.gameObject.SetActive(false);
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		/* This method is called to create the footstep audio
		and particle effects*/
		public void OnFootstep() {
			audioSource.PlayOneShot(grassFootsteps, 1f);
		}

		public void OnSwordAttack(int animation) {

			StartEmission();
			// Play the sound effect
			switch (animation) {
				case 1:
					audioSource.PlayOneShot(swordAtk1, 1f);
					break;
				default:
					audioSource.PlayOneShot(swordAtk1, 1f);
					break;
			}
		}

		public void StartEmission() {
			// Turn on the weapon tail
			ParticleSystem ps = swordTrail.GetComponent<ParticleSystem>();
			ps.enableEmission = true;
		}

		public void EndEmission() {
			// Turn off weapon trail
			ParticleSystem ps = swordTrail.GetComponent<ParticleSystem>();
			ps.enableEmission = false;
		}

		/// <summary>
	    /// Switch sword meshes when player draws the Cyan sword
	    /// </summary>	
		public void OnSwordDraw() {
			if (sword && swordSheathed) {
				// Activate held sword
				sword.gameObject.SetActive(true);

				// Play Sound FX
				audioSource.PlayOneShot(getSword, 1f);

				// Deactivate sheathed sword
				swordSheathed.gameObject.SetActive(false);
			}
		}

		/// <summary>
	    /// Switch sword meshes when player sheaths the Cyan sword
	    /// </summary>	
		public void OnSwordSheath() {
			if (sword && swordSheathed) {

				// Activate sheathed sword
				swordSheathed.gameObject.SetActive(true);

				// Deactivate held sword
				sword.gameObject.SetActive(false);
			}
		}

		public void OnDodgeStart() {
			CharacterController cController = GetComponent<CharacterController>();
			cController.radius = 0.1f;
			cController.height = 1.05f;
			cController.center = new Vector3(0f, 0.7f, 0f);
		}
		public void OnDodgeEnd() {
			CharacterController cController = GetComponent<CharacterController>();
			cController.radius = 0.5f;
			cController.height = 2.1f;
			cController.center = new Vector3(0, 1.05f, 0);
		}
	}
}
