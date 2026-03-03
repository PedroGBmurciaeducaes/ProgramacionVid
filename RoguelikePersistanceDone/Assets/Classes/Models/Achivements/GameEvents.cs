using System;
using UnityEngine;

public static class GameEvents
{

    public enum AchievementEventType
    {
        EnemyKilled,
        WallDestroyed,
        LevelCompleted,
        FoodConsumed,
        HealthRestored,
        ChestsOpened,
        TurnHappened
    }


    public static event Action <AchievementEventType, int> OnEnemyKilled;
    public static event Action <AchievementEventType, int> OnFoodConsumed;
    public static event Action <AchievementEventType, int> OnWallDestroyed;
    public static event Action<AchievementEventType, int> OnLevelCompleted;
    public static event Action<AchievementEventType, int> OnHealthRestored;
    public static event Action<AchievementEventType, int> OnChestOpened;
    public static event Action<AchievementEventType, int> OnTurnHappened;





    // Métodos seguros para disparar los eventos (evita
    //errores si nadie escucha)
    public static void TriggerEnemyKilled() =>
        OnEnemyKilled?.Invoke(AchievementEventType.EnemyKilled, 1);

        public static void TriggerWallDestroyed() =>
        OnWallDestroyed?.Invoke(AchievementEventType.WallDestroyed, 1);

        public static void TriggerLevelCompleted() =>
        OnLevelCompleted?.Invoke(AchievementEventType.LevelCompleted, 1);

        public static void TriggerFoodConsumed() =>
        OnFoodConsumed?.Invoke(AchievementEventType.FoodConsumed, 1);

        public static void TriggerHealthRestored(int amount) =>
        OnHealthRestored?.Invoke(AchievementEventType.HealthRestored, amount);

        public static void TriggerChestOpened() =>
        OnChestOpened?.Invoke(AchievementEventType.ChestsOpened, 1);

        public static void TriggerTunrHappened() =>
         OnTurnHappened?.Invoke(AchievementEventType.TurnHappened, 1);


}
