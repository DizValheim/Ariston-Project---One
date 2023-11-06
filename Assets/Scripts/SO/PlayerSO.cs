using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston.ScriptableObjects
{
    [CreateAssetMenu(fileName ="Player Data", menuName ="Player/Player Data")]
    public class PlayerSO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float sprintSpeed = 5.335f;
        [SerializeField] private float speedChangeRate = 10.0f;
        [SerializeField] private float jumpHeight = 1.2f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime = 0.12f;

        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float fallTimeout = 0.15f;



        [Header("Look")]
        [Tooltip("How far in degrees can you move the camera up")]
        [SerializeField] private float topClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        [SerializeField] private float bottomClamp = -30.0f;



        [Header("Grounded Check")]
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float gravity = -15.0f;
        [Tooltip("Useful for rough ground")]
        [SerializeField] private float groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private float groundedRadius = 0.28f;


        //Audio Section:
        // public AudioClip LandingAudioClip;
        // public AudioClip[] FootstepAudioClips;
        // [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        #region Properties

        //Movement
        public float MoveSpeed => moveSpeed;
        public float SprintSpeed => sprintSpeed;
        public float SpeedChangeRate => speedChangeRate;
        public float RotationSmoothTime => rotationSmoothTime;

        //Jump
        public float JumpHeight => jumpHeight;
        public float JumpTimeout => jumpTimeout;
        public float FallTimeout => fallTimeout;

        //Look
        public float TopClamp => topClamp;
        public float BottomClamp => bottomClamp;

        //Grounded Check
        public LayerMask GroundLayers => groundLayers;
        public float Gravity => gravity;
        public float GroundedOffset => groundedOffset;
        public float GroundedRadius => groundedRadius;
        
        #endregion
        
    }

}