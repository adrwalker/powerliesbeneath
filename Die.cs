using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using plyBloxKit;
using com.ootii.Base;
using com.ootii.Cameras;
using com.ootii.Helpers;
using com.ootii.Input;
using com.ootii.Utilities.Debug;

namespace com.ootii.AI.Controllers
{
    /// <summary>
    /// Death animation
    /// </summary>
    [MotionTooltip("Player death animaton")]
    public class Die : MotionControllerMotion
    {
    	// Enum values for the motion
        public const int PHASE_UNKNOWN = 0;
        public const int PHASE_START = 21000;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Die() : base()
        {
            _Priority = 0;
            mIsStartable = true;
        }

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="rController">Controller the motion belongs to</param>
        public Die(MotionController rController)  : base(rController)
        {
            _Priority = 0;
            mIsStartable = true;
        }

        /// <summary>
        /// Preprocess any animator data so the motion can use it later
        /// </summary>
        public override void LoadAnimatorData()
        {
        	mController.AddAnimatorName("Entry -> Base Layer.Die-SM.Death");
        	mController.AddAnimatorName("AnyState -> Base Layer.Die-SM.Death");

        	mController.AddAnimatorName("Base Layer.Die-SM.Death");
        }

        /// <summary>
        /// Tests if this motion should be started. However, the motion
        /// isn't actually started.
        /// </summary>
        /// <returns></returns>
        public override bool TestActivate()
        {
            bool isDead = (bool)plyBloxGlobal.Instance.GetVarValue("plr_dead");
            // Debug.Log(isDead+"|"+mIsStartable);
            // if (isDead && mIsStartable) Debug.Log("<color=#FF0000>Die motion should activate now.</color>");
            return isDead && mIsStartable;
        }

        /// <summary>
        /// Called to start the specific motion. If the motion
        /// were something like 'jump', this would start the jumping process
        /// </summary>
        /// <param name="rPrevMotion">Motion that this motion is taking over from</param>
        public override bool Activate(MotionControllerMotion rPrevMotion)
        {
            // Debug.Log("Activate death motion");
            ControllerState lState = mController.State;
            lState.Stance = EnumControllerStance.DEAD;
            mController.SetAnimatorMotionPhase(mAnimatorLayerIndex, Die.PHASE_START, true);
            return base.Activate(rPrevMotion);
        }

        /// <summary>
        /// Tests if the motion should continue. If it shouldn't, the motion
        /// is typically disabled
        /// </summary>
        /// <returns></returns>
        public override bool TestUpdate() {
            return true;
        }

        /// <summary>
        /// Updates the motion over time. This is called by the controller
        /// every update cycle so animations and stages can be updated.
        /// </summary>
        public override void UpdateMotion() {

        }

        /// <summary>
        /// Called to stop the motion. If the motion is stopable. Some motions
        /// like jump cannot be stopped early
        /// </summary>
        public override void Deactivate() {  

        }

        /// <summary>
        /// Raised when a motion is being interrupted by another motion
        /// </summary>
        /// <param name="rMotion">Motion doing the interruption</param>
        /// <returns>Boolean determining if it can be interrupted</returns>
        public override bool OnInterruption(MotionControllerMotion rMotion)
        {
            return false;
        }
    }
}