using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace UniRx
{
    /// <summary>
    /// <see cref="Toggle"/>の拡張メソッド群．
    /// </summary>
    public static partial class ToggleExtensions
    {

        /// ----------------------------------------------------------------------------
        #region Toggle

        /// <summary>
        /// <see cref="Toggle"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToToggle(this IObservable<bool> source, Toggle toggle)
        {
            return source.SubscribeWithState(toggle, (x, s) => s.isOn = x);
        }

        /// <summary>
        /// <see cref="Toggle"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToToggle(this IReactiveProperty<bool> property, Toggle toggle)
        {
            // Model → View
            var d1 = property.SubscribeWithState(toggle, (x, t) => t.isOn = x);
            // View → Model
            var d2 = toggle.OnValueChangedAsObservable()
                .SubscribeWithState(property, (x, p) => p.Value = x);

            return StableCompositeDisposable.Create(d1, d2);
        }


        /// <summary>
        /// 
        /// </summary>
        [Obsolete("実験的なAPI．変更される可能性があります．")]
        public static IDisposable BindToToggleGroup<T>(this IReactiveProperty<T> property,
            IEnumerable<Toggle> toggles, IEnumerable<T> values)
            where T : Enum
        {

            int toggleCount = toggles.Count();
            int valueCount = values.Count();
            if (toggleCount == 0 || valueCount == 0 || toggleCount != valueCount)
                throw new InvalidOperationException("");


            var toggleValuePairs = toggles.Zip(values, (toggle, value) => (toggle, value));
            var toggleStreams = toggleValuePairs.Select(pair => pair.toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => pair.value));

            var d1 = toggleStreams
                .Merge()
                .SubscribeWithState(property, (value, p) => p.Value = value);

            // Model → View
            var d2 = property
                .Subscribe(current =>
                {
                    foreach (var (toggle, value) in toggleValuePairs)
                    {
                        toggle.isOn = EqualityComparer<T>.Default.Equals(current, value);
                    }
                });

            return StableCompositeDisposable.Create(d1, d2);
        }

        #endregion
    }

}
