using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Async scene loader. Add to a persistent GameObject alongside GameManager.
///
/// Usage:
///   // Simple async load with progress and completion callbacks
///   SceneLoader.Instance.LoadAsync("GameScene",
///       progress => loadingBar.value = progress,
///       () => Debug.Log("Scene ready"));
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>Synchronous scene load. Suitable for small scenes only.</summary>
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Async scene load. The scene activates automatically once loaded.
    /// <param name="onProgress">Called each frame with 0–1 progress value.</param>
    /// <param name="onComplete">Called once the scene is active.</param>
    /// </summary>
    public void LoadAsync(string sceneName, Action<float> onProgress = null, Action onComplete = null)
    {
        StartCoroutine(LoadRoutine(sceneName, onProgress, onComplete));
    }

    IEnumerator LoadRoutine(string sceneName, Action<float> onProgress, Action onComplete)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            onProgress?.Invoke(op.progress);
            yield return null;
        }

        onProgress?.Invoke(1f);
        op.allowSceneActivation = true;
        yield return op;
        onComplete?.Invoke();
    }
}
