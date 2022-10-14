using TMPro;
using System;
using System.Linq;
using UnityEngine;
using Core.Singleton;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    TextMeshProUGUI debugTextArea = null;

    [SerializeField]
    bool enableDebug = true;

    [SerializeField]
    int maxLines = 15;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        if(debugTextArea == null)
        {
            debugTextArea = GetComponent<TextMeshProUGUI>();
        }
        debugTextArea.text = string.Empty;
    }

    // This function is called when the object becomes enabled and active.
    private void OnEnable()
    {
        debugTextArea.enabled = enableDebug;
        enabled = enableDebug;

        if(enabled)
        {
            debugTextArea.text += $"<color=\"white\">{DateTime.Now.ToString("HH:mm:ss.fff")} {this.GetType().Name} enabled</color>\n";
        }
    }

    public void LogInfo(string message)
    {
        ClearLines();
        debugTextArea.text += $"<color=\"green\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    public void LogError(string message)
    {
        ClearLines();
        debugTextArea.text += $"<color=\"red\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    public void LogWarning(string message)
    {
        ClearLines();
        debugTextArea.text += $"<color=\"yellow\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    void ClearLines()
    {
        if(debugTextArea.text.Split('\n').Count() >= maxLines)
        {
            debugTextArea.text = string.Empty;
        }
    }
}
