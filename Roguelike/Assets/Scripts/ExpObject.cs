using UnityEngine;

public class ExpObject : CellObject
{
    public int AmountGranted = 1;
    public int peso = 1;

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        //increase Exp
        GameManager.Instance.ChangeExp(AmountGranted);
    }
}