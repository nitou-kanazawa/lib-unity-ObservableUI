using System;

namespace Nitou.ObservableUI
{
    public interface IReactiveInputFieldStepper<T> : IReactiveInputField<T>
        where T : struct
    {
        T Delta { get; }

        void MovePrevious();
        void MoveNext();

        bool CanMovePrevious();
        bool CanMoveNext();
    }
}