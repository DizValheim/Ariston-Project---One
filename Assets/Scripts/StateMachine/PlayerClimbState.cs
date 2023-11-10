using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerClimbState : PlayerBaseState
    {
        private Vector3 lastGrabLadderDirection;
        
        public PlayerClimbState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void EnterState() 
        {
            // Ctx.Animator.SetBool(Ctx.AnimIDAtTop, false);
            lastGrabLadderDirection = Ctx.TargetDirection;
            InitializeSubState();
        }
        public override void UpdateState() 
        {
            CheckGrounded();
            HandleLogic();
            HandleAnimation();

            CheckSwitchState();

        }
        public override void ExitState() 
        {
            Ctx.VerticalVelocity = 4f;
        }
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

        void HandleLogic()
        {
            Ctx.transform.rotation = Quaternion.identity;
            Ctx.TargetDirection = new Vector3(0f, Ctx.TargetDirection.z, 0f);
            Ctx.VerticalVelocity = 0f;

            Ctx.Speed = Ctx.TargetSpeed;
            
            // Debug.Log("IsGrounded: " + Ctx.IsGrounded);
            Ctx.CharacterController.Move(Ctx.TargetDirection.normalized * (Ctx.Speed * Time.deltaTime) + new Vector3(0.0f, Ctx.VerticalVelocity, 0.0f) * Time.deltaTime);
            
        }

        void HandleAnimation()
        {
            Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, false);
            Ctx.Animator.SetBool(Ctx.AnimIDClimb, true);
        }

        void CheckGrounded()
        {
            float avoidFloorDistance = 0.5f;
            float ladderGrabDistance = 0.4f;
            if(Physics.Raycast(Ctx.transform.position + Vector3.up * avoidFloorDistance, lastGrabLadderDirection, out RaycastHit raycastHit, ladderGrabDistance))
            {
                if(!raycastHit.transform.TryGetComponent(out Ladder ladder))
                {
                    Ctx.IsGrounded = true;
                    // Ctx.Animator.SetBool(Ctx.AnimIDAtTop, true);
                }
            }
            else
            {
                Ctx.IsGrounded = true;  
                // Ctx.Animator.SetBool(Ctx.AnimIDAtTop, true);                             
            }
            if(Vector3.Dot(Ctx.TargetDirection, lastGrabLadderDirection) < 0f)
            {
                //Climbing down the ladder
                float ladderFloorDropDistance = 0.1f;
                if(Physics.Raycast(Ctx.transform.position, Vector3.down, out RaycastHit floorRaycastHit, ladderFloorDropDistance))
                {
                    Ctx.IsGrounded = true;
                    
                }
            }
            
        }
    }
}
