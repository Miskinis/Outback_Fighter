using System;
using UnityEngine;

namespace MecanimBehaviors
{
    public abstract class ActionOnFrame : StateMachineBehaviour
    {
        public Action onFrameAction;

        [Tooltip("Total number of frames in this animation")]
        public int totalFrameCount;

        [Tooltip("When to fire action")] public int actionFrame;

        private float _frameTime;
        private bool _initialized;
        private bool _actionFired;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_initialized)
            {
                _initialized = true;
                _frameTime   = stateInfo.length / totalFrameCount;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onFrameAction == null) return;
            
            var currentTime = stateInfo.length * stateInfo.normalizedTime;

            if (currentTime >= _frameTime * actionFrame && !_actionFired)
            {
                _actionFired = true;
                onFrameAction.Invoke();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _actionFired = false;
        }
    }
}