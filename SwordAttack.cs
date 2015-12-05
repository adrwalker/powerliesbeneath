using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using com.ootii.Base;
using com.ootii.Cameras;
using com.ootii.Helpers;
using com.ootii.Input;
using com.ootii.Utilities.Debug;

namespace com.ootii.AI.Controllers
{
    /// <summary>
    /// Attack with sword while stationary or while running
    /// </summary>
    [MotionTooltip("A Layer1 animation for different sword attacks")]
    public class SwordAttack : MotionControllerMotion
    {
    	// Enum values for the motion
        public const int PHASE_UNKNOWN = 0;
        public const int PHASE_START = 20700;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SwordAttack()
            : base()
        {
            _Priority = 10;
            mIsStartable = true;
        }

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="rController">Controller the motion belongs to</param>
        public SwordAttack(MotionController rController)
            : base(rController)
        {
            _Priority = 10;
            mIsStartable = true;
        }

		/// <summary>
        /// Preprocess any animator data so the motion can use it later
        /// </summary>
        public override void LoadAnimatorData()
        {
        	mController.AddAnimatorName("Entry -> Base Layer.SwordAttack-SM.sword_slash1");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash1");
        	mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash2");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash2.5");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash3");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash4");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordAttack-SM.sword_slash5");

        	mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash1");
            mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash2.5");
            mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash3");
            mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash2");
            mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash4");
            mController.AddAnimatorName("Base Layer.SwordAttack-SM.sword_slash5");
        }

        /// <summary>
        /// Tests if this motion should be started. However, the motion
        /// isn't actually started.
        /// </summary>
        /// <returns></returns>
        public override bool TestActivate()
        {
            if (mController.UseInput && ootiiInputStub.IsJustPressed("ClickInteract"))
            {
                // Grab the state name from the first active state we find
                string lStateName = mController.GetAnimatorStateName();

                // Grab the controller state to test Stance
                ControllerState lState = mController.State;

                // Ensure we're not currently climbing and that we're in combat stance
                if (!lStateName.Contains("ClimbCrouch-SM") && 
                	lState.Stance == EnumControllerStance.COMBAT_MELEE)
                {
                    return true;
                }
            }            

            // Get out
            return false;
        }

        /// <summary>
        /// Called to start the specific motion. If the motion
        /// were something like 'jump', this would start the jumping process
        /// </summary>
        /// <param name="rPrevMotion">Motion that this motion is taking over from</param>
        public override bool Activate(MotionControllerMotion rPrevMotion)
        {
            int swordAttackAnimClipNumber = UnityEngine.Random.Range(0, 6);
            mController.SetAnimatorMotionPhase(mAnimatorLayerIndex, (SwordAttack.PHASE_START + swordAttackAnimClipNumber), true);
            return base.Activate(rPrevMotion);
        }

        /// <summary>
        /// Tests if the motion should continue. If it shouldn't, the motion
        /// is typically disabled
        /// </summary>
        /// <returns></returns>
        public override bool TestUpdate() {
            if (mIsActivatedFrame) { return true; }

            if (IsInAttackState) {
                if (mController.State.AnimatorStates[mAnimatorLayerIndex].StateInfo.normalizedTime > 1f) { 
                return false; }
            }

            return true;
        }

        /// <summary>
        /// Updates the motion over time. This is called by the controller
        /// every update cycle so animations and stages can be updated.
        /// </summary>
        public override void UpdateMotion() {
            if (!TestUpdate()) {
                Deactivate();
            }
        }

        /// <summary>
        /// Called to stop the motion. If the motion is stopable. Some motions
        /// like jump cannot be stopped early
        /// </summary>
        public override void Deactivate()
        {
            mPhase = SwordAttack.PHASE_UNKNOWN;

            mIsActive = false;
            mIsStartable = true;
            mDeactivationTime = Time.time;

            mVelocity = Vector3.zero;
        }

		/// <summary>
        /// Test to see if we're currently in the locomotion state
        /// </summary>
        public bool IsInAttackState
        {
            get
            {
                string lState = mController.GetAnimatorStateName(mAnimatorLayerIndex);
                string lTransition = mController.GetAnimatorStateTransitionName(mAnimatorLayerIndex);

                if (lTransition == "Entry -> Base Layer.SwordAttack-SM.sword_slash1" || 
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash1" ||
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash2" ||
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash2.5" ||
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash3" ||
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash4" ||
                    lTransition == "AnyState -> Base Layer.SwordAttack-SM.sword_slash5" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash1" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash2" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash2.5" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash3" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash4" ||
                    lState == "Base Layer.SwordAttack-SM.sword_slash5") {
                    return true;
                }
                return false;
            }
        }
    }
}