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
	[plyBlock("Custom", "General", "Load Scene", BlockType.Action, Order = 5, ShowName = "Load Scene",
		Description = "Load the next scene for the game")]
	public class Custom_LoadScene_Block : plyBlock {

		public override void Created()
		{
			blockIsValid = true;
		}

		public override BlockReturn Run(BlockReturn param)
		{
			Application.LoadLevel("00_loading");
			return BlockReturn.OK;
		}
	}
}