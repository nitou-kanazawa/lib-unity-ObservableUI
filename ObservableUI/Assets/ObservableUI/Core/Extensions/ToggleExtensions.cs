using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="Toggle"/>の拡張メソッド群．
    /// </summary>
    public static partial class ToggleExtensions
    {
        /// <summary>
        /// <see cref="Toggle"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToToggle(this Observable<bool> source, Toggle toggle)
        {
            return source.Subscribe(x => toggle.isOn = x);
        }

        /// <summary>
        /// <see cref="Toggle"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToToggle(this ReactiveProperty<bool> property, Toggle toggle)
        {
            // Model → View
            var d1 = property.Subscribe(x => toggle.isOn = x);
            // View → Model
            var d2 = toggle.OnValueChangedAsObservable()
                           .Subscribe(x => property.Value = x);

            return Disposable.Combine(d1, d2);
        }


        /// <summary>
        ///
        /// </summary>
        [Obsolete("実験的なAPI．変更される可能性があります．")]
        public static IDisposable BindToToggleGroup<T>(this ReactiveProperty<T> property,
                                                       IEnumerable<Toggle> toggles, IEnumerable<T> values)
            where T : Enum
        {
            int toggleCount = toggles.Count();
            int valueCount = values.Count();
            if (toggleCount == 0 || valueCount == 0 || toggleCount != valueCount)
                throw new InvalidOperationException("");


            var toggleValuePairs = toggles.Zip(values, (toggle, value) => (toggle, value));
            var toggleStreams = toggleValuePairs.Select(pair => pair.toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => pair.value));

            var d1 = Observable.Merge(toggleStreams.ToArray())
                               .Subscribe(value => property.Value = value);

            // Model → View
            var d2 = property
                .Subscribe(current =>
                {
                    foreach (var (toggle, value) in toggleValuePairs)
                    {
                        toggle.isOn = EqualityComparer<T>.Default.Equals(current, value);
                    }
                });

            return Disposable.Combine(d1, d2);
        }
    }
}