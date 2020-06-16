using UnityEngine;
using UnityEngine.UI;

public class DebugWriter : MonoBehaviour
{
    private static DebugWriter Instance { get; set; }

    public Text text;

    private void Awake()
    {
        Instance = this;
    }
	
	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string condition, string stacktrace, LogType type)
	{
		if (type == LogType.Exception)
			text.text += "\nEXCEPTION: ";

		text.text += condition + "\n";
	}
}