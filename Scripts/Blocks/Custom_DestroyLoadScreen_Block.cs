// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Copyright (c) Adrian Walker
// ====================================================================================================================
using UnityEngine;
using System.Collections;
using plyCommon;
using plyBloxKit;
using plyGame;
using plyRPG;

namespace PowerLiesBeneath {
	[plyBlock("Custom", "General", "Destroy Load Screen", BlockType.Action, Order = 5, ShowName = "Destroy Load Screen",
		Description = "Once the scene is loaded. We must destroy the Load Screen ourselves")]
	public class Custom_DestroyLoadScreen_Block : plyBlock {

		public override void Created() {
			blockIsValid = true;
		}

		public override BlockReturn Run(BlockReturn param) {
			GameObject go = GameObject.FindWithTag("LoadingScreen");
			if (go) {
				GameObject.Destroy(go);
				Fader.Instance.FadeOut(0.25f);
				return BlockReturn.OK;
			} else {
				Debug.LogError("No Loading Screen found!");
				return BlockReturn.Error;
			}
			
		}
	}
}
