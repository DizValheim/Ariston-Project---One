using System;
using Cinemachine;
using UnityEngine;

namespace Ariston
{
    public class ThirdPersonAimController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerAimVirtualCamera;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private LayerMask layerToHit = new();
        // [SerializeField] private Transform debugTransform;
        private PlayerController playerController;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            //Gets the center point of the screen
            Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
            //Casts a ray from camera to screenCenterPoint
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerToHit)) {
                // debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }

            //Aim mode
            if(GameInput.Instance.IsAimPressed) {
                playerAimVirtualCamera.gameObject.SetActive(true);
                playerController.SetAimSensitivity(aimSensitivity);
                playerController.SetRotationOnAim(false);

                //Setting up the aim direction
                Vector3 worldAimPoint = mouseWorldPosition;
                worldAimPoint.y = transform.position.y;
                Vector3 aimDirection = (worldAimPoint - transform.position).normalized;

                //Rotating player towards aim direction
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 15f);
            }

            //Normal mode
            else {
                playerAimVirtualCamera.gameObject.SetActive(false);
                playerController.SetAimSensitivity(normalSensitivity);
                playerController.SetRotationOnAim(true);
            }
        }
    }
}