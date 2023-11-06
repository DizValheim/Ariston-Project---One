using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState (PlayerController currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
        {

        }
        public override void EnterState() 
        {
            Ctx.TargetSpeed = Ctx.MoveSpeed;
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

            if(!isMoving)
            {
                SwitchState(Factory.Idle());
            }
            else if(isMoving && isSprinting)
            {
                SwitchState(Factory.Run());
            }
        }
        public override void InitializeSubState() {}
    }

}