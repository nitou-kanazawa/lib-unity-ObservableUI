using System;
using TMPro;

namespace UniRx
{
    /// <summary>
    /// <see cref="TMP_Dropdown"/>の拡張メソッド群．
    /// </summary>
    public static partial class DropdownExtensions
    {
        /// ----------------------------------------------------------------------------
        #region Observable

        /// <summary>
        /// Observe onValueChanged with current `value` on subscribe.
        /// </summary>
        public static IObservable<int> OnValueChangedAsObservable(this TMP_Dropdown dropdown, bool withCurrentValue = true)
        {
            return Observable.CreateWithState<int, TMP_Dropdown>(dropdown, (d, observer) =>
            {
                if (withCurrentValue)
                {
                    observer.OnNext(d.value);
                }
                return d.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>
        /// <see cref="TMP_Dropdown"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToDropdown(this IReactiveProperty<int> property, TMP_Dropdown dropdown)
        {
            // Model → View
            var d1 = property.SubscribeWithState(dropdown, (x, d) => d.value = x);

            // View → Model
            var d2 = dropdown.OnValueChangedAsObservable()
                .SubscribeWithState(property, (x, p) => p.Value = x);

            return StableCompositeDisposable.Create(d1, d2);
        }

        /// <summary>
        /// <see cref="TMP_Dropdown"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToDropdown<T>(
            this IReactiveProperty<T> property, TMP_Dropdown dropdown,
            Func<T, int> toDropdownValue, Func<int, T> fromDropdownValue)
        {
            // Model → View
            var d1 = property
                .Select(toDropdownValue)  // T → int の変換
                .SubscribeWithState(dropdown, (x, d) => d.value = x);

            // View → Model
            var d2 = dropdown.OnValueChangedAsObservable()
                .Select(fromDropdownValue) // int → T の変換
                .SubscribeWithState(property, (x, p) => p.Value = x);

            return StableCompositeDisposable.Create(d1, d2);
        }

        #endregion
    }
}
