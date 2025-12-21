using UnityEngine;
using R3;

namespace Nitou.ObservableUI
{
    public interface IReactivePropertyHolder<T>
    {
        ReactiveProperty<T> ReactiveProperty { get; }
    }
}