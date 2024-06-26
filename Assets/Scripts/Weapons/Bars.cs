using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bar", menuName = "Weapons/Bars")]
public class Bars : ScriptableObject
{
    public float fullBar = 100;
    public float rateRechargePerSeconds = 1;
    [Tooltip("the secods to wait after you use the myBar")]
    public float waitAfterUse = 1;
    [HideInInspector] public float actualBar;
    [HideInInspector] public bool canRecharge = true;
    [HideInInspector] public IEnumerator recharge;

    private void OnEnable()
    {
        actualBar = fullBar;
        canRecharge = true;
        recharge = null;
    }

    //public bool UsageCheck(float amount)
    //{
    //    if (actualBar-amount*Time.deltaTime <= 0)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    //public void Usage(float amount)
    //{
    //    actualBar -= amount * Time.deltaTime;
    //}

    //public IEnumerator Used()
    //{
    //    while(actualBar < fullBar)
    //    {
    //        actualBar += recarchePerSecond * Time.deltaTime;
    //        yield return null;
    //    }
    //    actualBar = fullBar;
    //}
}
