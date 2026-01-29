using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referencia al personaje")]
    public Transform target;       // El transform del jugador

    [Header("Opciones de seguimiento")]
    public float smoothSpeed = 0.125f;  // Qué tan rápido sigue la cámara
    public Vector3 offset;              // Desfase de la cámara respecto al jugador

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Suavizar el movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicar la posición suavizada
        transform.position = smoothedPosition;

        // Mantener la cámara en el mismo plano z (importante en 2D)
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
