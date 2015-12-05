using UnityEngine;
using System.Collections;
using plyBloxKit;

namespace PowerLiesBeneath {
	public class PCColor : MonoBehaviour {

		// ------------------------------------------------------------------------------------------------------------
		// Properties
		public enum ColorType {Kanohi, Primary, Secondary}

		public ColorType colorType = ColorType.Kanohi;

		private Color globalVarColor;
		private Renderer rend;
		private plyVar plyColor;

		// ------------------------------------------------------------------------------------------------------------
		// Use this for initialization
		void Start () {
			rend = GetComponent<Renderer>();
			if (rend.material.name == "Mask_Color (Instance)") {
				colorType = ColorType.Kanohi;
			} else if (rend.material.name == "COLOR2 (Instance)") {
				colorType = ColorType.Primary;
			} else if (rend.material.name == "COLOR1 (Instance)") {
				colorType = ColorType.Secondary;
			}
		}
		
		// Update is called once per frame
		void Update () {
			globalVarColor = GetGlobalVarColor();

			if (globalVarColor != rend.material.color) {
				rend.material.color = globalVarColor;
			}
		}

		private Color GetGlobalVarColor() {
			Color color = Color.white;
			if (colorType == ColorType.Kanohi) {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_kanohi_clr");
			} else if (colorType == ColorType.Primary) {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_prim_clr");
			} else if (colorType == ColorType.Secondary) {
				plyColor = plyBloxGlobal.Instance.GetVariable("plr_secondary_clr");
			}
			
			if (plyColor != null) {
				plyColor.TryGetColor(out color);
			}

			return color;
		}
	}
}
