using System.Collections;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [Header("Display Settings")]
    public bool showOnScreen = true;
    public bool logToConsole = false;
    public float updateInterval = 0.5f;
    
    [Header("UI Settings")]
    public int fontSize = 20;
    public Color textColor = Color.white;
    public Vector2 screenPosition = new Vector2(10, 10);
    
    private float fps;
    private float deltaTime;
    private GUIStyle guiStyle;
    
    void Start()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = fontSize;
        guiStyle.normal.textColor = textColor;
        if (logToConsole)
        {
            StartCoroutine(LogFPSToConsole());
        }
    }
    
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }
    
    void OnGUI()
    {
        if (!showOnScreen) return;

        string fpsText = string.Format("FPS: {0:0.0}", fps);
        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 200, 30), fpsText, guiStyle);
    }
    
    IEnumerator LogFPSToConsole()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            Debug.Log($"FPS: {fps:F1} | Frame Time: {deltaTime * 1000:F1}ms");
        }
    }
    public float GetCurrentFPS()
    {
        return fps;
    }
    
    public void ToggleDisplay()
    {
        showOnScreen = !showOnScreen;
    }
}