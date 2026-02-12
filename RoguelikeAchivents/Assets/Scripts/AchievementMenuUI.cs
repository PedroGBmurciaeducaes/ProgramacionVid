using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class AchievementMenuUI : MonoBehaviour
{
    public UIDocument uiDocument;

    //  Referencia al manager en escena
    public AchievementManager achievementManager;

    private VisualElement root;
    private ScrollView achievementList;
    private Button btnClose;

    void OnEnable()
    {
        root = uiDocument.rootVisualElement;

        achievementList = root.Q<ScrollView>("AchievementList");
        btnClose = root.Q<Button>("BtnClose");

        btnClose.clicked += CloseMenu;

        RefreshUI();

        // Actualizar en tiempo real cuando se desbloquee uno
        AchievementManager.OnUnlockAchievement += OnAchievementUnlocked;
    }

    void OnDisable()
    {
        AchievementManager.OnUnlockAchievement -= OnAchievementUnlocked;
    }

    void OnAchievementUnlocked(Achievement ach)
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        achievementList.Clear();

        if (achievementManager == null)
        {
            Debug.LogError("AchievementManager no asignado en el inspector.");
            return;
        }

        List<Achievement> achievements = achievementManager.achievements;

        foreach (var ach in achievements)
        {
            achievementList.Add(CreateAchievementElement(ach));
        }
    }

    VisualElement CreateAchievementElement(Achievement ach)
    {
        VisualElement container = new VisualElement();
        container.AddToClassList("achievement-item");

        if (ach.IsUnlocked)
            container.AddToClassList("unlocked");

        // Título
        Label title = new Label(ach.Title);
        title.AddToClassList("achievement-title");

        // Descripción
        Label desc = new Label(ach.Description);
        desc.AddToClassList("achievement-desc");

        // Progreso texto
        int current = Mathf.Min(ach.CurrentCount, ach.TargetCount);
        Label progressText = new Label($"{current} / {ach.TargetCount}");
        progressText.AddToClassList("progress-label");

        // Barra fondo
        VisualElement barBg = new VisualElement();
        barBg.AddToClassList("progress-bar-background");

        // Barra relleno
        VisualElement barFill = new VisualElement();
        barFill.AddToClassList("progress-bar-fill");

        float percent = (float)ach.CurrentCount / ach.TargetCount;
        percent = Mathf.Clamp01(percent);

        barFill.style.width = Length.Percent(percent * 100);

        barBg.Add(barFill);

        container.Add(title);
        container.Add(desc);
        container.Add(progressText);
        container.Add(barBg);

        return container;
    }

    void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
