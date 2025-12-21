using System;
using UnityEngine.UI;
using R3.Triggers;

namespace R3
{
    public static partial class ButtonExtensions
    {
        #region Observable

        /// <summary>
        /// <see cref="Button"/>のダブルクリックイベントをObservableとして取得する．
        /// </summary>
        public static Observable<Unit> OnDoubleClickAsObservable(this Button button, float intervalSeconds = 0.3f, float cooldownSeconds = 0.5f)
        {
            return button.OnClickAsObservable()
                         .Timestamp()
                         .Pairwise()
                         .Select(pair => (pair.Current.Timestamp - pair.Previous.Timestamp))
                         .Where(x => x <= intervalSeconds)
                         .AsUnitObservable()
                         .ThrottleFirst(TimeSpan.FromSeconds(cooldownSeconds));
        }

        /// <summary>
        /// <see cref="Button"/>の長押しイベントをObservableとして取得する．
        /// </summary>
        public static Observable<Unit> OnLongPressAsObservable(this Button button, float threshold = 0.5f)
        {
            var pointerDown = button.OnPointerDownAsObservable();
            var pointerUp = button.OnPointerUpAsObservable();

            return pointerDown
                   .SelectMany(_ =>
                       Observable.Timer(TimeSpan.FromSeconds(threshold))
                                 .TakeUntil(pointerUp)
                   )
                   .TakeUntil(button.OnDestroyAsObservable())
                   .AsUnitObservable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="intervalSeconds"></param>
        /// <returns></returns>
        public static Observable<Unit> OnHoldPressAsObservable(
            this Button button,
            float intervalSeconds = 0.1f)
        {
            var pointerDown = button.OnPointerDownAsObservable();
            var pointerUp = button.OnPointerUpAsObservable();

            return pointerDown
                   .SelectMany(_ =>
                       Observable.Interval(TimeSpan.FromSeconds(intervalSeconds))
                                 .TakeUntil(pointerUp)
                   )
                   .TakeUntil(button.OnDestroyAsObservable());
        }

        #endregion
    }
}