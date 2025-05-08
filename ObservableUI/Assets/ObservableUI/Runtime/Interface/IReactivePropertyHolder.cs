using UnityEngine;
using UniRx;

namespace Nitou.ObservableUI {
	
	public interface IReactivePropertyHolder<T> {
		IReactiveProperty<T> ReactiveProperty { get; }
	}
}
