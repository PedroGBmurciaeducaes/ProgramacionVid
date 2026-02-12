using UnityEngine;
using UnityEngine.UIElements;



public class PlayerCreator: MonoBehaviour
{
    PlayerStats playerData;

    public UIDocument UI;
    private VisualElement root;

    public enum ATTRIBUTES 
    {
        fuerza,resistencia,destreza,inteligencia
    }

    public void Start()
    {
        playerData = nuevoPersonaje();  

        root = UI.rootVisualElement;

        ConectarUI();
        ActualizarUI();
    }

    public PlayerStats nuevoPersonaje()
    {
        PlayerStats nuevo = new PlayerStats();
        nuevo.puntosDisponibles = 50;
        return nuevo;
    }




    void ConectarUI()
    {
        // BOTONES +
        root.Q<Button>("BtnFuerzaMas").clicked += () => SumarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("BtnResistenciaMas").clicked += () => SumarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("BtnDestrezaMas").clicked += () => SumarPunto(ATTRIBUTES.destreza);
        root.Q<Button>("BtnInteligenciaMas").clicked += () => SumarPunto(ATTRIBUTES.inteligencia);

        // BOTONES -
        root.Q<Button>("BtnFuerzaMenos").clicked += () => RestarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("BtnResistenciaMenos").clicked += () => RestarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("BtnDestrezaMenos").clicked += () => RestarPunto(ATTRIBUTES.destreza);
        root.Q<Button>("BtnInteligenciaMenos").clicked += () => RestarPunto(ATTRIBUTES.inteligencia);

        // BOTÓN CREAR PERSONAJE
        root.Q<Button>("BtnCrearPersonaje").clicked += OnCrearPersonajeClicked;

        // NOMBRE
        TextField nombreField = root.Q<TextField>("NombrePersonaje");
        nombreField.RegisterValueChangedCallback(evt =>
        {
            playerData.nombre = evt.newValue;
        });
    }


    void SumarPunto(ATTRIBUTES atributo)
    {
        if (playerData.puntosDisponibles <= 0)
            return;

        playerData.puntosDisponibles--;

        switch (atributo)
        {
            case ATTRIBUTES.fuerza:
                playerData.fuerza++;
                break;
            case ATTRIBUTES.resistencia:
                playerData.resistencia++;
                break;
            case ATTRIBUTES.destreza:
                playerData.destreza++;
                break;
            case ATTRIBUTES.inteligencia:
                playerData.inteligencia++;
                break;
        }

        ActualizarUI();
    }

    void RestarPunto(ATTRIBUTES atributo)
    {
        switch (atributo)
        {
            case ATTRIBUTES.fuerza:
                if (playerData.fuerza <= 1) return;
                playerData.fuerza--;
                break;

            case ATTRIBUTES.resistencia:
                if (playerData.resistencia <= 1) return;
                playerData.resistencia--;
                break;

            case ATTRIBUTES.destreza:
                if (playerData.destreza <= 1) return;
                playerData.destreza--;
                break;

            case ATTRIBUTES.inteligencia:
                if (playerData.inteligencia <= 1) return;
                playerData.inteligencia--;
                break;
        }

        playerData.puntosDisponibles++;
        ActualizarUI();
    }

    // =========================
    // ACTUALIZAR UI
    // =========================
    void ActualizarUI()
    {
        // ATRIBUTOS
        root.Q<Label>("FuerzaValue").text = playerData.fuerza.ToString();
        root.Q<Label>("ResistenciaValue").text = playerData.resistencia.ToString();
        root.Q<Label>("DestrezaValue").text = playerData.destreza.ToString();
        root.Q<Label>("InteligenciaValue").text = playerData.inteligencia.ToString();

        // PUNTOS
        root.Q<Label>("PuntosDisponibles").text =
            playerData.puntosDisponibles.ToString();

        // ESTADÍSTICAS CALCULADAS
        root.Q<Label>("SaludMaxima").text =
            playerData.saludMaxima.ToString();

        root.Q<Label>("Danio").text =
            playerData.daño.ToString();

        root.Q<Label>("Esquiva").text =
            playerData.esquiva.ToString();

        root.Q<Label>("Mana").text =
            playerData.mana.ToString();
    }

    private void OnCrearPersonajeClicked()
    {
        GameSesion.instance.fichaDePersonaje = playerData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }




}
