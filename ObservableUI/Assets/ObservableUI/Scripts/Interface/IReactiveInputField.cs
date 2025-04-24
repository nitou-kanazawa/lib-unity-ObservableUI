using UnityEngine;
using UniRx;

namespace Nitou.ObservableUI {

	public interface IReactiveInputField<T> {

		IReactiveProperty<T> ReactiveProperty { get; }

		bool IsIntaractable { get; set; }
	}



}
