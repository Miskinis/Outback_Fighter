using UnityEngine;

namespace MecanimBehaviors
{
    public class ChangeBoolFramesDuration : StateMachineBehaviour
    {
        public string boolParameterName;
        public int eventEndFrame;
        private bool _eventFired;

        public int eventStartFrame;
        protected float frameTime;
        protected bool initialized;

        [Tooltip("Total number of frames in this animation")]
        public int totalFrameCount;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!initialized)
            {
                initialized = true;
                frameTime   = stateInfo.length / totalFrameCount;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_eventFired)
            {
                var currentTime = stateInfo.length * stateInfo.normalizedTime;
                if (currentTime >= frameTime * eventStartFrame && currentTime <= frameTime * eventEndFrame)
                {
                    animator.SetBool(boolParameterName, true);
                    _eventFired = true;
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolParameterName, false);
            _eventFired = false;
        }
    }
}