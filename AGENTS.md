# AGENTS.md — unity-base

Agent guidance for this repository. Read this before starting any work.

## Environment

**Windows + Git Bash only. Never use WSL-style `/mnt/` paths.**
- `pwd` in Git Bash returns `/e/workspace/...` — this is normal, not an error
- Pass paths to Windows executables using `pwd -W` to get `E:/workspace/...`
- Do not generate `/mnt/`, WSL, or Linux filesystem assumptions

## Project Overview

A generic Unity project template (Unity 2022.3 LTS).
No external assets required — all audio and effects are procedurally generated.
Intended as a starting point for new games.

## Directory Structure

```
unity-base/
├── Assets/
│   ├── Scenes/              # Add scene files here
│   └── Scripts/
│       ├── Game/            # Core systems (singletons, utilities)
│       │   ├── GameManager.cs   # Game loop, Pause/Resume, scene transitions
│       │   ├── SaveSystem.cs    # PlayerPrefs-based save/load
│       │   ├── AudioManager.cs  # SFX and BGM playback
│       │   ├── ScoreManager.cs  # Score and combo tracking
│       │   ├── EventBus.cs      # Type-safe static event bus
│       │   └── SceneLoader.cs   # Async scene loading
│       ├── UI/
│       │   └── UIManager.cs     # Simple OnGUI-based UI
│       └── Util/
│           └── ObjectPool.cs    # Generic object pool
├── Packages/
│   └── manifest.json        # Unity package dependencies
├── ProjectSettings/
└── unity.ps1                # Unity launcher (open / compile)
```

## Unity

- Unity 2022.3 LTS
- Launcher / compile check: `powershell -ExecutionPolicy Bypass -File unity.ps1 <action>`
  - `unity.ps1 open` (default) — open the Editor
  - `unity.ps1 compile` — batch-mode compile, reports `error CS` lines from `E:\tmp\unity-build.log`
- `unity.ps1 compile` automatically stops any running Unity.exe and removes the project lockfile before starting batch mode.

## Verifying Changes

After any code change, run `unity.ps1 compile`. **Zero `error CS` lines required before submitting.**

## Coding Conventions

### General Rules
- **No namespaces** — all classes live in global scope
- **MonoBehaviour singletons** follow this exact pattern:
  ```csharp
  public static Foo Instance { get; private set; }
  void Awake() {
      if (Instance != null && Instance != this) { Destroy(gameObject); return; }
      Instance = this;
  }
  ```
- **Avoid `FindObjectOfType<T>()`** — use `EventBus` for cross-system communication instead
- **Comments only where logic is non-obvious** — no API description comments

### Cross-System Communication
Use `EventBus` to keep systems decoupled:
```csharp
// Define event (prefer structs)
struct PlayerDiedEvent { public int score; }

// Subscribe (manage in OnEnable/OnDisable)
void OnEnable()  => EventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
void OnDisable() => EventBus.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
void OnPlayerDied(PlayerDiedEvent e) { ... }

// Publish
EventBus.Publish(new PlayerDiedEvent { score = 500 });
```

### Scene Loading
Use synchronous `SceneManager.LoadScene` only for small scenes.
Otherwise use `SceneLoader`:
```csharp
SceneLoader.Instance.LoadAsync("GameScene", progress => { }, () => Debug.Log("Done"));
```

### Object Pooling
Use `ObjectPool` instead of `Instantiate/Destroy` in hot paths:
```csharp
ObjectPool.Instance.Prewarm("Bullet", bulletPrefab, 20);
GameObject obj = ObjectPool.Instance.Get("Bullet", pos, rot);
ObjectPool.Instance.Return("Bullet", obj);
```

## System Quick Reference

| Class | Key API |
|-------|---------|
| `GameManager` | `.GameOver()` `.LevelClear()` `.Pause()` `.Resume()` `.RestartLevel()` `.LoadMainMenu()` |
| `SaveSystem` | `UnlockLevel(n)` `SaveLevelStats(n, time)` `GetLevelBestTime(n)` `ClearAll()` |
| `AudioManager` | `.PlaySFX(clip)` `.PlayFootstep()` |
| `ScoreManager` | `.RegisterScore(pts)` `.GetTotalScore()` `.ResetScore()` |
| `EventBus` | `Subscribe<T>(handler)` `Unsubscribe<T>(handler)` `Publish<T>(data)` `Clear()` |
| `SceneLoader` | `.Load(name)` `.LoadAsync(name, onProgress, onComplete)` |
| `ObjectPool` | `.Prewarm(key, prefab, n)` `.Get(key, pos, rot)` `.Return(key, obj)` |
| `UIManager` | `.ShowGameOver()` `.ShowStageClear()` |

## Adding New Features

1. Place game systems in `Scripts/Game/`, utilities in `Scripts/Util/`
2. Use the singleton pattern above for manager classes
3. Communicate between systems via `EventBus`, avoid direct references
4. Run `unity.ps1 compile` before submitting — zero errors required
5. Do not manually create `.meta` files — Unity generates them automatically

## What Not To Do

- Do not `Resources.Load()` heavily (increases build size and load times)
- Do not call `FindObjectOfType<T>()` in `Update()`
- Do not use `DontDestroyOnLoad` outside of singleton managers
- Do not use `System.Threading.Thread` directly — Unity API is main-thread only
