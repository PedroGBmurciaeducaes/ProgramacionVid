using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    [field: SerializeField]
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

    [field: SerializeField]
    public int llaves { get; set; }


    public int saludMaxima => resistencia * 10 + fuerza / 2;
    public int daño => fuerza / 2;

    public int esquiva => destreza/ 2;

    public int mana => inteligencia * 10 + resistencia / 2;


    // Estadisticas de logros
    public int murosDestruidos { get; set; }
    public int enemigosDerrotados { get; set; }
    public int nivelesCompletados { get; set; }




    public PlayerStats()
    {
        fuerza = 1;
        resistencia = 1;
        destreza = 1;
        inteligencia = 1;
        puntosDisponibles = 0;
        experiencia = 0;
        murosDestruidos = 0;
        enemigosDerrotados = 0;
        nivelesCompletados = 0;
        llaves = 0;
    }

    public PlayerStats(string nombre):this()
    { 
        this.nombre = nombre;
        
    }


    //Mover a GameManager?
    public void AddKey(int amount = 1)
    {
        llaves += amount;
        GameManager.Instance.UpdateKeys(llaves);
    }

    public bool HasKey()
    {
        return llaves > 0;
    }

    public bool TryUseKey()
    {
        if (llaves <= 0)
            return false;

        llaves--;
        GameManager.Instance.UpdateKeys(llaves);
        return true;
    }

}
