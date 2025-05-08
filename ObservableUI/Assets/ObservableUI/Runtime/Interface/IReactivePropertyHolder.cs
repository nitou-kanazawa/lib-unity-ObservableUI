using UnityEngine;
using UniRx;

namespace Nitou.ObservableUI {
	
	public interface IReactivePropertyHolder<T> where T : struct {
		IReactiveProperty<T> ReactiveProperty { get; }
	}



}
