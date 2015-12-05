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
	[plyBlock("GUI", "Custom", "Update Coins on HUD", BlockType.Action, Order = 5, ShowName = "Update Coins",
		Description = "Update the number of Bionicle Symbols on the HUD.")]
	public class Custom_UpdateCoins_Block : plyBlock {

		[plyBlockField("to", ShowName = true, ShowValue = true, DefaultObject = typeof(Float_Value), SubName = "Value - Integer", Description = "Always set this to the current XP")]
		public Float_Value val;

		public override void Created()
		{
			blockIsValid = true;
		}

		public override BlockReturn Run(BlockReturn param)
		{
			int num = (int)val.RunAndGetFloat();
			HUD.Instance.GenerateCoinsNumber(num);
			return BlockReturn.OK;
		}
	}
}
	
