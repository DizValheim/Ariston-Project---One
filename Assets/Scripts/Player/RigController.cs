using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Ariston
{
    public class RigController: MonoBehaviour
    {
        [SerializeField] private Rig headRig;
        [SerializeField] private Rig handsRig;
        [SerializeField] private Rig itemRig;
        [SerializeField] private Rig alternateItemRig;
        [SerializeField] private Rig legsRig;
        [SerializeField] private float setupDuration = 0.2f;

        private GameObject leftHandConstraint;
        private GameObject rightHandConstraint;

        private Transform leftHandIKTarget;
        private Transform leftHandIKHint;
        private Transform rightHandIKTarget;
        private Transform rightHandIKHint;

        TwoBoneIKConstraint leftHandIKConstraint = new();
        TwoBoneIKConstraint rightHandIKConstraint = new();

        void Awake()
        {
            leftHandConstraint = handsRig.transform.GetChild(0).gameObject;
            rightHandConstraint = handsRig.transform.GetChild(1).gameObject;

            leftHandIKConstraint = leftHandConstraint.GetComponent<TwoBoneIKConstraint>();
            rightHandIKConstraint = rightHandConstraint.GetComponent<TwoBoneIKConstraint>();
            
            IdleRig();

            leftHandIKTarget = leftHandConstraint.transform.GetChild(0);
            leftHandIKHint = leftHandConstraint.transform.GetChild(1);
            rightHandIKTarget = rightHandConstraint.transform.GetChild(0);
            rightHandIKHint = rightHandConstraint.transform.GetChild(1);
        }

        public void IdleRig()
        {
            headRig.weight = 0;
            handsRig.weight = 0;
            itemRig.weight = 0;
            alternateItemRig.weight = 0;
            // legsRig.weight = 0;
        }

        public void HoldRig(GameObject[] ob)
        {   
            TargetsTransfer(ob);

            itemRig.weight += setupDuration;
            handsRig.weight += setupDuration;
        }

        public void UseRig(bool condition, GameObject[] ob)
        {
            if(condition)
            {
                // Debug.Log("Right mouse pressed");
                
                TargetsTransfer(ob);
                itemRig.weight -= setupDuration;
                alternateItemRig.weight += setupDuration;
                headRig.weight += setupDuration;
            }
            else
            {
                alternateItemRig.weight -= setupDuration;
                headRig.weight -= setupDuration;
                HoldRig(ob);
            }
        }

        private void TargetsTransfer(GameObject[] heldObject)
        {
            leftHandIKTarget.SetPositionAndRotation(heldObject[0].transform.position, heldObject[0].transform.rotation);
            leftHandIKHint.SetPositionAndRotation(heldObject[1].transform.position, heldObject[1].transform.rotation);
            rightHandIKTarget.SetPositionAndRotation(heldObject[2].transform.position, heldObject[2].transform.rotation);
            rightHandIKHint.SetPositionAndRotation(heldObject[3].transform.position, heldObject[3].transform.rotation);
        }
    }
}