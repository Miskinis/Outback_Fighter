using UnityEngine;

namespace MecanimBehaviors
{
    public class RandomAnimationSelector : StateMachineBehaviour
    {
        public string integerParameter;

        [Tooltip("How many animations you have to choose from")]
        public int animationCount;

        private int _parameter;

        private void Awake()
        {
            _parameter = Animator.StringToHash(integerParameter);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetInteger(_parameter, Random.Range(0, animationCount));
        }
    }
}