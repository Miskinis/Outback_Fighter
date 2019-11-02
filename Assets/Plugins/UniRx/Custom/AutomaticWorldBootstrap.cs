using UniRx.Async;
using Unity.Entities;
using UnityEngine;

namespace Plugins.UniRx
{  
#if! UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP
    static class AutomaticWorldBootstrap
    {
        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize ()
        {
            var playerLoop = ScriptBehaviourUpdateOrder.CurrentPlayerLoop;
            PlayerLoopHelper.Initialize ( ref playerLoop);
        }
    }
#endif
}