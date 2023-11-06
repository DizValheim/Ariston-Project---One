using Unity.VisualScripting;
using UnityEngine;

namespace Ariston
{
    /// <summary>
    /// Base item class
    /// </summary>
    public class Item : Interactable , IHoldableItem
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject itemHolder;

        private RigController rigController;
        private GameObject targets, idleTargets;

        protected override void Awake()
        {
            rigController = playerController.GetComponent<RigController>();
        }

        protected override void Update()
        {
            if(GameInput.Instance.IsUsePressed)
            {
                // Debug.Log("Use");
                Interact();
            }
        }

        protected override void Interact()
        {
            // Debug.Log("Interact");
            targets = ChildFinder(playerController.HeldItem, 1);
            PickUp();
        }

        protected virtual void PickUp() 
        {
            // Debug.Log("Hello Darkness, you're my friend!!!");
            playerController.HeldItem.transform.SetParent(itemHolder.transform);
            playerController.HeldItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            idleTargets = ChildFinder(targets, 0);
            GameObject[] idleChildren = new GameObject[4];
            for(int i=0; i<4; i++)
            {
                idleChildren[i] = ChildFinder(idleTargets, i);
            }

            rigController.RigSetup(idleChildren);
        }
       
        public virtual void Drop() {}
        
        public virtual void Use() {}

        private GameObject ChildFinder(GameObject parent, int childIndex)
        {
            Transform child = parent.transform.GetChild(childIndex);
            return child.gameObject;
        }

    }
}