using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public string nombre { get; set; }
    [field: SerializeField]
    public int fuerza { get; set; }
    [field: SerializeField]
    public int resistencia { get; set; }

    [field: SerializeField]
    public int destreza { get; set; }

    [field: SerializeField]
    public int inteligencia { get; set; }

    [field: SerializeField]
    public int puntosDisponibles { get; set; }

    [field: SerializeField]
    public int experiencia { get; set; }

    public int saludMaxima => resistencia * 10 + fuerza / 2;
    public int daño => fuerza / 2;

    public int esquiva => destreza/ 2;

    public int mana => inteligencia * 10 + resistencia / 2;

    public PlayerStats()
    {

        fuerza = 1;
        resistencia = 1;
        destreza = 1;
        inteligencia = 1;
        puntosDisponibles = 0;
        experiencia = 0;
    }

    public PlayerStats(string nombre):this()
    { 
        this.nombre = nombre;
        
    }


}
