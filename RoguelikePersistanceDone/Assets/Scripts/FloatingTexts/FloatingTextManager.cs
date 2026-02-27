using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [Header("Prefabs y Canvas")]
    public GameObject floatingTextPrefab; // Tu prefab de TMP
    public Canvas canvas;                  // Canvas donde se instancian los textos

    [Header("Separación y movimiento")]
    public float verticalOffset = 1.5f;      // altura inicial sobre el target (en unidades del mundo)
    public float maxRandomOffsetX = 50f;     // desviación horizontal en píxeles
    public float moveSpeed = 40f;            // velocidad vertical de subida (pixeles/seg)

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Spawnea un número flotante sobre un target.
    /// </summary>
    public void Spawn(string text, Color color, Transform target, float scale = 1f)
    {
        if (target == null) return;

        // Posición inicial en pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * verticalOffset);

        // Offset horizontal aleatorio para separar mensajes
        float offsetX = Random.Range(30, maxRandomOffsetX);
        float offsetY = Random.Range(0f, 10f); // 0 a 10 píxeles
        Vector3 finalPos = screenPos + new Vector3(offsetX, offsetY, 0);

        // Instanciar prefab como hijo del canvas
        GameObject obj = Instantiate(floatingTextPrefab, canvas.transform);
        obj.transform.position = finalPos;

        // Inicializar texto
        FloatingText ft = obj.GetComponent<FloatingText>();
        ft.moveSpeed = moveSpeed; // le pasamos la velocidad
        ft.Init(text, color, scale);

        // Callback al destruir
        ft.OnDestroyed = () =>
        {
            // Aquí puedes hacer limpieza o actualizar contadores si quieres
        };
    }
}