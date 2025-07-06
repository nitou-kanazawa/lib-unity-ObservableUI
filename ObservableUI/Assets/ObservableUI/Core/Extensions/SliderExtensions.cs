using System;
using UnityEngine.UI;

namespace UniRx
{
    public static partial class SliderExtensions
    {

        /// ----------------------------------------------------------------------------
        #region Binding

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSlider(this IObservable<float> source, Slider slider)
        {
            return source.SubscribeWithState(slider, (x, s) => s.value = x);
        }

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSliderRange(this IObservable<(float min, float max)> source, Slider slider)
        {
            return source.SubscribeWithState(slider, (range, s) =>
            {
                s.minValue = range.min;
                s.maxValue = range.max;
            });
        }

        /// <summary>
        /// <see cref="Slider"/>への双方向バインディング．
        /// </summary>
        public static IDisposable BindToSlider(this IReactiveProperty<float> property, Slider slider)
        {
            // Model → View
            var d1 = property.SubscribeWithState(slider, (x, s) => s.value = x);

            // View → Model
            var d2 = slider.OnValueChangedAsObservable()
              .SubscribeWithState(property, (x, p) => p.Value = x);

            return StableCompositeDisposable.Create(d1, d2);
        }

        /// <summary>
        /// <see cref="Slider"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToSlider(this IReactiveProperty<bool> reactiveProperty, Slider slider)
        {
            // Model → View
            var d1 = reactiveProperty.SubscribeWithState(slider, (value, s) => s.value = value ? 1f : 0f);

            // View → Model
            var d2 = slider.OnValueChangedAsObservable()
              .SubscribeWithState(reactiveProperty, (value, p) => p.Value = value >= 0.5f);

            return StableCompositeDisposable.Create(d1, d2);
        }
        #endregion
    }

}
