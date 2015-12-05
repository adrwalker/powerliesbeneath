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
	[plyBlock("Custom", "General", "Apply Color", BlockType.Action, Order = 5, ShowName = "Apply Color",
		Description = "Apply the color chosen in the Toa Creation screen to the player character.")]
	public class Custom_ApplyColor_Block : plyBlock {

		[plyBlockField("from", ShowName = true, ShowValue = true, DefaultObject = typeof(Color_Value), SubName="Color", Description = "The color to apply.")]
		public Color_Value color;

		[plyBlockField("to", ShowName = true, ShowValue = true, EmptyValueName = "-self-", SubName="Target - GameObject", Description = "Object to apply the color.")]
		public GameObject_Value target;

		[plyBlockField("Cache target", Description = "Tell plyBlox if it can cache a reference to the Target Object, if you know it will not change, improving performance a little. This is done either way when the target is -self-")]
		public bool cacheTarget = false;

		public override void Created()
		{
			GameGlobal.Create(); // make sure Global is available
			if (target == null) cacheTarget = true; // force caching when target is null (-self-)
			blockIsValid = color != null;
			if (!blockIsValid) Log(LogType.Error, "The color must be set.");
		}

		public override BlockReturn Run(BlockReturn param)
		{
			// Gather the materials containing the color we are changing
			GameObject go = target == null ? owningBlox.gameObject : target.RunAndGetGameObject();
			if (go == null)
			{
				Log(LogType.Error, "The Target is invalid.");
				return BlockReturn.Error;
			}

			// Get the color
			Color c = color.RunAndGetColor();
			// Debug.Log("color: "+c.ToHexStringRGB());

			Renderer[] meshRenderers = go.GetComponentsInChildren<Renderer>();
			Material mat;
			foreach (Renderer r in meshRenderers) {
				// Debug.Log(r.material.name);
				// If material is COLOR2, then that's primary color. Change to the chosen color
				if (r.material.name == "COLOR2 (Instance)") {
					r.material.color = c;
				}
			}


			return BlockReturn.OK;
		}
	}
}