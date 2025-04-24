using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// [�Q�l]
//  qiita: UniRx����@~ �f�[�^�o�C���f�B���O��Unity�C�x���g�֐��̍w�� ~ https://qiita.com/su10/items/6d7fd792d4b553454a4f
//  Hatena: UniRx: SubscribeWithState �� Subscribe ���������悢 https://noriok.hatenadiary.jp/entry/2018/09/17/144930

namespace UniRx {

    public static partial class UnityUIComponentExtensions {

        // [NOTE]
        // - ��{�CUniRx�W���g�����\�b�h�Ɠ��l�����C`OnValueChangedAsObservable`�ł͈�����currentValue���g�p���邩�I���ł���悤�ɂ��Ă���D
        // - �P�����o�C���h��`SubscribeToXXX`�C�o�����o�C���h��`BindToXXX`�ƕ\�����Ă���D


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
        /// <see cref="TMP_InputField"/>�Ƃ̑o�����o�C���f�B���O�D
        /// </summary>
        public static void BindToInputField(this IReactiveProperty<string> property, TMP_InputField inputField, ICollection<IDisposable> disposables) {

            // Model �� View
            property.SubscribeWithState(inputField, (x, i) => i.text = x).AddTo(disposables);

            // View �� Model
            inputField.OnEndEditAsObservable().SubscribeWithState(property, (x, p) => p.Value = x).AddTo(disposables);
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>�Ƃ̑o�����o�C���f�B���O�D
        /// </summary>
        public static void BindToInputField(this IReactiveProperty<int> property, TMP_InputField inputField, ICollection<IDisposable> disposables) {
            // int
            property.BindToInputField(inputField, 
                value => int.Parse(value),
                value => value.ToString(),
                disposables);
        }

        /// <summary>
        /// <see cref="TMP_InputField"/>�Ƃ̑o�����o�C���f�B���O�D
        /// </summary>
        public static void BindToInputField(this IReactiveProperty<float> property, TMP_InputField inputField, ICollection<IDisposable> disposables) {
            // float 
            property.BindToInputField(inputField, 
                value => float.Parse(value),
                value => value.ToString("F2"),
                disposables);
        }

        /// <summary>
        /// �ϊ��������w�肵��<see cref="TMP_InputField"/>�Ƃ̑o�����o�C���f�B���O�D
        /// </summary>
        public static void BindToInputField<T>(this IReactiveProperty<T> property, TMP_InputField inputField,
            Func<string, T> parseFunc, Func<T, string> formatFunc,
            ICollection<IDisposable> disposables) {

            // Model �� View
            property.SubscribeWithState2(inputField, formatFunc, (x, i, f) => i.text = f(x)).AddTo(disposables);

            // View �� Model
            inputField.OnEndEditAsObservable()
                .SubscribeWithState3(property, formatFunc, inputField, 
                (value, p, f, i) => {
                    try {
                        p.Value = parseFunc(value);
                    } catch {
                        // �ϊ����s���ɓ��̓t�B�[���h�����Z�b�g
                        i.text = f(p.Value);
                    }
                })
                .AddTo(disposables);
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
        /// 
        /// </summary>
        public static void BindToDropdown(this IReactiveProperty<int> property, TMP_Dropdown dropdown, ICollection<IDisposable> disposables) {

            // Model �� View
            property.SubscribeWithState(dropdown, (x, d) => d.value = x).AddTo(disposables);

            // View �� Model
            dropdown.OnValueChangedAsObservable()
                .SubscribeWithState(property, (x, p) => p.Value = x).AddTo(disposables);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void BindToDropdown<T>(
            this IReactiveProperty<T> property, TMP_Dropdown dropdown,
            Func<T, int> toDropdownValue, Func<int, T> fromDropdownValue,
            ICollection<IDisposable> disposables) {
            // Model �� View
            property
                .Select(toDropdownValue)  // T �� int �̕ϊ�
                .SubscribeWithState(dropdown, (x, d) => d.value = x)
                .AddTo(disposables);

            // View �� Model
            dropdown.OnValueChangedAsObservable()
                .Select(fromDropdownValue) // int �� T �̕ϊ�
                .SubscribeWithState(property, (x, p) => p.Value = x)
                .AddTo(disposables);
        }

        #endregion



        /// ----------------------------------------------------------------------------
        #region Slider

        /// <summary>
        /// <see cref="Slider"/>�ւ̒P�����o�C���f�B���O�D
        /// </summary>
        public static IDisposable SubscribeToSlider(this IObservable<float> source, Slider slider) {
            return source.SubscribeWithState(slider, (x, s) => s.value = x);
        }

        /// <summary>
        /// <see cref="Slider"/>�ւ̒P�����o�C���f�B���O�D
        /// </summary>
        public static IDisposable SubscribeToSliderRange(this IObservable<(float min, float max)> source, Slider slider) {
            return source.SubscribeWithState(slider, (range, s) => {
                s.minValue = range.min;
                s.maxValue = range.max;
            });
        }


        /// <summary>
        /// 
        /// </summary>
        public static void BindToSlider(this IReactiveProperty<float> property, Slider slider, ICollection<IDisposable> disposables) {

            // Model �� View
            property.SubscribeWithState(slider, (x, s) => s.value = x).AddTo(disposables);

            // View �� Model
            slider.OnValueChangedAsObservable()
                .SubscribeWithState(property, (x, p) => p.Value = x).AddTo(disposables);
        }

        /// <summary>
        /// <see cref="Slider"/>�Ƃ̑o�����o�C���f�B���O�D
        /// </summary>
        public static void BindToSlider(this IReactiveProperty<bool> reactiveProperty, Slider slider, ICollection<IDisposable> disposables) {
            // Model �� View
            reactiveProperty.SubscribeWithState(slider, (value, s) => s.value = value ? 1f : 0f).AddTo(disposables);

            // View �� Model
            slider.OnValueChangedAsObservable()
                  .SubscribeWithState(reactiveProperty, (value, p) => p.Value = value >= 0.5f).AddTo(disposables);
        }
        #endregion


        /// ----------------------------------------------------------------------------
        #region Toggle

        /// <summary>
        /// 
        /// </summary>
        public static void BindToToggle(this IReactiveProperty<bool> property, Toggle toggle, ICollection<IDisposable> disposables) {

            // Model �� View
            property.SubscribeWithState(toggle, (x, t) => t.isOn = x).AddTo(disposables);

            // View �� Model
            toggle.OnValueChangedAsObservable()
                .SubscribeWithState(property, (x, p) => p.Value = x).AddTo(disposables);
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("�����I��API�ł���C�ύX�����\��������܂��D")]
        public static void BindToToggleGroup<T>(this IReactiveProperty<T> property, 
            IEnumerable<Toggle> toggles, IEnumerable<T> values,
            ICollection<IDisposable> disposables) 
            where T : Enum {

            int toggleCount = toggles.Count();
            int valueCount = values.Count();
            if (toggleCount == 0 || valueCount == 0 || toggleCount != valueCount)
                throw new InvalidOperationException("");


            var toggleValuePairs = toggles.Zip(values, (toggle, value) => (toggle, value));
            var toggleStreams = toggleValuePairs.Select(pair => pair.toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => pair.value));

            toggleStreams
                .Merge()
                .SubscribeWithState(property, (value, p) => p.Value = value)
                .AddTo(disposables);

            // Model �� View
            property
                .Subscribe(current => {
                    foreach (var (toggle, value) in toggleValuePairs) {
                        toggle.isOn = EqualityComparer<T>.Default.Equals(current, value);
                    }
                })
                .AddTo(disposables);
        }

        #endregion


        /// ----------------------------------------------------------------------------
        #region Image

        /// <summary>
        /// <see cref="Image"/>.fillAmount�ւ̃o�C���f�B���O�D
        /// </summary>
        public static IDisposable SubscribeToImageFillAmount(this IObservable<float> source, Image image) {
            return source.SubscribeWithState(image, (x, i) => i.fillAmount = x);
        }

        /// <summary>
        /// <see cref="Image"/>.color�ւ̃o�C���f�B���O�D
        /// </summary>
        public static IDisposable SubscribeToImageColor(this IObservable<Color> source, Image image) {
            return source.SubscribeWithState(image, (x, i) => i.color = x);
        }
        #endregion
    }
}
