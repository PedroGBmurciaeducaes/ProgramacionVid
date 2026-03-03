using UnityEngine;

public class FleeEnemy : Enemy
{
    [SerializeField] private Key keyPrefab;

    protected override void DoTurn()
    {
        var playerCell = GameManager.Instance.PlayerController.getCell();

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if (absXDist + absYDist > 6)
            return;

        if (absXDist > absYDist)
        {
            if (!TryMoveAwayInX(xDist))
                TryMoveAwayInY(yDist);
        }
        else
        {
            if (!TryMoveAwayInY(yDist))
                TryMoveAwayInX(xDist);
        }
    }

    bool TryMoveAwayInX(int xDist)
    {
        if (xDist > 0)
            return MoveToOrConsume(m_Cell + Vector2Int.left);

        return MoveToOrConsume(m_Cell + Vector2Int.right);
    }

    bool TryMoveAwayInY(int yDist)
    {
        if (yDist > 0)
            return MoveToOrConsume(m_Cell + Vector2Int.down);

        return MoveToOrConsume(m_Cell + Vector2Int.up);
    }

    bool MoveToOrConsume(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null || !targetCell.Passable)
            return false;

        // Si hay comida o EXP -> destruirla
        if (targetCell.ContainedObject != null)
        {
            if (targetCell.ContainedObject is FoodObject ||
                targetCell.ContainedObject is ExpObject)
            {
                Destroy(targetCell.ContainedObject.gameObject);
                targetCell.ContainedObject = null;
            }
            else
            {
                return false; // no puede atravesar otras cosas
            }
        }

        // Movimiento normal
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    public override void die()
    {
        DropKey();
        base.die();
    }

    void DropKey()
    {
        Key newKey = Instantiate(keyPrefab);
        GameManager.Instance.BoardManager.AddObject(newKey, m_Cell);
    }
}