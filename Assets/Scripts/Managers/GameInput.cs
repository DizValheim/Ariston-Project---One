using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Ariston
{
	public class GameInput : Singleton<GameInput>
	{

        #region Variables

        public PlayerInputActions PlayerInputActions { get; private set; }

        public event Action OnJumpButtonPressed;
        public event Action OnSprintButtonPressed;
        
        #endregion

        #region Unity Functions

        protected override void Awake()
        {
            base.Awake();

            PlayerInputActions = new PlayerInputActions();
            PlayerInputActions.Player.Enable();

        }

        private void OnEnable() 
        {
            PlayerInputActions.Player.Jump.performed += On_Jump;
            PlayerInputActions.Player.Sprint.performed += On_Sprint;   
        }

        private void OnDisable() 
        {
            PlayerInputActions.Player.Jump.performed -= On_Jump;
            PlayerInputActions.Player.Sprint.performed -= On_Sprint;  
        }

        #endregion


        #region Custom Functions

        public Vector2 MovementVector => PlayerInputActions.Player.Move.ReadValue<Vector2>();


        public Vector2 LookVector => PlayerInputActions.Player.Look.ReadValue<Vector2>();

        public Vector2 MousePosition => Mouse.current.position.ReadValue();

        public bool IsSprintPressed => PlayerInputActions.Player.Sprint.IsPressed();

        public bool IsJumpPressed => PlayerInputActions.Player.Jump.IsPressed();

        #region Events
        private void On_Jump(InputAction.CallbackContext context)
        {
            OnJumpButtonPressed?.Invoke();
        }

        private void On_Sprint(InputAction.CallbackContext context)
        {
            OnSprintButtonPressed?.Invoke();
        }
        
        #endregion

        #endregion

	}
	
}