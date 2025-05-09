using UnityEngine;
using UniRx;

namespace Nitou.ObservableUI {

	/// <summary>
	/// <see cref="ReactiveProperty{T}"/> を内部データとしたInputField．
	/// </summary>
	public interface IReactiveInputField<T> : IReactivePropertyHolder<T>
		where T : struct{

		bool IsIntaractable { get; set; }
	}

}
