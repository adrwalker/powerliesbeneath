using UnityEngine;
using System.Collections;
using plyGame;
 
[RequireComponent (typeof (GUIText))]
public class ObjectLabel : MonoBehaviour {
 
public Transform target;  // Object that this label should follow
public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
public bool useMainCamera = true;   // Use the camera tagged MainCamera
public Camera cameraToUse ;   // Only use this if useMainCamera is false
public string label;

private Camera cam;
private Transform thisTransform;
private Transform camTransform;
private GUIText text;
private bool flag;
 
	void Start () 
    {
	    thisTransform = transform;
	    if (useMainCamera)
	        cam = Player.Camera;
	    else
	        cam = cameraToUse;
    	camTransform = cam.transform;
    	text = gameObject.GetComponent<GUIText>();
        
	}
 
 
    void Update()
    {
 
		if (cam == null) {
			cam = Player.Camera;
			if (Player.Camera == null) {
				return;
			}
		}
        UpdateText();
        
        if (clampToScreen)
        {
            Vector3 relativePosition = camTransform.InverseTransformPoint(target.position + offset);
            relativePosition.z =  Mathf.Max(relativePosition.z, 1.0f);
            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition));
            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize),
                                             Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize),
                                             thisTransform.position.z);
 
        }
        else
        {
            thisTransform.position = cam.WorldToViewportPoint(target.position + offset);

        }
    }

    void UpdateText() {
        if (!flag) {
            SphereCollider collider = GetComponent<SphereCollider>();
            collider.center = new Vector3(transform.position.x*-1, transform.position.y*-1, transform.position.z*-1);
        }
        
        flag = true;
    }

    void OnTriggerEnter(Collider c) {
    	if (c.gameObject.tag == "Player")
    		text.text = label;
    }

    void OnTriggerExit(Collider c) {
    	if (c.gameObject.tag == "Player")
    		text.text = "";
    }
}
