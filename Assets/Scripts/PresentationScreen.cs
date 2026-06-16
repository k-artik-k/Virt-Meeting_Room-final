using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PresentationScreen : MonoBehaviourPun
{
    [Header("Slides")]
    public Texture2D[] slides;
    private int currentIndex = 0;
    private Renderer screenRenderer;

    void Start()
    {
        screenRenderer = GetComponent<Renderer>();
        if (slides != null && slides.Length > 0)
            ShowSlide(0);
    }

    public void NextSlide()
    {
        photonView.RPC("ChangeSlide", RpcTarget.AllBuffered, (currentIndex + 1) % slides.Length);
    }

    public void PrevSlide()
    {
        int prev = currentIndex - 1;
        if (prev < 0) prev = slides.Length - 1;
        photonView.RPC("ChangeSlide", RpcTarget.AllBuffered, prev);
    }

    [PunRPC]
    void ChangeSlide(int index)
    {
        currentIndex = index;
        ShowSlide(index);
    }

    void ShowSlide(int index)
    {
        if (slides == null || slides.Length == 0) return;
        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = slides[index];
        screenRenderer.material = mat;
    }
}