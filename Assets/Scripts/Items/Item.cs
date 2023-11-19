using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ariston
{
    /// <summary>
    /// Base item class
    /// </summary>
    public class Item : Interactable , IHoldableItem
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject itemHolder;
        [SerializeField] private GameObject worldObjects;

        private RigController rigController;
        private GameObject targets;
        private int interactCount = 0;
        
        protected override void Awake()
        {
            rigController = playerController.GetComponent<RigController>();
        }

        protected override void Update()
        {
            if(GameInput.Instance.IsInteractPressed)
            {
                // Debug.Log("Use");
                Interact();
                interactCount = 1;                
            }

            if(interactCount == 1)
            {
                rigController.RigSetup3(GameInput.Instance.IsAimPressed, TargetsFinder());
            }
        }

        protected override void Interact()
        {
            // Debug.Log("Interact");
            if(playerController.HeldItem != null)
            {
                targets = ChildFinder(playerController.HeldItem, 1);
                PickUp();
            }
        }

        protected virtual void PickUp() 
        {
            // Debug.Log("Hello Darkness, you're my friend!!!");
            playerController.HeldItem.transform.SetParent(itemHolder.transform);
            playerController.HeldItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            rigController.RigSetup2(TargetsFinder());
        }
       
        public virtual void Drop() {}
        
        public virtual void Use() {}

        private GameObject ChildFinder(GameObject parent, int childIndex)
        {
            Transform child = parent.transform.GetChild(childIndex);
            return child.gameObject;
        }

        private GameObject[] TargetsFinder()
        {
            GameObject[] idleChildren = new GameObject[4];
            for(int i=0; i<4; i++)
            {
                idleChildren[i] = ChildFinder(targets, i);
            }
            return idleChildren;
        }
    }
}