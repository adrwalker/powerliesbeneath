using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	public GameObject fireSpray;
	public GameObject airSpray;
	public GameObject earthSpray;
	public GameObject stoneSpray;
	public GameObject waterSpray;
	public GameObject iceSpray;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateBeam(string element) {
		if (element == "Fire") {
			fireSpray.SetActive(true);
		}
		if (element == "Air") {
			airSpray.SetActive(true);
		}
		if (element == "Earth") {
			earthSpray.SetActive(true);
		}
		if (element == "Stone") {
			stoneSpray.SetActive(true);
		}
		if (element == "Water") {
			waterSpray.SetActive(true);
		}
		if (element == "Ice") {
			iceSpray.SetActive(true);
		}
	}

	public void EndBeam() {
		fireSpray.SetActive(false);
		airSpray.SetActive(false);
		earthSpray.SetActive(false);
		stoneSpray.SetActive(false);
		waterSpray.SetActive(false);
		iceSpray.SetActive(false);
	}
}
