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
	[plyBlock("GUI", "Custom", "Load Toa Creation UI", BlockType.Action, Order = 5, ShowName = "Load Toa Creation UI",
		Description = "Load the UI for the Player Select / Toa Creation screen")]
	public class Custom_LoadPlayerSelectUI_Block : plyBlock {

		public override void Created()
		{
			blockIsValid = true;
		}

		public override BlockReturn Run(BlockReturn param)
		{
			ToaCreation.Load();
			return BlockReturn.OK;
		}
	}
}