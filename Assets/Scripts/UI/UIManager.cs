using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool showGameOver;
    private bool showStageClear;

    public void ShowGameOver() { showGameOver = true; }
    public void ShowStageClear() { showStageClear = true; }

    void OnGUI()
    {
        if (showGameOver)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 64;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "GAME OVER", style);
        }

        if (showStageClear)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 64;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.yellow;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "STAGE CLEAR!", style);
        }
    }
}
