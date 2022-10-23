#if UNITY_EDITOR
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class EditorHelper : MonoBehaviour
{
    [MenuItem("Assets/Open C# Project - VSCode")]
    public static void OpenVscode(MenuCommand command)
    {
        var process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "code",
            Arguments = $"\"{Path.GetDirectoryName(Application.dataPath)}\"",
        };

        try
        {
            process.Start();
        }
        catch
        {
            EditorUtility.DisplayDialog(
                "Open C# Project - VSCode",
                "Could not find vscode executable",
                "Close"
            );
        }
    }
}
#endif
