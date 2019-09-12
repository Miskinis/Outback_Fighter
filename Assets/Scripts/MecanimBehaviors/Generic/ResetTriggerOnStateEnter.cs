using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace MecanimBehaviors
{
    public class ResetTriggerOnStateEnter : StateMachineBehaviour
    {
        [Tooltip("Bool parameter names and values to change")]
        public List<string> parameters;

        private NativeArray<int> _parameters;

        private void Awake()
        {
            var count = parameters.Count;
            _parameters = new NativeArray<int>(count, Allocator.Persistent);
            for (var i = 0; i < count; i++)
            {
                _parameters[i] = Animator.StringToHash(parameters[i]);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (var i = 0; i < _parameters.Length; i++)
            {
                animator.ResetTrigger(_parameters[i]);
            }
        }

        private void OnDestroy()
        {
            _parameters.Dispose();
        }
    }
}