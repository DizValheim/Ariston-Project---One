using Ariston.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;


/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace Ariston
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public float interactionRange = 50f;
        public LayerMask HoldableLayer;
        private GameObject heldItem;
        [SerializeField] private PlayerSO playerData;

        [SerializeField] private GameObject cinemachineCameraTarget;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float sensitivityMultiplier = 1f;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private bool rotateOnAim = true;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        private float cameraAngleOverride = 0.0f;

        // For locking the camera position on all axis
        private bool lockCameraPosition = false;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;


        private PlayerInput _playerInput;

        private Animator _animator;
        private CharacterController _controller;
        
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return _playerInput.currentControlScheme == "KeyboardMouse";
            }
        }

        public bool IsGrounded = true;

        //State Variables
        PlayerBaseState _currentState;
        PlayerStateFactory _states;

        //Getters and Setters
        public PlayerBaseState CurrentState {get { return _currentState; } set { _currentState = value;} }
        
        public float VerticalVelocity {get { return _verticalVelocity; } set { _verticalVelocity = value;} }
        public float TerminalVelocity { get { return _terminalVelocity; } }
        public float JumpHeight {get { return playerData.JumpHeight; } }
        public float Gravity { get { return playerData.Gravity;} }
        public float JumpTimeout {get { return playerData.JumpTimeout; } }
        public float FallTimeout {get { return playerData.FallTimeout; } }
        public float MoveSpeed { get { return playerData.MoveSpeed; } }
        public float SprintSpeed { get {return playerData.SprintSpeed; } }
        public float TargetSpeed { get; set;}
        public bool HasAnimator { get { return _hasAnimator;} }
        public Animator Animator { get { return _animator; } }
        public int AnimIDSpeed { get { return _animIDSpeed; } }
        public int AnimIDGrounded { get { return _animIDGrounded; } }
        public int AnimIDJump { get { return _animIDJump; } }
        public int AnimIDFreeFall { get { return _animIDFreeFall; } }
        public int AnimIDMotionSpeed { get { return _animIDMotionSpeed; } }
        public CharacterController CharacterController { get { return _controller; } }
        public float JumpTimeoutDelta {get { return _jumpTimeoutDelta; } set { _jumpTimeoutDelta = value; } }
        public float FallTimeoutDelta {get { return _fallTimeoutDelta; } set { _fallTimeoutDelta = value; } }
        public GameObject HeldItem {get { return heldItem; } }


        public bool RequireNewJumpPress {get; set;}

        #region Unity Functions

        private void OnEnable() 
        {
            GameInput.Instance.PlayerInputActions.Player.Jump.performed+=On_Jump;    
        }

        private void OnDisable() 
        {
            GameInput.Instance.PlayerInputActions.Player.Jump.performed-=On_Jump;    
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();
        }

        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            // _input = GetComponent<GameInput>();
            _playerInput = GetComponent<PlayerInput>();

            AssignAnimationIDs();
            GameInput.Instance.SetCursorLockState(true);

            // reset our timeouts on start
            _jumpTimeoutDelta = playerData.JumpTimeout;
            _fallTimeoutDelta = playerData.FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            _currentState.UpdateStates();
            // Debug.Log("Current State: " + CurrentState);

            // JumpAndGravity();
            GroundedCheck();
            Move();

            //Detecting items
            ItemDetection();
            // Debug.Log(HeldItem.name);
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        #endregion
        
        #region Custom Functions
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - playerData.GroundedOffset,
                transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, playerData.GroundedRadius, playerData.GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, IsGrounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (GameInput.Instance.LookVector.sqrMagnitude >= _threshold && !lockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += GameInput.Instance.LookVector.x * deltaTimeMultiplier * sensitivityMultiplier;
                _cinemachineTargetPitch += GameInput.Instance.LookVector.y * deltaTimeMultiplier * sensitivityMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, playerData.BottomClamp, playerData.TopClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            Vector2 movementInputVector = GameInput.Instance.MovementVector;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            // float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            float inputMagnitude = movementInputVector.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < TargetSpeed - speedOffset ||
                currentHorizontalSpeed > TargetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, TargetSpeed * inputMagnitude,
                    Time.deltaTime * playerData.SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = TargetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, TargetSpeed, Time.deltaTime * playerData.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(movementInputVector.x, 0.0f, movementInputVector.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (movementInputVector != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    playerData.RotationSmoothTime);

                // rotate to face input direction relative to camera position
                if(rotateOnAim) {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }


        #endregion

        #region Event Subscribers

        private void On_Jump(InputAction.CallbackContext context)
        {
            RequireNewJumpPress = false;
        }

        #endregion

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - playerData.GroundedOffset, transform.position.z),
                playerData.GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f)
            // {
            //     if (FootstepAudioClips.Length > 0)
            //     {
            //         var index = Random.Range(0, FootstepAudioClips.Length);
            //         AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            //     }
            // }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f)
            // {
            //     AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            // }
        }

        
        public void SetAimSensitivity(float newSensitivity)
        {
            sensitivityMultiplier = newSensitivity;
        }

        public void SetRotationOnAim(bool newRotateOnAim)
        {
            rotateOnAim = newRotateOnAim;
        }

        public void ItemDetection()
        {
            // Debug.Log("Detecting...");
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, HoldableLayer);

            foreach (Collider detectedLayer in colliders)
            {
                heldItem = detectedLayer.gameObject;
                // Debug.Log(heldItem.name);
            }
        }
    }
}