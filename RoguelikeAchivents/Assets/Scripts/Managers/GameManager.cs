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

    public UIDocument UIDoc;
    private Label m_FoodLabel;

    public TurnManager TurnManager { get; private set; }

    private int m_maxFoodAmount;
    private int m_FoodAmount;
    private int m_ExpAmount;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private Label m_PlayerNameLabel; 



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

        StartNewGame();
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_maxFoodAmount = GameSesion.instance.fichaDePersonaje.saludMaxima;
        m_FoodAmount = m_maxFoodAmount;
        m_ExpAmount = GameSesion.instance.fichaDePersonaje.experiencia;

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
            SpawnFloatingMessage("+" + healed + "HP", Color.green);
        }
        else
        {
            m_FoodAmount += amount;
            m_FoodLabel.text = "Food : " + m_FoodAmount;
            SpawnFloatingMessage(amount + "HP", Color.red);

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

    public void DecreaseFood(int amount)
    {
        int hitProb = 100 - GameSesion.instance.fichaDePersonaje.esquiva;
        int golpeo = UnityEngine.Random.Range(0, 100);

        if (hitProb < golpeo)
        {
            SpawnFloatingMessage("Dodged!", Color.yellow);
        }
        else
        {
            m_FoodAmount -= amount;
            m_FoodLabel.text = "Food : " + m_FoodAmount;
            SpawnFloatingMessage("-" + amount + "HP", Color.red);

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
        m_ExpAmount += amount;
        Debug.Log("Experience Points: " + m_ExpAmount);
    }


    // ======================================
    // Sistema de mensajes flotantes con cola
    // ======================================
    private void SpawnFloatingMessage(string text, Color color)
    {
        StartCoroutine(FloatingMessageCoroutine(text, color));
    }

    private IEnumerator FloatingMessageCoroutine(string text, Color color)
    {
        // Limitar mensajes activos
        if (activeMessages.Count >= maxMessagesOnScreen)
        {
            Destroy(activeMessages[0].gameObject);
            activeMessages.RemoveAt(0);
        }

        // Crear instancia del TMP
        TextMeshProUGUI msg = Instantiate(notificacionPers, notificacionPers.transform.parent);
        msg.gameObject.SetActive(true);
        msg.text = text;
        msg.color = color;

        // Offset inicial según cantidad de mensajes activos
        float yOffset = activeMessages.Count * 0.3f;
        Vector3 startPos = notificacionPers.transform.localPosition + Vector3.up * yOffset;
        msg.transform.localPosition = startPos;

        activeMessages.Add(msg);

        float elapsed = 0f;
        Color startColor = color;
        Color endColor = new Color(color.r, color.g, color.b, 0f);

        while (elapsed < messageDuration)
        {
            if (msg == null) yield break; // Sale si el objeto fue destruido

            float t = elapsed / messageDuration;
            msg.transform.localPosition = startPos + Vector3.up * floatHeight * t;
            msg.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        activeMessages.Remove(msg);
        Destroy(msg.gameObject);
    }
}
