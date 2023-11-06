using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Ariston
{
    public class RigController: MonoBehaviour
    {
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject leftUpperArm;
        [SerializeField] private GameObject leftLowerArm;
        [SerializeField] private GameObject rightUpperArm;
        [SerializeField] private GameObject rightLowerArm;
        [SerializeField] private GameObject legs;

        private Transform leftUpperArmIKTarget;
        private Transform leftUpperArmIKHint;
        private Transform rightUpperArmAim;
        private Transform rightLowerArmAim;

        TwoBoneIKConstraint leftUpperArmIKConstraint = new();
        MultiAimConstraint rightUpperArmAimConstraint = new();
        MultiAimConstraint rightLowerArmAimConstraint = new();

        void Awake()
        {
            leftUpperArmIKConstraint = leftUpperArm.GetComponent<TwoBoneIKConstraint>();
            rightUpperArmAimConstraint = rightUpperArm.GetComponent<MultiAimConstraint>();
            rightLowerArmAimConstraint = rightLowerArm.GetComponent<MultiAimConstraint>();
            
            RigReset();

            leftUpperArmIKTarget = leftUpperArm.transform.GetChild(0);
            leftUpperArmIKHint = leftUpperArm.transform.GetChild(1);
            rightUpperArmAim = rightUpperArm.transform.GetChild(0);
            rightLowerArmAim = rightLowerArm.transform.GetChild(0);
        }

        public void RigSetup(GameObject[] ob)
        {   
            TargetsTransfer(ob);

            leftUpperArmIKConstraint.weight = 1;
            rightUpperArmAimConstraint.weight = 1;
            rightLowerArmAimConstraint.weight = 1;
        }

        public void RigReset()
        {
            leftUpperArmIKConstraint.weight = 0;
            rightUpperArmAimConstraint.weight = 0;
            rightLowerArmAimConstraint.weight = 0;
        }

        private void TargetsTransfer(GameObject[] ob)
        {
            leftUpperArmIKTarget.SetPositionAndRotation(ob[0].transform.position, ob[0].transform.rotation);
            leftUpperArmIKHint.SetPositionAndRotation(ob[1].transform.position, ob[1].transform.rotation);
            rightUpperArmAim.SetPositionAndRotation(ob[2].transform.position, ob[2].transform.rotation);
            rightLowerArmAim.SetPositionAndRotation(ob[3].transform.position, ob[3].transform.rotation);
        }
    }
}