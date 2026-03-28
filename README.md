# unity-base

Unity 汎用プロジェクトテンプレート (Unity 2022.3 LTS)

外部アセット不要・手続き生成ベースのスターターテンプレートです。

## 含まれているもの

### Scripts/Game/

| ファイル | 内容 |
|---------|------|
| `GameManager.cs` | シングルトン。ゲームの開始・終了・リスタート・メインメニュー遷移を管理 |
| `SaveSystem.cs` | PlayerPrefs ベースのセーブ/ロード。レベル進行度とベストタイムを保存 |
| `AudioManager.cs` | シングルトン。SFX・BGM の再生管理。効果音は手続き生成 |
| `ScoreManager.cs` | シングルトン。スコア加算・コンボ管理・合計スコア計算 |

### Scripts/UI/

| ファイル | 内容 |
|---------|------|
| `UIManager.cs` | OnGUI ベースのシンプルな UI 表示 (GAME OVER / STAGE CLEAR) |

## 主な API

```csharp
// ゲーム制御
GameManager.Instance.GameOver();
GameManager.Instance.LevelClear();
GameManager.Instance.RestartLevel();
GameManager.Instance.LoadMainMenu();

// セーブ
SaveSystem.UnlockLevel(2);
SaveSystem.SaveLevelStats(1, elapsedTime);
float best = SaveSystem.GetLevelBestTime(1);

// スコア
ScoreManager.Instance.RegisterScore(100);
int total = ScoreManager.Instance.GetTotalScore();

// サウンド
AudioManager.Instance.PlaySFX(clip);
AudioManager.Instance.PlayFootstep();
```

## 使い方

1. このリポジトリをクローンして Unity 2022.3 LTS で開く
2. 新規シーンを作成し、`GameManager`・`AudioManager`・`ScoreManager`・`UIManager` を空の GameObject にアタッチ
3. ゲーム固有のスクリプトを `Assets/Scripts/` 以下に追加していく

## 動作環境

- Unity 2022.3 LTS
- 外部パッケージ不要
