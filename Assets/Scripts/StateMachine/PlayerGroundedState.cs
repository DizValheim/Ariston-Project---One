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
        }

        public override void EnterState() 
        {
            InitializeSubState();
        }
        public override void UpdateState() 
        {
            HandleGravityError();
            Move();
            HandleAnimation();
            
            CheckSwitchState();
        }
        public override void ExitState() 
        {

        }
        public override void InitializeSubState() 
        {

            bool isMoving = GameInput.Instance.MovementVector != Vector2.zero;
            bool isSprinting = GameInput.Instance.IsSprintPressed;

            if(!isMoving)
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
            if(CheckLadder())
            {
                SwitchState(Factory.Climb());
            }
            else if(!Ctx.IsGrounded)
            {
                SwitchState(Factory.Fall());
            }
            //If Player is grounded and jump is pressed, switch to jump state
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

        void HandleAnimation()
        {
            // update animator if using character
            Ctx.Animator.SetBool(Ctx.AnimIDJump, false);
            Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, false);
            Ctx.Animator.SetBool(Ctx.AnimIDClimb, false);
        }

        void Move()
        {
            Ctx.CharacterController.Move(Ctx.TargetDirection.normalized * (Ctx.Speed * Time.deltaTime) + new Vector3(0.0f, Ctx.VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        bool CheckLadder()
        {
            float avoidFloorDistance = 0.1f;
            float ladderGrabDistance = 0.4f;
            if(Physics.Raycast(Ctx.transform.position + Vector3.up * avoidFloorDistance, Ctx.TargetDirection, out RaycastHit raycastHit, ladderGrabDistance))
            {
                if(raycastHit.transform.TryGetComponent(out Ladder ladder))                    
                {
                    return true;
                }
            }
            return false;
        }

        
    }

}