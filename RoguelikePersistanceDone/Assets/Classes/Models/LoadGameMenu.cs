using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadGameMenu : MonoBehaviour
{
    public UIDocument uiDocument;

    private VisualElement root;
    private ScrollView saveList;
    private Button btnClose;

    void OnEnable()
    {

        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        saveList = root.Q<ScrollView>("SaveList");
        btnClose = root.Q<Button>("BtnClose");

        btnClose.clicked += Volver;


        RefrescarListaArchivos();
    }

    void RefrescarListaArchivos()
    {
        saveList.Clear();

        string path = Application.persistentDataPath;
        if (!Directory.Exists(path)) return;

        string[] files = Directory.GetFiles(path, "*.json");

        foreach (string fullPath in files)
        {
            string fileName = Path.GetFileName(fullPath);

            if (fileName == "global_achievements.json")
                continue;

            // Contenedor horizontal
            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.AddToClassList("save-container");

            // Botón Cargar
            Button btnLoad = new Button(() => CargarPartida(fullPath));
            btnLoad.text = fileName;
            btnLoad.AddToClassList("save-button");

            // Botón Eliminar
            Button btnDelete = new Button(() => EliminarPartida(fullPath));
            btnDelete.text = "X";
            btnDelete.AddToClassList("delete-button");

            container.Add(btnLoad);
            container.Add(btnDelete);

            saveList.Add(container);
        }
    }

    void CargarPartida(string path)
    {
        PlayerStats datos = SaveManager.LoadPlayer(path);

        GameSesion.instance.fichaDePersonaje = datos;
        SceneManager.LoadScene("Main");
    }

    void EliminarPartida(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        RefrescarListaArchivos();
    }



    void Volver()
    {
        gameObject.SetActive(false);
    }
}
