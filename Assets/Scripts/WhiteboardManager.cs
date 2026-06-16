using UnityEngine;
using Photon.Pun;

public class WhiteboardManager : MonoBehaviourPun
{
    public int textureWidth = 1024;
    public int textureHeight = 512;
    public int brushSize = 10;

    private Texture2D boardTexture;
    private Renderer boardRenderer;

    void Start()
    {
        boardRenderer = GetComponent<Renderer>();

        boardTexture = new Texture2D(textureWidth, textureHeight);

        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;

        boardTexture.SetPixels(pixels);
        boardTexture.Apply();

        boardRenderer.material.mainTexture = boardTexture;
    }

    public void Draw(Vector2 uv)
    {
        int x = (int)(uv.x * textureWidth);
        int y = (int)(uv.y * textureHeight);

        photonView.RPC("DrawRPC", RpcTarget.AllBuffered, x, y);
    }

    [PunRPC]
    void DrawRPC(int x, int y)
    {
        int half = brushSize / 2;

        for (int i = -half; i < half; i++)
        {
            for (int j = -half; j < half; j++)
            {
                int px = Mathf.Clamp(x + i, 0, textureWidth - 1);
                int py = Mathf.Clamp(y + j, 0, textureHeight - 1);

                boardTexture.SetPixel(px, py, Color.black);
            }
        }

        boardTexture.Apply();
    }
}