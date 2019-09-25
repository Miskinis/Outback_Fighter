using UnityEngine;

namespace MecanimBehaviors
{
    public class ZeroWeightWhileInState : StateMachineBehaviour
    {
        private float _previousWeight;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            _previousWeight = animator.GetLayerWeight(layerIndex);
            animator.SetLayerWeight(layerIndex, 0);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetLayerWeight(layerIndex, _previousWeight);
        }
    }
}