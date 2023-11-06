using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {
            IsRootState = true;
        }
        public override void EnterState() 
        {
            InitializeSubState();
            HandleJump();
        }
        public override void UpdateState() 
        {
            HandleGravity();
            HandleAnimation();

            CheckSwitchState();
        }
        public override void ExitState() 
        {
            if(GameInput.Instance.IsJumpPressed)
            {
                Ctx.RequireNewJumpPress = true;
            }
        }
        public override void CheckSwitchState() 
        {
            if(Ctx.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
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

        void HandleJump()
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            Ctx.VerticalVelocity = Mathf.Sqrt(Ctx.JumpHeight * -2f * Ctx.Gravity);
            
            // // Ctx.VerticalVelocity = Ctx.JumpHeight;
    
        }
        void HandleGravity()
        {
            if(Ctx.VerticalVelocity < Ctx.TerminalVelocity)
            {
                Ctx.VerticalVelocity += Ctx.Gravity * Time.deltaTime; 
            }
        }

        void HandleAnimation()
        {
            
            Ctx.Animator.SetBool(Ctx.AnimIDJump, true);
            
        }
    }

}