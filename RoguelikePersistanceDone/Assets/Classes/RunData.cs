using UnityEngine;
using static GameEvents;

public class RunData : MonoBehaviour
{
    public int turnosJugados;
    public int murosDestruidos;
    public int comidaConsumida;
    public int enemigosEliminados;
    public int nivelesCompletados;
    public int cofresAbiertos;
    public int vidaRecuperada;



    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
        GameEvents.OnWallDestroyed += OnWallDestroyed;
        GameEvents.OnLevelCompleted += OnLevelCompleted;
        GameEvents.OnFoodConsumed += OnFoodConsumed;
        GameEvents.OnHealthRestored += OnHealthRestored;
        GameEvents.OnChestOpened += OnChestOpened;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
        GameEvents.OnWallDestroyed -= OnWallDestroyed;
        GameEvents.OnLevelCompleted -= OnLevelCompleted;
        GameEvents.OnFoodConsumed -= OnFoodConsumed;
        GameEvents.OnHealthRestored -= OnHealthRestored;
        GameEvents.OnChestOpened -= OnChestOpened;
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
    public void AddTurn()
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
