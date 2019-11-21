using Plugins.SerializableDictionary;
using UnityEngine;

namespace MecanimBehaviors.Generic
{
    public class ChangeBoolOnStateEnter : StateMachineBehaviour
    {
        [Tooltip("Bool parameter names and values to change")]
        public StringBoolDictionary parameters;

        private IntBoolDictionary _parameters;

        private void Awake()
        {
            _parameters = new IntBoolDictionary();
            foreach (var parameter in parameters) _parameters.Add(Animator.StringToHash(parameter.Key), parameter.Value);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var parameter in _parameters) animator.SetBool(parameter.Key, parameter.Value);
        }
    }
}