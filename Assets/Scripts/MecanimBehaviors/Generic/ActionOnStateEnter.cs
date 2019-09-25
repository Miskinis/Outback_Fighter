using System;
using UnityEngine;

namespace MecanimBehaviors
{
    public abstract class ActionOnStateEnter : StateMachineBehaviour
    {
        public Action onEnterAction;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onEnterAction?.Invoke();
        }
    }
}