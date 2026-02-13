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

            Button btn = new Button(() => CargarPartida(fullPath));
            btn.text = fileName;
            btn.AddToClassList("save-button");

            saveList.Add(btn);
        }
    }

    void CargarPartida(string path)
    {
        PlayerStats datos = SaveManager.LoadPlayer(path);

        GameSesion.instance.fichaDePersonaje = datos;
        SceneManager.LoadScene("Main");
    }

    void Volver()
    {
        gameObject.SetActive(false);
    }
}
