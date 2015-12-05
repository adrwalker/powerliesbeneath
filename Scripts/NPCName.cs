using UnityEngine;
using System.Collections;

public class NPCName : MonoBehaviour {

	public string name;

	private TextMesh tMesh;

	// Use this for initialization
	void Start () {
		tMesh = gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
    	if (c.gameObject.tag == "Player")
    		tMesh.text = name;
    }

    void OnTriggerExit(Collider c) {
    	if (c.gameObject.tag == "Player")
    		tMesh.text = "";
    }
}
