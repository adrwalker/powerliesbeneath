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
	[plyBlock("GUI", "Custom", "Load HUD", BlockType.Action, Order = 5, ShowName = "Load Game HUD",
		Description = "Load the HUD for Power Lies Beneath.")]
	public class Custom_LoadUI_Block : plyBlock {

		public override void Created()
		{
			blockIsValid = true;
		}

		public override BlockReturn Run(BlockReturn param)
		{
			HUD.Load();
			return BlockReturn.OK;
		}
	}
}
	
