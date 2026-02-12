using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;


public class AchievementManager : MonoBehaviour
{
    // Lista de logros (podría venir de una base de datos o ScriptableObject)
    public List<Achievement> achievements = new List<Achievement>();

    public static event Action<Achievement> OnUnlockAchievement;

    private void Start()
    {
        LoadOrInitializeAchievements();
    }


    private void LoadOrInitializeAchievements()
    {
        AchievementWrapper wrapper = SaveManager.LoadAchievements();

        if (wrapper != null && wrapper.Achievements != null)
        {
            Debug.Log("Cargando progreso de achievements desde archivo...");

            // Sincronizar progreso con los del inspector
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
            Debug.Log("No existe archivo. Guardando achievements base del editor...");
            SaveManager.SaveAchievements(achievements);
        }
    }



    // --- SUSCRIPCIÓN A EVENTOS (La parte clave) ---
    private void OnEnable()
        {
            // Nos "suscribimos" a la radio
            GameEvents.OnEnemyKilled += HandleGameEvent;
            GameEvents.OnWallDestroyed += HandleGameEvent;
            GameEvents.OnLevelCompleted += HandleGameEvent;
            GameEvents.OnFoodConsumed += HandleGameEvent;
            GameEvents.OnHealthRestored += HandleGameEvent;
            GameEvents.OnChestOpened += HandleGameEvent;

        }
        private void OnDisable()
        {
            // ¡IMPORTANTÍSIMO! Desuscribirse para evitar Memory Leaks
            GameEvents.OnEnemyKilled -= HandleGameEvent;
            GameEvents.OnWallDestroyed -= HandleGameEvent;
            GameEvents.OnLevelCompleted -= HandleGameEvent;
            GameEvents.OnFoodConsumed -= HandleGameEvent;
            GameEvents.OnHealthRestored -= HandleGameEvent;
            GameEvents.OnChestOpened -= HandleGameEvent;
        }




    // --- MANEJADORES DE EVENTOS ---
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
    /*private void CheckProgress(string achievementID)  En esta parte se comprueba el progreso de un logro concreto Creo q no hace falta
        {
            // Buscamos el logro por ID
            Achievement ach = achievements.Find(a => a.ID ==
            achievementID);
            if (ach != null && !ach.IsUnlocked)
            {
                ach.CurrentCount++;
                Debug.Log($"Progreso Logro '{ach.Title}':
            { ach.CurrentCount}/{ ach.TargetCount}
                ");
            if (ach.CurrentCount >= ach.TargetCount)
                {
                    None
            UnlockAchievement(ach);
                }
            }
        }
    */

    private void UnlockAchievement(Achievement ach)
        {
            ach.IsUnlocked = true;
            Debug.Log($"<color=yellow>¡LOGRO DESBLOQUEADO:{ ach.Title}!</color>");


        // Aquí lanzarías un evento de UI para mostrar la medalla en pantalla
        // Ej: UIManager.ShowAchievementPopup(ach);

        OnUnlockAchievement?.Invoke(ach);

    }
}