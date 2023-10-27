using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerRunState : PlayerBaseState
    {

        public PlayerRunState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {

        }

        public override void EnterState() 
        {
            Ctx.Animator.SetBool(Ctx.AnimIDSpeed, false);
            Ctx.TargetSpeed = Ctx.SprintSpeed;
        }
        public override void UpdateState() 
        {
            CheckSwitchState();
        }
        public override void ExitState() {}
        public override void CheckSwitchState() 
        {
            bool isMoving = GameInput.Instance.MovementVector != Vector2.zero;
            bool isSprinting = GameInput.Instance.IsSprintPressed;

            if(!isMoving  && !isSprinting)
            {
                SwitchState(Factory.Idle());
            }
            else if(isMoving && !isSprinting)
            {
                SwitchState(Factory.Walk());
            }
        }
        public override void InitializeSubState() {}
    }

}