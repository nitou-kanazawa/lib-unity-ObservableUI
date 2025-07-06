# Observable UI

UniRx に uGUI 関連の機能を追加する軽量ライブラリ．

<img src="docs/images/header.png" width=800>

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

#### 依存ライブラリ
- [UniRx][github: UniRx]

## 開発環境
- Unity `6000.0.30f1`
- Unity UI `2.0.0`
- Localization `1.5.4`
- UniRx `7.1.0`

## 機能

#### フォルダ構成

```
├── ObservableUI
     ├── Core
     │   ├── Interfaces
     │   ├── Components
     │   ├── Extensions
     ├── Editor
```

- `Interfaces` - Observable UI のインターフェース定義
- `Components` - Observable UI のコンポーネント実装
- `Extensions` - 拡張メソッド


#### 拡張メソッド

UniRx では [`UnityUIComponentExtensions`クラス][github: UnityUIComponentExtensions]で uGUI に対する拡張メソッド（イベントObservable化やバインディングなど）が提供されています．

本ライブラリではこれらに，以下を追加します．
- 主要コンポーネントへの双方向バインド（`BindToXXX`）
- `TextMeshPro`コンポーネントの対応

**Text** (TMP)：
```cs
stringRP.SubscribeToText(textMeshPro);
intRP.SubscribeToText(textMeshPro, value => t.text = $"{value} pt");
```

**InputField** (TMP)：
```cs

```

**Dropdown** (TMP)：
```cs

```

**Slider**：
```cs
floatRP.SubscribeToSlider(slider);  // 0~1
```

**Image**：
```cs

```

> [!note]
> 基本的にUniRx標準拡張メソッドと同様だが，`OnValueChangedAsObservable`では引数の`currentValue`で初期値でOnNextを発火するか選択できるようにしている．

> [!note]
> 単方向バインドを`SubscribeToXXX`，双方向バインドを`BindToXXX`と命名している．

#### Reactive InputField

InputField は文字列形式でユーザー入力を受け付けるため，常にパース処理を意識する必要があります．
`Reactive InputField` は文字列入力を内部で処理し，`ReactiveProperty` として外部に公開するラッパーコンポーネントです．Int や Float などのシンプルな入力用途で，拡張メソッドと合わせて使用すると便利です．


```cs
FloatReactiveInputField reactiveIF;

reactiveIF.ReactiveProperity
    .Subscribe(value => Debug.Log(value))
    .AddTo(this);
```

## 導入方法

#### UPM Package
Package Manager を開き，「git packageから追加」で `https://github.com/nitou-kanazawa/lib-unity-ObservableUI.git?path=ObservableUI/Assets/ObservableUI` を指定します．

もしくは，`Packages/manifest.json` に以下を追加してください．
```
https://github.com/nitou-kanazawa/lib-unity-ObservableUI.git?path=ObservableUI/Assets/ObservableUI
```



<!-- Link -->
[github: UniRx]: https://github.com/neuecc/UniRx/tree/master
[github: UnityUIComponentExtensions]: https://github.com/neuecc/UniRx/blob/master/Assets/Plugins/UniRx/Scripts/UnityEngineBridge/UnityUIComponentExtensions.cs