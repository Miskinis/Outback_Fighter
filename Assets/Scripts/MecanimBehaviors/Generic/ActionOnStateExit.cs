﻿using System;
using UnityEngine;

namespace MecanimBehaviors
{
    public abstract class ActionOnStateExit : StateMachineBehaviour
    {
        public Action onExitAction;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onExitAction?.Invoke();
        }
    }
}