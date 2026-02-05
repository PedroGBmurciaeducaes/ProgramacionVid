using UnityEngine;

public class GameSesion : MonoBehaviour
{
    public PlayerStats fichaDePersonaje;
    public RunData runData;
    public static GameSesion instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Asegurarse de que GameOverManager está asignado
        if (runData == null)
            runData = GetComponent<RunData>();
    }

    public void StartNewRun()
    {
        runData.Reset();
    }
}
