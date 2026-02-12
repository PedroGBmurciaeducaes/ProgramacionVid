using System;
using UnityEngine;
using static GameEvents;

[Serializable]
public class Achievement
{
    public string ID; // Ej: "MATAR_10"
    public string Title; // Ej: "Exterminador"
    public string Description; // Ej: "Derrota a 10 enemigos"

    public AchievementEventType EventType;      //Enum del tipo de evento nos sirve para actualizar todos los logros planos que cuentan un numero

    public int TargetCount; // Ej: 10
    public int CurrentCount; // Progreso actual
    public bool IsUnlocked; // Estado


                            // Constructor helper
    public Achievement(string id, string title, AchievementEventType eventType,   string desc,
    int target)
    {
        ID = id;
        Title = title;
        Description = desc;
        EventType = eventType;
        TargetCount = target;
        CurrentCount = 0;
        IsUnlocked = false;
    }
}