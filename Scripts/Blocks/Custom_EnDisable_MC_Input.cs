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
using com.ootii.AI.Controllers; 

namespace PowerLiesBeneath {
	[plyBlock("Custom", "General", "(dis)Enable MC Input", BlockType.Action, Order = 5, Description = "Enable or Disable the Input in Motion Controller")]
	public class Custom_EnDisable_MC_Input : plyBlock {

		[plyBlockField("state", ShowValue = true)]
		public plyEnabledState enable = plyEnabledState.Enabled;

		[plyBlockField("MC Input on", ShowName = true, ShowValue = true, EmptyValueName = "-self-", SubName = "Target - GameObject", Description = "Should be object that has a Motion Controller component.")]
		public GameObject_Value target;

		[plyBlockField("Cache target", Description = "Tell plyBlox if it can cache a reference to the Target Object, if you know it will not change, improving performance a little. This is done either way when the target is -self-")]
		public bool cacheTarget = false;

		private MotionController mController = null;

		public override void Created() {
			blockIsValid = true;
			if (target == null) cacheTarget = true; // force caching when target is null (-self-)
		}

		public override BlockReturn Run(BlockReturn param) {
			if (mController == null)
			{
				GameObject o = target == null ? owningBlox.gameObject : target.RunAndGetGameObject();
				if (o != null)
				{
					mController = o.GetComponent<MotionController>();
					if (mController == null)
					{
						blockIsValid = false;
						Log(LogType.Error, "The Target is invalid. Could not find any Motion Controller component on it.");
						return BlockReturn.Error;
					}
				}
			}

			mController.UseInput = (enable == plyEnabledState.Enabled);
			if (false == cacheTarget) mController = null;
			return BlockReturn.OK;
		}
	}
}
