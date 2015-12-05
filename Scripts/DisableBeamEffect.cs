using UnityEngine;
using plyBloxKit;
using System.Collections;

public class DisableBeamEffect : MonoBehaviour {

	void Update() {
		if (gameObject.active) {
			bool isSkillActivated = (bool)plyBloxGlobal.Instance.GetVarValue("plr_elem_beam");
			if (!isSkillActivated) {
				gameObject.SetActive(false);
			}
		}
	}
}
