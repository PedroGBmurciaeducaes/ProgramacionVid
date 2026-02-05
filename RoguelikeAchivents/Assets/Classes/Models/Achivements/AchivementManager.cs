using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;


public class AchievementManager : MonoBehaviour
{
    // Lista de logros (podría venir de una base de datos o ScriptableObject)
    public List<Achievement> achievements = new
    List<Achievement>();

    public static event Action<Achievement> OnUnlockAchievement;

    private void Start()
        {
            // Inicializamos algunos logros de prueba
            if (achievements.Count == 0)
            {
                achievements.Add(new Achievement("KILL_5",   //Comprobar esta parte
                "Principiante 5",GameEvents.AchievementEventType.EnemyKilled, "Derrota 5 enemigos", 5));

                achievements.Add(new Achievement("KILL_5",   
                    "Principiante 10", GameEvents.AchievementEventType.EnemyKilled, "Derrota 10 enemigos", 10));

                achievements.Add(new Achievement("WALL_3",
                    "Demoledor", GameEvents.AchievementEventType.WallDestroyed, "Rompe 3 muros", 3));

                achievements.Add(new Achievement("LEVEL_2",
                "Explorador", GameEvents.AchievementEventType.LevelCompleted, "Supera 2 niveles", 2));
                
                achievements.Add(new Achievement("FOOD_1",
                "Comedor10", GameEvents.AchievementEventType.FoodConsumed, "Come 10 deliciosas comidas", 10));

                achievements.Add(new Achievement("HEALTH_1",
                 "Duro de Pelar 200", GameEvents.AchievementEventType.HealthRestored, "Cura 200 de vida comiendo deliciosas comidas", 200));
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
            private void HandleGameEvent(AchievementEventType eventType,int amount)
            {
                foreach (var ach in achievements)
                {
                    if (ach.IsUnlocked) continue;
                    if (ach.EventType != eventType) continue;

                    ach.CurrentCount += amount;

                    if (ach.CurrentCount >= ach.TargetCount)
                    {
                        UnlockAchievement(ach);
                    }
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