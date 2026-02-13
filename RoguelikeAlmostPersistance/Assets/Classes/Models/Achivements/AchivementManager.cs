using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Achievements definidos en inspector")]
    public List<Achievement> achievements = new List<Achievement>();

    public static event Action<Achievement> OnUnlockAchievement;

    private void Awake()
    {
        // Singleton real
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadOrInitializeAchievements();
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleGameEvent;
        GameEvents.OnWallDestroyed += HandleGameEvent;
        GameEvents.OnLevelCompleted += HandleGameEvent;
        GameEvents.OnFoodConsumed += HandleGameEvent;
        GameEvents.OnHealthRestored += HandleGameEvent;
        GameEvents.OnChestOpened += HandleGameEvent;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleGameEvent;
        GameEvents.OnWallDestroyed -= HandleGameEvent;
        GameEvents.OnLevelCompleted -= HandleGameEvent;
        GameEvents.OnFoodConsumed -= HandleGameEvent;
        GameEvents.OnHealthRestored -= HandleGameEvent;
        GameEvents.OnChestOpened -= HandleGameEvent;
    }

    private void LoadOrInitializeAchievements()
    {
        AchievementWrapper wrapper = SaveManager.LoadAchievements();

        if (wrapper != null && wrapper.Achievements != null)
        {
            Debug.Log("Cargando progreso de achievements desde archivo...");

            foreach (var saved in wrapper.Achievements)
            {
                Achievement editorAch = achievements.Find(a => a.ID == saved.ID);

                if (editorAch != null)
                {
                    editorAch.CurrentCount = saved.CurrentCount;
                    editorAch.IsUnlocked = saved.IsUnlocked;
                }
            }
        }
        else
        {
            Debug.Log("No existe archivo. Guardando achievements base...");
            SaveManager.SaveAchievements(achievements);
        }
    }

    private void HandleGameEvent(GameEvents.AchievementEventType eventType, int amount)
    {
        bool changed = false;

        foreach (var ach in achievements)
        {
            if (ach.IsUnlocked) continue;
            if (ach.EventType != eventType) continue;

            ach.CurrentCount += amount;
            changed = true;

            if (ach.CurrentCount >= ach.TargetCount)
            {
                UnlockAchievement(ach);
            }
        }

        if (changed)
        {
            SaveManager.SaveAchievements(achievements);
        }
    }

    private void UnlockAchievement(Achievement ach)
    {
        ach.IsUnlocked = true;

        Debug.Log($"<color=yellow>¡LOGRO DESBLOQUEADO: {ach.Title}!</color>");

        SaveManager.SaveAchievements(achievements);

        OnUnlockAchievement?.Invoke(ach);
    }
}
