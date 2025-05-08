using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using Nitou.ObservableUI;

public class Develop_EntryPoint : MonoBehaviour {

	public Slider _slider;
	public FloatReactiveInputField _inputField;

	void Start() {

	 	_inputField.ReactiveProperty
			.BindToSlider(_slider)
			.AddTo(this);

	}

}
