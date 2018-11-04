using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingEmission : MonoBehaviour
{
  public float Speed;
  public Color baseColor;

  private Renderer blinkRenderer;
  private Material mat;

  private void Start()
  {
    blinkRenderer = GetComponent<Renderer>();
    mat = blinkRenderer.material;
  }

  private void OnDestroy()
  {
    Destroy(mat);
  }

  private void Update()
  {
    float emission = Mathf.PingPong(Time.time * Speed, 1.0f);
    Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
    mat.SetColor("_EmissionColor", finalColor);
  }
}