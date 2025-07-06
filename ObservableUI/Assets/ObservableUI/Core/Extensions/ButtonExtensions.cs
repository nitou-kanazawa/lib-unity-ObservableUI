using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx.Triggers;

namespace UniRx
{
    public static partial class ButtonExtensions
    {
        /// ----------------------------------------------------------------------------
        #region Observable

        /// <summary>
        /// <see cref="Button"/>のダブルクリックイベントをObservableとして取得する．
        /// </summary>
        public static IObservable<Unit> OnDoubleClickAsObservable(this Button button, float intervalSeconds = 0.3f, float cooldownSeconds = 0.5f)
        {
            return button.OnClickAsObservable()
                .Timestamp()
                .Pairwise()
                .Select(pair => (pair.Current.Timestamp - pair.Previous.Timestamp).TotalSeconds)
                .Where(x => x <= intervalSeconds)
                .AsUnitObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(cooldownSeconds));
        }

        /// <summary>
        /// <see cref="Button"/>の長押しイベントをObservableとして取得する．
        /// </summary>
        public static IObservable<Unit> OnLongPressAsObservable(this Button button, float threshold = 0.5f)
        {
            var pointerDown = button.OnPointerDownAsObservable();
            var pointerUp = button.OnPointerUpAsObservable();

            return pointerDown
                .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(threshold)))
                .TakeUntil(pointerUp)
                // .DoOnCompleted(() => Debug.Log("Press Released"))
                .RepeatUntilDestroy(button)
                // .Do(time => Debug.Log($"Pressing... {time}"))
                .AsUnitObservable();
        }


        // NOTE: 
        // 以下のメソッドはRepeatされないのでイベントというより，単発の非同期処理に近い．
        // OnPointerDown, OnPressHold, OnPointerUpのイベントを扱いたい場合が多いと思うので，以下は必要ないのでは？

        [Obsolete("Use OnHoldPressAsObservable instead.")]
        public static IObservable<Unit> OnHoldPressAsObservable(this Button button)
        {
            var pointerDown = button.OnPointerDownAsObservable();
            var pointerUp = button.OnPointerUpAsObservable();

            return pointerDown
                .SelectMany(_ => button.UpdateAsObservable())
                .TakeUntil(pointerUp);
        }

        [Obsolete("Use OnHoldPressAsObservable instead.")]
        public static IObservable<long> OnHoldPressAsObservable(this Button button, float intervalSeconds = 0.1f)
        {
            var pointerDown = button.OnPointerDownAsObservable();
            var pointerUp = button.OnPointerUpAsObservable();

            return pointerDown
                .SelectMany(_ => Observable.Interval(TimeSpan.FromSeconds(intervalSeconds)))
                .TakeUntil(pointerUp);
        }

        #endregion

    }
}
