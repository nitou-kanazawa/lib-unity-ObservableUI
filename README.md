# Observable UI

UniRx に uGUI 関連の機能を追加する軽量ライブラリ．

#### 依存ライブラリ
- [UniRx][github: UniRx]

## 開発環境
- Unity `6000.0.30f1`
- Unity UI `2.0.0`
- Localization `1.5.4`
- UniRx `7.1.0`


## 導入方法

#### UPM Package
Package Manager を開き，「git packageから追加」で `https://github.com/nitou-kanazawa/lib-unity-ObservableUI.git?path=ObservableUI/Assets/ObservableUI` を指定します．

もしくは，`Packages/manifest.json` に以下を追加してください．
```
https://github.com/nitou-kanazawa/lib-unity-ObservableUI.git?path=ObservableUI/Assets/ObservableUI
```


## 機能

#### 拡張メソッド

UniRx では [`UnityUIComponentExtensions`クラス][github: UnityUIComponentExtensions]で uGUI に対する拡張メソッド（`OnValueChangedAsObservable` や `SubscribeToText` など）が提供されています．

本ライブラリではこれらに，以下を追加します．
- 主要コンポーネントへの双方向バインド（`BindToXXX`）
- `TextMeshPro`コンポーネントの対応

```cs
// Text (TMP)
stringRP.SubscribeToText(textMeshPro);
intRP.SubscribeToText(textMeshPro, value => t.text = $"{value} pt");

// InputField (TMP)


// Dropdown (TMP)


// Slider
floatRP.SubscribeToSlider(slider);  // 0~1

// Image


```


#### Reactive InputField

InputField は文字列形式でユーザー入力を受け付けるため，常にパース処理を意識する必要があります．
`Reactive InputField` は文字列入力を内部で処理し，`ReactiveProperty` として外部に公開するラッパーコンポーネントです．Int や Float などのシンプルな入力用途で，拡張メソッドと合わせて使用すると便利です．


```cs
FloatReactiveInputField reactiveIF;

reactiveIF.ReactiveProperity
    .Subscribe(value => Debug.Log(value))
    .AddTo(this);
```


---
<!-- Link -->
[github: UniRx]: https://github.com/neuecc/UniRx/tree/master
[github: UnityUIComponentExtensions]: https://github.com/neuecc/UniRx/blob/master/Assets/Plugins/UniRx/Scripts/UnityEngineBridge/UnityUIComponentExtensions.cs