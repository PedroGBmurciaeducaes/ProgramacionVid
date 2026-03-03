using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [Header("Referencias")]
    public GameObject floatingTextPrefab;
    public Canvas canvas;

    [Header("Offsets base por tipo (mundo)")]
    public Vector3 foodOffset = new Vector3(0.8f, 0.5f, 0);
    public Vector3 enemyHitOffset = new Vector3(0f, 1.8f, 0);
    public Vector3 dodgeOffset = new Vector3(0f, 2.4f, 0);
    public Vector3 playerHitEnemyOffset = new Vector3(0f, 1.6f, 0);
    public Vector3 critOffset = new Vector3(0f, 2.6f, 0);
    public Vector3 chests = new Vector3(0f, 2.6f, 0); // Ya que no tenemos criticos, utilizamos el mismo offset para los cofres
    public Vector3 expOffset = new Vector3(0f, 3f, 0);
    public Vector3 healOffset = new Vector3(0f, 1.5f, 0);
    public Vector3 levelUpOffset = new Vector3(0f, 3.5f, 0);

    [Header("Separación vertical en pantalla")]
    public float stackSpacing = 25f;

    // Clave = targetID + tipo
    private Dictionary<string, int> activeStacks = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(string text, Color color, Transform target, float scale, MessageType.type type)
    {
        if (target == null) return;

        //  Obtener offset base por tipo
        Vector3 baseOffset = GetOffsetByType(type);

        //  Crear clave única por zona
        string stackKey = target.GetInstanceID() + "_" + type;

        int stackIndex = 0;

        if (activeStacks.ContainsKey(stackKey))
        {
            stackIndex = activeStacks[stackKey];
            activeStacks[stackKey]++;
        }
        else
        {
            activeStacks.Add(stackKey, 1);
        }

        //  Posición base en pantalla
        Vector3 worldPos = target.position + baseOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        //  Aplicar desplazamiento vertical solo si hay más de uno
        screenPos.y += stackSpacing * stackIndex;

        GameObject obj = Instantiate(floatingTextPrefab, canvas.transform);
        obj.transform.position = screenPos;

        FloatingText ft = obj.GetComponent<FloatingText>();
        ft.Init(text, color, scale);

        //  Cuando desaparece, reducimos pila
        ft.OnDestroyed = () =>
        {
            if (activeStacks.ContainsKey(stackKey))
            {
                activeStacks[stackKey]--;

                if (activeStacks[stackKey] <= 0)
                    activeStacks.Remove(stackKey);
            }
        };
    }

    private Vector3 GetOffsetByType(MessageType.type type)
    {
        switch (type)
        {
            case MessageType.type.Heal: return healOffset;
            case MessageType.type.EnemyHitPlayer: return enemyHitOffset;
            case MessageType.type.PlayerHitEnemy: return playerHitEnemyOffset;
            case MessageType.type.StarvationDamage: return foodOffset;
            case MessageType.type.Dodge: return dodgeOffset;
            case MessageType.type.CriticalHit: return critOffset;
            case MessageType.type.ChestOpened: return chests;
            case MessageType.type.NeedKey: return chests;
            case MessageType.type.LevelUp: return levelUpOffset;
            case MessageType.type.ExpGain: return expOffset;
            case MessageType.type.llaveObtenida: return chests;
            default: return Vector3.up * 2f;
        }
    }
}