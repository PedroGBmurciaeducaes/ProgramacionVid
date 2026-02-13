using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private Button btnNewGame;
    private Button btnLoadGame;
    private Button btnAchievements;
    private Button btnSettings;
    private Button btnExit;
    public UIDocument uiDocument;

    public GameObject settingsMenu;
    public GameObject achivementsMenu;
    public GameObject loadGamePanel;

    private void Awake()
    {
        // Obtener el UIDocument
        VisualElement root = uiDocument.rootVisualElement;

        // Buscar botones por name (definidos en UXML)
        btnNewGame = root.Q<Button>("btnNewGame");
        btnLoadGame = root.Q<Button>("btnLoadGame");
        btnAchievements = root.Q<Button>("btnAchievements");
        btnSettings = root.Q<Button>("btnSettings");
        btnExit = root.Q<Button>("btnExit");

        // Conectar callbacks
        btnNewGame.clicked += OnNewGameClicked;
        btnLoadGame.clicked += OnLoadGameClicked;
        btnAchievements.clicked += OnAchievementsClicked;
        btnSettings.clicked += OnSettingsClicked;
        btnExit.clicked += OnExitClicked;
    }

    private void OnDestroy()
    {
        // Buena práctica: desuscribirse
        btnNewGame.clicked -= OnNewGameClicked;
        btnLoadGame.clicked -= OnLoadGameClicked;
        btnAchievements.clicked -= OnAchievementsClicked;
        btnSettings.clicked -= OnSettingsClicked;
        btnExit.clicked -= OnExitClicked;
    }

    /* =========================
       CALLBACKS
       ========================= */

    private void OnNewGameClicked()
    {
        Debug.Log("Nueva partida iniciada");
        SceneManager.LoadScene("CreacionPersonaje");
    }

    private void OnLoadGameClicked()
    {
        loadGamePanel.SetActive(true);
        Debug.Log("Cargar partida (pendiente)");
        // TODO: abrir menú de guardados
    }

    private void OnAchievementsClicked()
    {
        achivementsMenu.SetActive(true);
        Debug.Log("Logros (pendiente)");
        // TODO: mostrar logros
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Configuración (pendiente)");
        settingsMenu.SetActive(true);
    }

    private void OnExitClicked()
    {
        Debug.Log("Salir del juego");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
