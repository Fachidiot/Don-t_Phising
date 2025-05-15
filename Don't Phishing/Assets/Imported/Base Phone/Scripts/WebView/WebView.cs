using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WebView : MonoBehaviour
{
    [DllImport("WebView", EntryPoint = "InitWebView")]
    private static extern void InitWebView(IntPtr hwnd, int width, int height);

    [DllImport("WebView", EntryPoint = "GetSharedTexture")]
    private static extern IntPtr GetSharedTexture();

    [DllImport("WebView", EntryPoint = "SendMouseClick")]
    private static extern void SendMouseClick(int x, int y);

    public RawImage displayImage;
    private Texture2D webTexture;

    private void Start()
    {
        // 유니티 창 핸들 가져오기
        IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        Debug.Log($"HWND: {hwnd}, Width: {Screen.width}, Height: {Screen.height}");

        // WebView 초기화
        InitWebView(hwnd, Screen.width, Screen.height);

        // 잠시 대기
        System.Threading.Thread.Sleep( 100 );

        // 공유 텍스처 가져오기
        IntPtr sharedHandle = GetSharedTexture();
        if (sharedHandle == IntPtr.Zero)
        {
            Debug.LogError("Failed to get shared texture handle from DLL");
            return;
        }

        webTexture = Texture2D.CreateExternalTexture(Screen.width, Screen.height, TextureFormat.RGBA32, false, false, sharedHandle);
        if (webTexture == null)
        {
            Debug.LogError("Failed to create external texture");
            return;
        }

        // RawImage에 텍스처 연결
        displayImage.texture = webTexture;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;
            SendMouseClick((int)pos.x, (int)pos.y);
        }
    }
}
