using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 0;

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        GameEvents.TriggerFoodConsumed(); // Emitir evento de comida recolectada
        GameEvents.TriggerHealthRestored(AmountGranted); // Emitir evento de comida recolectada


        //increase food
        GameManager.Instance.ChangeFood(AmountGranted);
    }
}