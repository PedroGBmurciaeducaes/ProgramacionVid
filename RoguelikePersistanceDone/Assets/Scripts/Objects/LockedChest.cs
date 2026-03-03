using UnityEngine;

public class LockedChest : ChestObject
{
    private bool m_Unlocked = false;

    public override bool PlayerWantsToEnter(int damage)
    {
        // Si todavía está cerrado con llave
        if (!m_Unlocked)
        {
            if (GameSesion.instance.fichaDePersonaje.TryUseKey())
            {
                m_Unlocked = true;
                m_HealthPoint = 0; // lo ponemos en 0 para que al ejecutarse el comportamiento del cofre normal, no le de vida al tratar de destruirlo
                GameManager.Instance.BoardManager.SetCellTile(m_Cell, ObstacleTile[1]);
                GameEvents.TriggerChestOpened();
                GameManager.Instance.ChangeExp((int)Random.Range(7, 18)); //Hacemos que el cofre nos de una cantidad aleatoria de EXP entre 3 y 12
                GameManager.Instance.ChangeFood((int)Random.Range(10, 25)); //Hacemos que el cofre nos de una cantidad aleatoria de comida entre 3 y 15

                GameManager.Instance.ShowCombatText(
                    MessageType.type.ChestOpened, // mejor crear NeedKey / Unlocked
                    0,
                    GameManager.Instance.player.transform
                );
                return false;
            }
            else
            {
                GameManager.Instance.ShowCombatText(
                    MessageType.type.NeedKey, // mejor crear NeedKey
                    0,
                    GameManager.Instance.player.transform
                );

                return false;
            }
        }

        // Si ya está desbloqueado, usar comportamiento normal
        return base.PlayerWantsToEnter(damage);
    }
}