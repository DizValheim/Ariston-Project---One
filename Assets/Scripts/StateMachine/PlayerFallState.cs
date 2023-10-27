using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void EnterState() 
        {
            InitializeSubState();
        }
        public override void UpdateState() 
        {
            HandleGravity();
            HandleAnimation();

            CheckSwitchState();
        }
        public override void ExitState() {}
        public override void CheckSwitchState() 
        {
            if(Ctx.IsGrounded)
                SwitchState(Factory.Grounded());
        }
        public override void InitializeSubState() 
        {
            bool isMoving = GameInput.Instance.MovementVector != Vector2.zero;
            bool isSprinting = GameInput.Instance.IsSprintPressed;

            if (!isMoving)
            {
                SetSubState(Factory.Idle());
            }
            else if (isMoving && !isSprinting)
            {
                SetSubState(Factory.Walk());
            }
            else if (isMoving && isSprinting)
            {
                SetSubState(Factory.Run());
            }
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
            Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, true);
        }
    }
}
