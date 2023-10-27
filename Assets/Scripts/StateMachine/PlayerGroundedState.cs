using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerGroundedState : PlayerBaseState
    {

        public PlayerGroundedState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState() 
        {
            
        }
        public override void UpdateState() 
        {
            HandleGravityError();
            HandleAnimation();
            // HandleTimeout();


            CheckSwitchState();
        }
        public override void ExitState() 
        {

        }
        public override void InitializeSubState() 
        {

            bool isMoving = GameInput.Instance.MovementVector != Vector2.zero;
            bool isSprinting = GameInput.Instance.IsSprintPressed;

            if(!isMoving  && !isSprinting)
            {
                SetSubState(Factory.Idle());
            }
            else if(isMoving && !isSprinting)
            {
                SetSubState(Factory.Walk());
            }
            else
            {
                SetSubState(Factory.Run());
            }
        }
        public override void CheckSwitchState()
        {
            //If Player is grounded and jump is pressed, switch to jump state
            if(!Ctx.IsGrounded)
            {
                SwitchState(Factory.Fall());
            }
            else if(GameInput.Instance.IsJumpPressed && !Ctx.RequireNewJumpPress)
            {
                SwitchState(Factory.Jump());
            }
            
            
        }

        private void HandleGravityError()
        {
            if (Ctx.VerticalVelocity <= 0)
            {
                Ctx.VerticalVelocity = -2f;
            }
        }

        private void HandleTimeout()
        {
            // reset the fall timeout timer
            Ctx.FallTimeoutDelta = Ctx.FallTimeout;

            // jump timeout
            if (Ctx.JumpTimeoutDelta >= 0.0f)
            {
                Ctx.JumpTimeoutDelta -= Time.deltaTime;
            }
        }

        void HandleAnimation()
        {
            // update animator if using character
            Ctx.Animator.SetBool(Ctx.AnimIDJump, false);
            Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, false);
        }

        
    }

}