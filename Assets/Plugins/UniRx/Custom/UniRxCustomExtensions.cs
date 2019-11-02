using System;
using TMPro;
using UniRx;

namespace Plugins.UniRx
{
    public static class UnityUIComponentExtensions
    {
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }
    }
}