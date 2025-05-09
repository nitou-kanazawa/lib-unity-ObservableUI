using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace UniRx {

    public static partial class UnityUIComponentExtensions {

        // [NOTE]
        // - 基本，UniRx標準拡張メソッドと同様だが，`OnValueChangedAsObservable`では引数でcurrentValueを使用するか選択できるようにしている．
        // - 単方向バインドを`SubscribeToXXX`，双方向バインドを`BindToXXX`と表現している．


        /// ----------------------------------------------------------------------------
        #region Text

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshProUGUI text) {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text) {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text, Func<T, string> selector) {
            return source.SubscribeWithState2(text, selector, (x, t, s) => t.text = s(x));
        }
        #endregion


        /// ----------------------------------------------------------------------------
        #region InputField

        /// <summary>
        /// Observe onEndEdit(Submit) event.
        /// </summary>
        public static IObservable<string> OnEndEditAsObservable(this TMP_InputField inputField) {
            return inputField.onEndEdit.AsObservable();
        }

        /// <summary>
        /// Observe onValueChanged with current `text` value on subscribe.
        /// </summary>
        public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField, bool withCurrentValue = true) {
            return Observable.CreateWithState<string, TMP_InputField>(inputField, (i, observer) => {
                if (withCurrentValue) {
                    observer.OnNext(i.text);
                }
                return i.onValueChanged.AsObservable().Subscribe(observer);
            });
        }


        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<string> property, TMP_InputField inputField) {
            // Model → View
            var d1 = property.SubscribeWithState(inputField, (x, i) => i.text = x);

			// View → Model
			var d2 = inputField.OnEndEditAsObservable().SubscribeWithState(property, (x, p) => p.Value = x);

			return StableCompositeDisposable.Create(d1, d2);
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<int> property, TMP_InputField inputField) {
			// int
			return property.BindToInputField(inputField, 
                value => int.Parse(value),
                value => value.ToString());
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField(this IReactiveProperty<float> property, TMP_InputField inputField, string format = "F2") {
            // float 
            return property.BindToInputField(inputField, 
                value => float.Parse(value),
                value => value.ToString(format));
        }

        /// <summary>
        /// 変換処理を指定した<see cref="TMP_InputField"/>との双方向バインディング．
        /// </summary>
        public static IDisposable BindToInputField<T>(
			this IReactiveProperty<T> property, TMP_InputField inputField,
            Func<string, T> parseFunc, Func<T, string> formatFunc) {

            // Model → View
            var d1 = property.SubscribeWithState2(inputField, formatFunc, (x, i, f) => i.text = f(x));

			// View → Model
			var d2 = inputField.OnEndEditAsObservable()
                .SubscribeWithState3(property, formatFunc, inputField, 
                (value, p, f, i) => {
                    try {
                        p.Value = parseFunc(value);
                    } catch {
                        // 変換失敗時に入力フィールドをリセット
                        i.text = f(p.Value);
                    }
                });

			return StableCompositeDisposable.Create(d1, d2);
        }
        #endregion


        /// ----------------------------------------------------------------------------
        #region Dropdown

        /// <summary>
        /// Observe onValueChanged with current `value` on subscribe.
        /// </summary>
        public static IObservable<int> OnValueChangedAsObservable(this TMP_Dropdown dropdown, bool withCurrentValue = true) {
            return Observable.CreateWithState<int, TMP_Dropdown>(dropdown, (d, observer) => {
                if (withCurrentValue) {
                    observer.OnNext(d.value);
                }
                return d.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

		/// <summary>
		/// <see cref="TMP_Dropdown"/>への双方向バインディング．
		/// </summary>
		public static IDisposable BindToDropdown(this IReactiveProperty<int> property, TMP_Dropdown dropdown) {
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
            Func<T, int> toDropdownValue, Func<int, T> fromDropdownValue) {
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


        /// ----------------------------------------------------------------------------
        #region Slider

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSlider(this IObservable<float> source, Slider slider) {
            return source.SubscribeWithState(slider, (x, s) => s.value = x);
        }

        /// <summary>
        /// <see cref="Slider"/>への単方向バインディング．
        /// </summary>
        public static IDisposable SubscribeToSliderRange(this IObservable<(float min, float max)> source, Slider slider) {
            return source.SubscribeWithState(slider, (range, s) => {
                s.minValue = range.min;
                s.maxValue = range.max;
            });
        }

		/// <summary>
		/// <see cref="Slider"/>への双方向バインディング．
		/// </summary>
		public static IDisposable BindToSlider(this IReactiveProperty<float> property, Slider slider) {
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
        public static IDisposable BindToSlider(this IReactiveProperty<bool> reactiveProperty, Slider slider) {
            // Model → View
            var d1 = reactiveProperty.SubscribeWithState(slider, (value, s) => s.value = value ? 1f : 0f);

            // View → Model
            var d2 = slider.OnValueChangedAsObservable()
                  .SubscribeWithState(reactiveProperty, (value, p) => p.Value = value >= 0.5f);

			return StableCompositeDisposable.Create(d1, d2);
        }
		#endregion


		/// ----------------------------------------------------------------------------
		#region Toggle

		/// <summary>
		/// <see cref="Toggle"/>への単方向バインディング．
		/// </summary>
		public static IDisposable SubscribeToToggle(this IObservable<bool> source, Toggle toggle) {
			return source.SubscribeWithState(toggle, (x, s) => s.isOn = x);
		}

		/// <summary>
		/// <see cref="Toggle"/>への双方向バインディング．
		/// </summary>
		public static IDisposable BindToToggle(this IReactiveProperty<bool> property, Toggle toggle) {
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
        [Obsolete("実験的なAPIであり，変更される可能性があります．")]
        public static IDisposable BindToToggleGroup<T>(this IReactiveProperty<T> property, 
            IEnumerable<Toggle> toggles, IEnumerable<T> values) 
            where T : Enum {

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
				.Subscribe(current => {
					foreach (var (toggle, value) in toggleValuePairs) {
						toggle.isOn = EqualityComparer<T>.Default.Equals(current, value);
					}
				});

			return StableCompositeDisposable.Create(d1, d2);
        }

        #endregion


        /// ----------------------------------------------------------------------------
        #region Image

        /// <summary>
        /// <see cref="Image"/>.fillAmountへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageFillAmount(this IObservable<float> source, Image image) {
            return source.SubscribeWithState(image, (x, i) => i.fillAmount = x);
        }

        /// <summary>
        /// <see cref="Image"/>.colorへのバインディング．
        /// </summary>
        public static IDisposable SubscribeToImageColor(this IObservable<Color> source, Image image) {
            return source.SubscribeWithState(image, (x, i) => i.color = x);
        }
		#endregion


		/// ----------------------------------------------------------------------------
		#region ScrollRect


		#endregion
	}
}
