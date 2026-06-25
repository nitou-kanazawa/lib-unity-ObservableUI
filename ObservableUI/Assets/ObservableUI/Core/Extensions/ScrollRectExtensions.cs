using System;
using UnityEngine;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="ScrollRect"/>の拡張メソッド群．
    /// </summary>
    public static partial class ScrollRectExtensions
    {
        #region Observable

        /// <summary>
        /// Observe onValueChanged with current `normalizedPosition` on subscribe.
        /// </summary>
        public static Observable<Vector2> OnValueChangedAsObservable(this ScrollRect scrollRect, bool withCurrentValue = true)
        {
            return Observable.Create<Vector2>(observer =>
            {
                if (withCurrentValue)
                    observer.OnNext(scrollRect.normalizedPosition);

                return scrollRect.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        #endregion


        #region Binding

        /// <summary>
        /// <see cref="ScrollRect.normalizedPosition"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToNormalizedPosition(this Observable<Vector2> source, ScrollRect scrollRect)
        {
            return source.Subscribe(x => scrollRect.normalizedPosition = x);
        }

        #endregion
    }
}
