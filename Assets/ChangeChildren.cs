using UnityEngine;

public class ChangeChildren : MonoBehaviour
{
    GameObjectHolder[] holders;
    void Start()
    {
        holders = GetComponentsInChildren<GameObjectHolder>();
        PlayerHp.Nightmare += Nightmare;
    }

    void Nightmare()
    {
        foreach (GameObjectHolder holder in holders)
        {
            holder.NightmareChange();
        }
    }
    
}
