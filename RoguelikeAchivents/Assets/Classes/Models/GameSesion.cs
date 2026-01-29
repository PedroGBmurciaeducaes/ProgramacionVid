using UnityEngine;

public class GameSesion : MonoBehaviour
{
    public PlayerStats fichaDePersonaje;
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
    }

}
