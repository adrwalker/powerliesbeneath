// -= Power Lies Beneath Source =-
// www.powerliesbeneath.com
// Adrian Walker
// ====================================================================================================================
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
	public class SwordForward : MotionControllerMotion {

		// Enum values for the motion
		public const int PHASE_UNKNOWN = 0;
		public const int PHASE_START = 20400;

        // Number of degrees we'll accelerate and decelerate by
        // in order to reach the rotation target
        [SerializeField]
        protected float mRotationAcceleration = 12.0f;

        [MotionTooltip("Determines how quickly the avatar will start rotating or stop rotating.")]
        public float RotationAcceleration
        {
            get { return mRotationAcceleration; }
            set { mRotationAcceleration = value; }
        }

        // Current yaw we're rotating towards
        private float mYaw = 0f;

        // Default constructor
        public SwordForward() : base()
        {
            _Priority = 1;
            mIsStartable = true;
            mIsGroundedExpected = true;
        }

        // Controller constructor
        public SwordForward(MotionController rController) : base(rController)
        {
            _Priority = 1;
            mIsStartable = true;
            mIsGroundedExpected = true;
        }

        // Preprocess any animator data so the motion can use it later
        public override void LoadAnimatorData() {
        	mController.AddAnimatorName("Entry -> Base Layer.SwordForward-SM.Forward");

        	mController.AddAnimatorName("AnyState -> Base Layer.SwordForward-SM.Forward");
            mController.AddAnimatorName("AnyState -> Base Layer.SwordForward-SM.Forward (fem)");
        	mController.AddAnimatorName("Base Layer.SwordForward-SM.Forward");
            mController.AddAnimatorName("Base Layer.SwordForward-SM.Forward (fem)");
        }

        // Tests if this motion should be started. However, the motion isn't actually started.
        public override bool TestActivate() {
        	// We let the ExplorationRun take over if we're in the traversal stance and grounds.
        	// There must be an attempt to move the avatar with some input/AI
            if (!mIsStartable) { return false; }
            if (!mController.IsGrounded) { return false; }

            ControllerState lState = mController.State;
            if (lState.InputMagnitudeTrend.Value < 0.1f) { return false; }
            if (lState.InputX == 0 && lState.InputY == 0 && lState.MouseX == 0) { return false; }
            
            if (lState.Stance != EnumControllerStance.COMBAT_MELEE) { return false; }
                
            return true;
        }

        // Tests if the motion should continue. If it shouldn't, the motion
        // is typically disabled
        public override bool TestUpdate() {
        	if (mIsActivatedFrame) { return true; }

            if (!IsInRunState && mController.GetAnimatorMotionPhase(mMotionLayer.AnimatorLayerIndex) != SwordForward.PHASE_START) { return false; }

            if (!mController.IsGrounded) { return false; }

            ControllerState lState = mController.State;
            if (lState.InputMagnitudeTrend.Average == 0f) { return false; }
            
            if (lState.Stance != EnumControllerStance.COMBAT_MELEE) { return false; }

            return true;
        }

        // Called to start the specific motion. If the motion
        // were something like 'jump', this would start the jumping process
        public override bool Activate(MotionControllerMotion rPrevMotion) {
        	// Store the last camera mode and force it to a fixed view.
            // We do this to always keep the camera behind the player
            if (mController.UseInput && mController.CameraRig != null)
            {
                mController.CameraRig.TransitionToMode(EnumCameraMode.THIRD_PERSON_FOLLOW);
            }

            // It's possible we're activating from a small fall or other
            // skip while already in this motion. If so, no need to restart it.
            if (!IsInRunState)
            {
                // Trigger the change in the animator
                mController.SetAnimatorMotionPhase(mMotionLayer.AnimatorLayerIndex, SwordForward.PHASE_START, true);
            }

            return base.Activate(rPrevMotion);
        }

        // Called to stop the motion. If the motion is stopable. Some motions
        // like jump cannot be stopped early
        public override void Deactivate()
        {
            base.Deactivate();
        }

        // Updates the motion over time. This is called by the controller
        // every update cycle so animations and stages can be updated.
        public override void UpdateMotion() {
        	if (!TestUpdate())
            {
                Deactivate();
                return;
            }

            // If we're blocked, we're going to modify the speed in order to blend into and out of a stop
            if (mController.State.IsForwardPathBlocked)
            {
                float lAngle = Vector3.Angle(mController.State.ForwardPathBlockNormal, mController.transform.forward);

                float lDiff = 180f - lAngle;
                float lSpeed = mController.State.InputMagnitudeTrend.Value * (lDiff / mController.ForwardBumperBlendAngle);

                mController.State.InputMagnitudeTrend.Replace(lSpeed);
            }
            // Debug.Log(mController.GetAnimatorStateName(mMotionLayer.AnimatorLayerIndex) + " | "
            //     + mController.GetAnimatorStateTransitionName(mMotionLayer.AnimatorLayerIndex));
            DetermineAngularVelocity();
        }

        // Test to see if we're currently in the locomotion state
        public bool IsInRunState {
        	get {
        		string lState = mController.GetAnimatorStateName(mMotionLayer.AnimatorLayerIndex);
                string lTransition = mController.GetAnimatorStateTransitionName(mMotionLayer.AnimatorLayerIndex);

                // Do a simple test for the substate name
                if (lState.Length == 0) { return false; }
                if (lState.IndexOf("SwordForward-SM") >= 0 || lTransition.IndexOf("SwordForward-SM") >= 0)
                {
                    return true;
                }

                return false;
        	}
        }

        // Returns the current angular velocity of the motion
        protected override Vector3 DetermineAngularVelocity()
        {
            float lView = ootiiInputStub.ViewX;
            float lMovement = ootiiInputStub.MovementX;


            float lViewTarget = lView * mController.RotationSpeed;

            // We want to work our way to the goal smoothly
            if (mYaw < lViewTarget)
            {
                mYaw += mRotationAcceleration;
                if (mYaw > lViewTarget) { mYaw = lViewTarget; }
            }
            else if (mYaw > lViewTarget)
            {
                mYaw -= mRotationAcceleration;
                if (mYaw < lViewTarget) { mYaw = lViewTarget; }
            }

            // Assign the current rotation
            mAngularVelocity.y = mYaw;

            // Return the results
            return mAngularVelocity;
        }

        // Allows the motion to modify the velocity before it is applied.
        public override void CleanRootMotion(ref Vector3 rVelocityDelta, ref Quaternion rRotationDelta)
        {
            string lState = mController.GetAnimatorStateName(mMotionLayer.AnimatorLayerIndex);
            if (lState.IndexOf("Forward (fem)") >= 0 && mController.State.InputY > 0f) {
                rVelocityDelta.z = mController.State.InputY * 10;
                return;
            }
            // Remove any x movement. This will prevent swaying
            if (mController.State.InputX == 0f) {
                rVelocityDelta.x = 0f;
            }

            // Make this motion move a little faster
            if (rVelocityDelta.x > 0f) {
                rVelocityDelta.x = rVelocityDelta.x * 2;
            }
            if (rVelocityDelta.z > 0f) {
                rVelocityDelta.z = rVelocityDelta.z * 3;
            }

            // No automatic rotation in this motion
            rRotationDelta = Quaternion.identity;
        }

        // Test to see if we're currently pivoting
        public bool IsInPivotState
        {
        	get {
        		return false;
        	}
        }
	}
}
