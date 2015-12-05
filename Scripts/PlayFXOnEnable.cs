using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class PlayFXOnEnable : MonoBehaviour {

	private ParticleSystem effect;

	void Start() {
		effect = GetComponent<ParticleSystem>();
		effect.Play();
	}
}
