using UnityEngine;
using TMPro;
using UniRx;
using System;
using System.Linq;


namespace Nitou.ObservableUI {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(TMP_Dropdown))]
	public abstract class ReactiveEnumDropdown<TEnum> : MonoBehaviour, IReactivePropertyHolder<TEnum>
		where TEnum : Enum {

		private TMP_Dropdown _dropdown;
		private ReactiveProperty<TEnum> _currentRP = new();


		public IReactiveProperty<TEnum> ReactiveProperty => _currentRP;


		/// ----------------------------------------------------------------------------
		// LifeCycle Events

		private void Awake() {
			_dropdown ??= GetComponent<TMP_Dropdown>();
			UpdateView();

			// Viewの更新
			_dropdown.onValueChanged.AsObservable()
				.Subscribe(index => {
					if (0 <= index && index < kEnumValues.Length) {
						_currentRP.Value = kEnumValues[index];
					} else {
						UpdateView();
					}
				})
				.AddTo(this);

			// RPの更新
			_currentRP
				.Subscribe(type => RefreshDropdownValue())
				.AddTo(this);
		}

		private void OnDestroy() {
			_currentRP?.Dispose();
		}

		private void OnValidate() {
			_dropdown ??= GetComponent<TMP_Dropdown>();
		}


		/// ----------------------------------------------------------------------------
		// Public Method

		public void UpdateView() {
			SetupOptions();
			RefreshDropdownValue();
		}


		/// ----------------------------------------------------------------------------
		// Private Method

		private void SetupOptions() {
			// Enumの名前リストを取得してDropdownのオプションに設定
			_dropdown.options.Clear();
			_dropdown.options.AddRange(
				kEnumValues.Select(name => new TMP_Dropdown.OptionData(name.ToString()))
			);
		}

		private void RefreshDropdownValue() {
			_dropdown.value = GetEnumIndex(_currentRP.Value);
			_dropdown.RefreshShownValue();
		}


		/// ----------------------------------------------------------------------------
		#region Static

		private static readonly TEnum[] kEnumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

		private static int GetEnumIndex(TEnum type) {
			var index = Array.IndexOf(kEnumValues, type);
			return Math.Max(0, index);
		}
		#endregion

	}
}
