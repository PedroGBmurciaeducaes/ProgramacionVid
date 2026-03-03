using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager;
    public PlayerController PlayerController;

    public GameObject player;
    public UIDocument UIDoc;
    private Label m_FoodLabel;

    public TurnManager TurnManager { get; private set; }

    private int m_maxFoodAmount;
    private int m_FoodAmount;
    private int m_ExpAmount;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private Label m_PlayerNameLabel;
    private Label keyLabel;
    private Label m_ExpLabel;




    [Header("Notificaciones")]
    public TextMeshProUGUI notificacionPers;
    public float messageDuration = 1.2f;   // Duración de cada mensaje
    public float floatHeight = 0.5f;       // Cuánto flotan hacia arriba
    public int maxMessagesOnScreen = 5;    // Limita mensajes simultáneos

    private List<TextMeshProUGUI> activeMessages = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private int m_CurrentLevel = 1;

    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_PlayerNameLabel = UIDoc.rootVisualElement.Q<Label>("PlayerNameLabel");
        keyLabel = UIDoc.rootVisualElement.Q<Label>("KeyLabel");
        m_ExpLabel = UIDoc.rootVisualElement.Q<Label>("ExpLabel");

        StartNewGame();
    }
    public void UpdateKeys(int amount)
    {
        keyLabel.text = "x" + amount.ToString();
    }

    public void UpdateExp(int amount)
    {
        m_ExpLabel.text = "EXP : " + amount.ToString();
    }


    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        UpdateKeys(GameSesion.instance.fichaDePersonaje.llaves);
        UpdateExp(GameSesion.instance.fichaDePersonaje.experiencia);

        m_maxFoodAmount = GameSesion.instance.fichaDePersonaje.saludMaxima;
        m_FoodAmount = m_maxFoodAmount;
        m_ExpAmount = GameSesion.instance.fichaDePersonaje.experiencia;

        SaveManager.SavePlayer(GameSesion.instance.fichaDePersonaje);
        Debug.Log("Nombre del personaje:" + GameSesion.instance.fichaDePersonaje.nombre);

        if (m_PlayerNameLabel != null)
        {
            m_PlayerNameLabel.text = GameSesion.instance.fichaDePersonaje.nombre;
        }

        m_CurrentLevel = 1;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        BoardManager.Clean();
        BoardManager.Init();

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        SaveManager.SavePlayer(GameSesion.instance.fichaDePersonaje);
        Debug.Log("Nombre del personaje:"+GameSesion.instance.fichaDePersonaje.nombre);


        m_CurrentLevel++;
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        if (amount > 0)
        {
            int healed = Mathf.Min(amount, m_maxFoodAmount - m_FoodAmount);
            m_FoodAmount = Mathf.Min(m_FoodAmount + amount, m_maxFoodAmount);
            m_FoodLabel.text = "Food : " + m_FoodAmount;
            ShowCombatText(MessageType.type.Heal, amount, player.transform);
        }
        else
        {
            m_FoodAmount += amount;
            m_FoodLabel.text = "Food : " + m_FoodAmount;
            ShowCombatText(MessageType.type.StarvationDamage, amount, player.transform);
            if (m_FoodAmount <= 0)
            {
                PlayerController.GameOver();
                SceneManager.LoadScene("GameOver");
                /*
                m_GameOverPanel.style.visibility = Visibility.Visible;
                m_GameOverMessage.text = "Game Over!\n\nSurvived " + m_CurrentLevel + " days";
                */
            }
        }
    }

    public void DecreaseFood(int amount)  // Para ataques enemigos, con posibilidad de esquivar
    {
        int hitProb = 100 - GameSesion.instance.fichaDePersonaje.esquiva;
        int golpeo = UnityEngine.Random.Range(0, 100);

        if (hitProb < golpeo)
        {
            GameManager.Instance.ShowCombatText(MessageType.type.Dodge, amount, player.transform);
        }
        else
        {
            m_FoodAmount -= amount;
            m_FoodLabel.text = "Food : " + m_FoodAmount;
            ShowCombatText(MessageType.type.EnemyHitPlayer, amount, player.transform);
            if (m_FoodAmount <= 0)
            {
                PlayerController.GameOver();
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");


                /*
                m_GameOverPanel.style.visibility = Visibility.Visible;
                m_GameOverMessage.text = "Game Over!\n\nSurvived " + m_CurrentLevel + " days";
                
                 */
            }
        }
    }

    public void ChangeExp(int amount)
    {
        GameSesion.instance.fichaDePersonaje.experiencia += amount;

        UpdateExp(GameSesion.instance.fichaDePersonaje.experiencia);    

        GameManager.Instance.ShowCombatText(MessageType.type.ExpGain, amount, player.transform);
    }


    // ======================================
    // Sistema de mensajes flotantes con cola
    // ======================================








    public void ShowCombatText(MessageType.type type, int amount, Transform target)
    {
        string text = "";
        Color color = Color.white;
        float scale = 1f;

        switch (type)
        {
            case MessageType.type.Heal:
                text = "+" + amount +" Healed";
                color = Color.green;
                break;

            case MessageType.type.EnemyHitPlayer:
                text = "-" + amount + " DMG recived";
                color = new Color(1f, 0.2f, 0.2f);
                scale = 1.1f;
                break;

            case MessageType.type.PlayerHitEnemy:
                text = amount.ToString()+" Dealed to enemy";
                color = Color.cyan;
                break;

            case MessageType.type.StarvationDamage:
                text = amount + "HP lost" ;
                color = new Color(1f, 0.5f, 0.1f);
                break;

            case MessageType.type.Dodge:
                text = "DODGE";
                color = Color.yellow;
                scale = 1.2f;
                break;

            case MessageType.type.CriticalHit:
                text = "CRIT " + amount;
                color = Color.magenta;
                scale = 1.5f;
                break;

            case MessageType.type.LevelUp:
                text = "LEVEL UP!";
                color = Color.green;
                scale = 1.4f;
                break;

            case MessageType.type.ExpGain:
                text = "+" + amount + " EXP";
                color = Color.blue;
                break;

            case MessageType.type.ChestOpened:
                text = "Chest Unlocked";
                color = Color.magenta;
                break;

            case MessageType.type.NeedKey:
                text = "Need a Key to open this";
                color = Color.magenta;
                break;
            case MessageType.type.llaveObtenida:
                text = "Key Obtained";
                color = Color.magenta;
                break;
        }

        FloatingTextManager.Instance.Spawn(
            text,
            color,
            target,
            scale,
            type
        );
    }


    private IEnumerator FloatingMessageCoroutine(string text, Color color, MessageType.type type)
    {
        if (activeMessages.Count >= maxMessagesOnScreen)
        {
            Destroy(activeMessages[0].gameObject);
            activeMessages.RemoveAt(0);
        }

        TextMeshProUGUI msg = Instantiate(notificacionPers, notificacionPers.transform.parent);
        msg.gameObject.SetActive(true);
        msg.text = text;
        msg.color = color;

        float yOffset = activeMessages.Count * 25f;
        Vector3 startPos = notificacionPers.transform.localPosition + Vector3.up * yOffset;
        msg.transform.localPosition = startPos;

        activeMessages.Add(msg);

        float elapsed = 0f;
        float duration = messageDuration;

        Vector3 originalScale = Vector3.one;

        // Efectos especiales según tipo
        if (type == MessageType.type.CriticalHit)
        {
            originalScale = Vector3.one * 1.4f;
            msg.transform.localScale = originalScale;
            duration *= 1.2f;
        }

        if (type == MessageType.type.EnemyHitPlayer)
        {
            msg.fontStyle = FontStyles.Bold;
        }

        Color startColor = color;
        Color endColor = new Color(color.r, color.g, color.b, 0f);

        while (elapsed < duration)
        {
            if (msg == null) yield break;

            float t = elapsed / duration;

            float verticalSpeed = (type == MessageType.type.EnemyHitPlayer) ? floatHeight * 1.5f : floatHeight;

            msg.transform.localPosition = startPos + Vector3.up * verticalSpeed * t;
            msg.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        activeMessages.Remove(msg);
        Destroy(msg.gameObject);
    }
}
