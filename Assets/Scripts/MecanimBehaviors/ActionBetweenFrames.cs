using System;
using UnityEngine;

namespace MecanimBehaviors
{
    public abstract class ActionBetweenFrames : StateMachineBehaviour
    {
        public Action onStartAction;
        public Action onEndAction;

        [Tooltip("Total number of frames in this animation")]
        public int totalFrameCount;

        public int actionStartFrame;
        public int actionEndFrame;

        private float _frameTime;
        private bool _initialized;
        private bool _actionStarted;
        private bool _actionEnded;

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
            if (onStartAction == null || onEndAction == null) return;
            
            var currentTime = stateInfo.length * stateInfo.normalizedTime;

            if (currentTime >= _frameTime * actionStartFrame && !_actionStarted)
            {
                _actionStarted = true;
                onStartAction.Invoke();
            }
            else if (currentTime >= _frameTime * actionEndFrame && !_actionEnded)
            {
                onEndAction.Invoke();
                _actionEnded = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _actionStarted = false;
            _actionEnded   = false;
        }
    }
}