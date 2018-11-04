using UnityEngine;

public class TextureSlide : MonoBehaviour
{
    // Scroll main texture based on time

    public float scrollSpeedx = 0;
    public float scrollSpeedy = 0.5f;
	
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer> ();
    }

    void Update()
    {
        float offsetx = Time.time * scrollSpeedx;
        float offsety = Time.time * scrollSpeedy;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetx, offsety));
    }
}