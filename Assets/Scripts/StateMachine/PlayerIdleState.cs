using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerIdleState : PlayerBaseState
    {

        public PlayerIdleState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {

        }

        public override void EnterState() 
        {
            // if there is no input, set the target speed to 0
            Ctx.TargetSpeed = 0.0f;
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

            if(isMoving && !isSprinting)
            {
                SwitchState(Factory.Walk());
            }
            else if(isMoving && isSprinting)
            {
                SwitchState(Factory.Run());
            }
        }
        public override void InitializeSubState() {}
    }

}