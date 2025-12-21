using System;
using TMPro;

namespace R3
{
    /// <summary>
    /// <see cref="TMP_Dropdown"/>の拡張メソッド群．
    /// </summary>
    public static partial class DropdownExtensions
    {
        #region Observable

        /// <summary>
        /// Observe onValueChanged with current `value` on subscribe.
        /// </summary>
        public static Observable<int> OnValueChangedAsObservable(this TMP_Dropdown dropdown, bool withCurrentValue = true)
        {
            return Observable.Create<int>(observer =>
            {
                if (withCurrentValue)
                {
                    observer.OnNext(dropdown.value);
                }
                return dropdown.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>
        /// <see cref="TMP_Dropdown"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToDropdown(this ReactiveProperty<int> property, TMP_Dropdown dropdown)
        {
            // Model → View
            var d1 = property.Subscribe(x => dropdown.value = x);

            // View → Model
            var d2 = dropdown.OnValueChangedAsObservable()
                .Subscribe(x => property.Value = x);

            return Disposable.Combine(d1, d2);
        }

        /// <summary>
        /// <see cref="TMP_Dropdown"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToDropdown<T>(
            this ReactiveProperty<T> property, TMP_Dropdown dropdown,
            Func<T, int> toDropdownValue, Func<int, T> fromDropdownValue)
        {
            // Model → View
            var d1 = property
                .Select(toDropdownValue)  // T → int の変換
                .Subscribe(x => dropdown.value = x);

            // View → Model
            var d2 = dropdown.OnValueChangedAsObservable()
                .Select(fromDropdownValue) // int → T の変換
                .Subscribe(x => property.Value = x);

            return Disposable.Combine(d1, d2);
        }

        #endregion
    }
}
