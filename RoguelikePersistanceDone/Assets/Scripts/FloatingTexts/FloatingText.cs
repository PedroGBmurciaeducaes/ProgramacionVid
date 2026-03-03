using System;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Action OnDestroyed;

    [Header("Timing")]
    public float duration = 3.5f;     // ahora dura más
    public float fadeStartPercent = 0.7f; // empieza a desaparecer al 70%

    [Header("Movimiento")]
    public float moveSpeed = 25f;     // más lento = más legible

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

        float normalizedTime = timer / duration;

        // Movimiento más suave
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade solo al final
        if (normalizedTime >= fadeStartPercent)
        {
            float fadeT = (normalizedTime - fadeStartPercent) / (1f - fadeStartPercent);
            text.color = Color.Lerp(startColor,
                new Color(startColor.r, startColor.g, startColor.b, 0f),
                fadeT);
        }

        if (timer >= duration)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}