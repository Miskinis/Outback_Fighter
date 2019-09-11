using System;
using System.Collections.Generic;
using UnityEngine;

namespace MecanimBehaviors
{
    public class ActionsBetweenFrames : StateMachineBehaviour
    {
        public List<Action> actions;
        private List<bool> _actionsFired;

        public List<int> eventsOnFrames;

        private float _frameTime;
        private bool _initialized;

        [Tooltip("Total number of frames in this animation")]
        public int totalFrameCount;

        private void Awake()
        {
            actions = new List<Action>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_initialized)
            {
                _initialized  = true;
                _frameTime    = stateInfo.length / totalFrameCount;
                _actionsFired = new List<bool>(actions.Count);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var currentTime = stateInfo.length * stateInfo.normalizedTime;

            var length                                            = actions.Count;
            if (actions.Count != _actionsFired.Count) _actionsFired = new List<bool>(new bool[length]);

            length = eventsOnFrames.Count > actions.Count ? actions.Count : eventsOnFrames.Count;
            for (var i = 0; i < length; i++)
            {
                if (currentTime >= _frameTime * eventsOnFrames[i] && !_actionsFired[i])
                {
                    actions[i]?.Invoke();
                    _actionsFired[i] = true;
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var length                                       = _actionsFired.Count;
            for (var i = 0; i < length; i++)
            {
                _actionsFired[i] = false;
            }
        }
    }
}