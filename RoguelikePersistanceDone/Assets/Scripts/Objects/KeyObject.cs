using UnityEngine;

public class Key : CellObject
{
    public override void PlayerEntered()
    {
        // Añadir llave al jugador
        GameSesion.instance.fichaDePersonaje.AddKey();

        // Mostrar mensaje
        GameManager.Instance.ShowCombatText(
            MessageType.type.llaveObtenida,   // o crea uno nuevo tipo "ItemPickup"
            1,
            GameManager.Instance.player.transform
        );

        // Eliminar del tablero
        var board = GameManager.Instance.BoardManager;
        var cellData = board.GetCellData(m_Cell);
        cellData.ContainedObject = null;

        Destroy(gameObject);
    }

    public override bool PlayerWantsToEnter(int damage)
    {
        return true; // No bloquea movimiento
    }
}