using System;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Action OnDestroyed; // <- Esto permite asignar un callback desde el manager

    public float duration = 1f;
    public float moveSpeed = 40f;

    private TextMeshProUGUI text;
    private Color startColor;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Init(string message, Color color, float scale)
    {
        text.text = message;
        text.color = color;
        startColor = color;
        transform.localScale = Vector3.one * scale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // Movimiento hacia arriba constante
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out
        text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0f), t);

        if (timer >= duration)
        {
            OnDestroyed?.Invoke(); // <- Llama al callback si existe
            Destroy(gameObject);
        }
    }
}