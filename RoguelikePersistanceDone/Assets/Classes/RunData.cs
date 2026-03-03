using UnityEngine;
using static GameEvents;

public class RunData : MonoBehaviour
{
    public int turnosJugados=0;
    public int murosDestruidos = 0;
    public int comidaConsumida = 0;
    public int enemigosEliminados = 0;
    public int nivelesCompletados = 0;
    public int cofresAbiertos = 0;
    public int vidaRecuperada = 0;



    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
        GameEvents.OnWallDestroyed += OnWallDestroyed;
        GameEvents.OnLevelCompleted += OnLevelCompleted;
        GameEvents.OnFoodConsumed += OnFoodConsumed;
        GameEvents.OnHealthRestored += OnHealthRestored;
        GameEvents.OnChestOpened += OnChestOpened;
        GameEvents. OnTurnHappened+= AddTurn;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
        GameEvents.OnWallDestroyed -= OnWallDestroyed;
        GameEvents.OnLevelCompleted -= OnLevelCompleted;
        GameEvents.OnFoodConsumed -= OnFoodConsumed;
        GameEvents.OnHealthRestored -= OnHealthRestored;
        GameEvents.OnChestOpened -= OnChestOpened;
        GameEvents.OnTurnHappened -= AddTurn;

    }

    private void OnEnemyKilled(AchievementEventType type, int amount)
    {
        enemigosEliminados += amount;
    }

    private void OnWallDestroyed(AchievementEventType type, int amount)
    {
        murosDestruidos += amount;
    }

    private void OnLevelCompleted(AchievementEventType type, int amount)
    {
        nivelesCompletados += amount;
    }

    private void OnFoodConsumed(AchievementEventType type, int amount)
    {
        comidaConsumida += amount;
    }

    private void OnHealthRestored(AchievementEventType type, int amount)
    {
        vidaRecuperada += amount;
    }

    private void OnChestOpened(AchievementEventType type, int amount)
    {
        cofresAbiertos += amount;
    }

    // Este lo puedes llamar desde TurnManager
    public void AddTurn(AchievementEventType type, int amount)
    {
        turnosJugados++;
    }

    public void Reset()
    {
        turnosJugados = 0;
        murosDestruidos = 0;
        comidaConsumida = 0;
        enemigosEliminados = 0;
        nivelesCompletados = 0;
        cofresAbiertos = 0;
        vidaRecuperada = 0;
    }
}
