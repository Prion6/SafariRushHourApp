using System.Runtime.InteropServices;
using UnityEngine;

public class UnityWebBridge
{
    [DllImport("__Internal")]
    private static extern void closewindow();

    public static void QuitAndClose()
    {
        Application.Quit();
        closewindow();
    }
}
