using UnityEngine;

namespace Nitou.ObservableUI
{
    public interface IInputValidator<T>
    {
        T Validate(T value);
    }
}