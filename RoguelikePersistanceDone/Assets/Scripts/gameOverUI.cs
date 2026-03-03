using UnityEngine;
using UnityEngine.UIElements;

public class gameOverUI : MonoBehaviour
{
    public UIDocument UIDoc;



    private void Start()
    {
        var root = UIDoc.rootVisualElement;

        // Labels
        root.Q<Label>("turnosLabel").text =
            $"Pasos andados (turnos): {GameSesion.instance.runData.turnosJugados}";

        root.Q<Label>("enemigosLabel").text =
            $"Enemigos eliminados: {GameSesion.instance.runData.enemigosEliminados}";

        root.Q<Label>("murosLabel").text =
            $"Muros destruidos: {GameSesion.instance.runData.murosDestruidos}";

        root.Q<Label>("comidaLabel").text =
            $"Comida consumida: {GameSesion.instance.runData.comidaConsumida}";

        root.Q<Label>("nivelesLabel").text =
            $"Niveles completados: {GameSesion.instance.runData.nivelesCompletados}";

        root.Q<Label>("cofresLabel").text =
            $"Cofres abiertos: {GameSesion.instance.runData.cofresAbiertos}";

        root.Q<Label>("vidaLabel").text =
            $"Vida recuperada: {GameSesion.instance.runData.vidaRecuperada}";

        // Botón
        root.Q<Button>("menuButton").clicked += () =>
        {
            GameSesion.instance.StartNewRun();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuInicial");   // TENGO QUE RESETEAR LAS ESTADISTICAS DE CADA RUN
        };
    }
}
