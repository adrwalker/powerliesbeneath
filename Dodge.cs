using System.Collections.Generic;
using System.Text;
using UnityEngine;
using com.ootii.Base;
using com.ootii.Cameras;
using com.ootii.Helpers;
using com.ootii.Input;
using plyGame;
using com.ootii.Utilities.Debug;

namespace com.ootii.AI.Controllers {
	/// <summary>
    /// Handles the basic motion for making the avatar dodge left, right, and backwards
    /// crouch position and moving them from the crouch to idle.
    /// </summary>
    [MotionTooltip("Allows the avatar to dodge by rolling to the side or backwards")]
	public class Dodge : MotionControllerMotion {
		// Enum values for the motion
        public const int PHASE_UNKNOWN = 0;
        public const int PHASE_START = 20600;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Dodge()
            : base()
        {
            _Priority = 10;
            mIsStartable = true;
        }

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="rController">Controller the motion belongs to</param>
        public Dodge(MotionController rController)
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
        	mController.AddAnimatorName("Entry -> Base Layer.Dodge-SM.roll_back");
        	mController.AddAnimatorName("AnyState -> Base Layer.Dodge-SM.roll_back");
            mController.AddAnimatorName("AnyState -> Base Layer.Dodge-SM.roll");
        	mController.AddAnimatorName("AnyState -> Base Layer.Dodge-SM.roll_side_L");
        	mController.AddAnimatorName("AnyState -> Base Layer.Dodge-SM.roll_side_R");
        	mController.AddAnimatorName("Base Layer.Dodge-SM.roll_back");
            mController.AddAnimatorName("Base Layer.Dodge-SM.roll");
        	mController.AddAnimatorName("Base Layer.Dodge-SM.roll_side_L");
        	mController.AddAnimatorName("Base Layer.Dodge-SM.roll_side_R");
        } 

        /// <summary>
        /// Tests if this motion should be started. However, the motion
        /// isn't actually started.
        /// </summary>
        /// <returns></returns>
        public override bool TestActivate()
        {
            if (mController.State.Stance == EnumControllerStance.DEAD) { return false; }
            if (ootiiInputStub.IsJustPressed("Dodge"))
            {
                return true;
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
            mController.SetAnimatorMotionPhase(mAnimatorLayerIndex, Dodge.PHASE_START, true);
            // Set the ground velocity so that we can keep momentum going
            ControllerState lState = mController.State;
            lState.GroundLaunchVelocity = (mController.transform.rotation * mController.RootMotionVelocityAvg.Average);
            mController.State = lState;

            // Continue with the activation
            return base.Activate(rPrevMotion);
        }      

        /// <summary>
        /// Tests if the motion should continue. If it shouldn't, the motion
        /// is typically disabled
        /// </summary>
        /// <returns></returns>
        public override bool TestUpdate() {
            if (mController.State.Stance == EnumControllerStance.DEAD) { return false; }
            if (mIsActivatedFrame) { return true; }

            if (IsInDodgeState) {
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
            // Start the motion from the beginning if dodge key is pressed
            if (TestActivate()) {
                mController.SetAnimatorMotionPhase(mAnimatorLayerIndex, Dodge.PHASE_START, true);
            }

            if (!TestUpdate()) {
                Deactivate();
            }

            // Determine the resulting velocity of this update
            DetermineVelocity();
        }

        /// <summary>
        /// Called to stop the motion. If the motion is stopable. Some motions
        /// like jump cannot be stopped early
        /// </summary>
        public override void Deactivate()
        {
            mPhase = Dodge.PHASE_UNKNOWN;

            mIsActive = false;
            mIsStartable = true;
            mDeactivationTime = Time.time;

            mVelocity = Vector3.zero;
        }

        /// <summary>
        /// Raised when a motion is being interrupted by another motion
        /// </summary>
        /// <param name="rMotion">Motion doing the interruption</param>
        /// <returns>Boolean determining if it can be interrupted</returns>
        public override bool OnInterruption(MotionControllerMotion rMotion)
        {
            bool canInterrupt = mController.State.AnimatorStates[mAnimatorLayerIndex].StateInfo.normalizedTime > 0.8f || 
                rMotion.Name == "sword attack" || rMotion.Name == "die" || rMotion.Name == "element beam";
            Debug.Log(rMotion.Name);
            return canInterrupt;
        }

        /// <summary>
        /// Returns the current velocity of the motion
        /// </summary>
        protected override Vector3 DetermineVelocity()
        {
            ControllerState lState = mController.State;
            if (IsInDodgeState) {
                Vector3 lBaseForward = mController.CameraTransform.forward;

                // Direction of the camera
                Vector3 lCameraForward = lBaseForward;
                lCameraForward.y = 0f;
                lCameraForward.Normalize();

                // Create a quaternion that gets us from our world-forward to our camera direction.
                // FromToRotation creates a quaternion using the shortest method which can sometimes
                // flip the angle. LookRotation will attempt to keep the "up" direction "up".
                Quaternion lFromCamera = Quaternion.LookRotation(lCameraForward);

                // Determine the avatar displacement direction. This isn't just
                // normal movement forward, but includes movement to the side
                Vector3 lMoveDirection = lFromCamera * lState.InputForward;

                // Determine the max air speed
                Vector3 lMomentum = lState.GroundLaunchVelocity;

                // Determine the air speed. We want the max of the momentum or control
                // speed. This gives us smooth movement while running and jumping
                float lControlSpeed = 0f;

                float lAirSpeed = lMomentum.magnitude;
                lAirSpeed = Mathf.Max(lAirSpeed, lControlSpeed);

                // Combine our control velocity with momentum
                Vector3 lAirVelocity = Vector3.zero;

                // When on the ground, continue with our momentum
                if (lState.IsGrounded)
                {
                    lAirVelocity += mController.transform.forward * lAirSpeed;
                }
                // While in the air, we have a speed based the max of our momentum or control speed
                else
                {
                    // If momementum is enabled, add it to keep the player moving in the direction of the jump
                    lAirVelocity += lMomentum;
                }

                // Don't exceed our air speed
                if (lAirVelocity.magnitude > lAirSpeed)
                {
                    lAirVelocity = lAirVelocity.normalized * lAirSpeed;
                }

                // Return the final velocity
                mVelocity = lAirVelocity;
            }
            return mVelocity;
        }

        /// <summary>
        /// Test to see if we're currently in the locomotion state
        /// </summary>
        public bool IsInDodgeState
        {
            get
            {
                string lState = mController.GetAnimatorStateName(mAnimatorLayerIndex);
                string lTransition = mController.GetAnimatorStateTransitionName(mAnimatorLayerIndex);

                if (lTransition == "Entry -> Base Layer.Dodge-SM.roll_back" || 
                    lTransition == "AnyState -> Base Layer.Dodge-SM.roll_back" ||
                    lTransition == "AnyState -> Base Layer.Dodge-SM.roll" ||
                    lTransition == "AnyState -> Base Layer.Dodge-SM.roll_side_L" ||
                    lTransition == "AnyState -> Base Layer.Dodge-SM.roll_side_R" ||
                    lState == "Base Layer.Dodge-SM.roll_back" ||
                    lState == "Base Layer.Dodge-SM.roll" ||
                    lState == "Base Layer.Dodge-SM.roll_side_L" ||
                    lState == "Base Layer.Dodge-SM.roll_side_R" ) {
                    return true;
                }
                return false;
            }
        }
	}
}