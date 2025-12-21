using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nitou.ObservableUI;
using R3;

public class Develop_EntryPoint : MonoBehaviour
{

    public Slider _slider;
    public Button _button;
    // public TextMeshProUGUI _text;
    // public Toggle _toggle;

    public FloatReactiveInputField _inputField;

    void Start()
    {

        _inputField.ReactiveProperty
            .BindToSlider(_slider)
            .AddTo(this);

        _button.OnDoubleClickAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("Button double clicked!");
            })
            .AddTo(this);

        _button.OnLongPressAsObservable(threshold: 1f)
            .Subscribe(_ =>
            {
                Debug.Log("Button long pressed!");
            })
            .AddTo(this);
    }

}
