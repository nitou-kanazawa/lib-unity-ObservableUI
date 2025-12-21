using System;
using UnityEngine.UI;

namespace R3
{
    /// <summary>
    /// <see cref="Slider"/>の拡張メソッド群．
    /// </summary>
    public static partial class SliderExtensions
    {
        #region Binding

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSlider(this Observable<float> source, Slider slider)
        {
            return source.Subscribe(x => slider.value = x);
        }

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSliderRange(this Observable<(float min, float max)> source, Slider slider)
        {
            return source.Subscribe(range =>
            {
                slider.minValue = range.min;
                slider.maxValue = range.max;
            });
        }

        /// <summary>
        /// <see cref="Slider"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToSlider(this ReactiveProperty<float> property, Slider slider)
        {
            // Model → View
            var d1 = property.Subscribe(x => slider.value = x);

            // View → Model
            var d2 = slider.OnValueChangedAsObservable()
                           .Subscribe(x => property.Value = x);

            return Disposable.Combine(d1, d2);
        }

        /// <summary>
        /// <see cref="Slider"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToSlider(this ReactiveProperty<bool> reactiveProperty, Slider slider)
        {
            // Model → View
            var d1 = reactiveProperty.Subscribe(value => slider.value = value ? 1f : 0f);

            // View → Model
            var d2 = slider.OnValueChangedAsObservable()
                           .Subscribe(value => reactiveProperty.Value = value >= 0.5f);

            return Disposable.Combine(d1, d2);
        }

        #endregion
    }
}