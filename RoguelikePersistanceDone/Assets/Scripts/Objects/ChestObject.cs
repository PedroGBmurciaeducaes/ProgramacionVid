using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestObject : CellObject
{
    public Tile[] ObstacleTile;
    public int MaxHealth = 1;
    public int peso = 100;
    protected int m_HealthPoint;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoint = MaxHealth;

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile[0]);
    }

    public override bool PlayerWantsToEnter(int damage)
    {

        if (m_HealthPoint > 1)
        {
            m_HealthPoint -= 1;
            return false;
        }

        if (m_HealthPoint == 1)
        {
            m_HealthPoint -= 1;                                                      // Cofre abierto
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, ObstacleTile[1]);
            GameEvents.TriggerChestOpened();                                            // Emitir evento de cofre abierto
            GameManager.Instance.ChangeExp((int)Random.Range(5, 10)); //Hacemos que el cofre nos de una cantidad aleatoria de EXP entre 3 y 12
            GameManager.Instance.ChangeFood((int)Random.Range(3, 12)); //Hacemos que el cofre nos de una cantidad aleatoria de comida entre 3 y 15

            return false;
        }
       
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
            Destroy(gameObject);
            return true;

    }
}