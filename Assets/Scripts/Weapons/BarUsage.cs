using UnityEngine;

[System.Serializable]
public class BarUsage
{
    public Bars bar;
    public float usagePerShot = 1f;
    [Tooltip("if the weapon uses ammo overtime")]
    public bool isOvertime;

    float oldDeltaTime;




    public void Use()
    {
        if (isOvertime)
        {
            bar.actualBar -= usagePerShot*Time.deltaTime;
            oldDeltaTime = Time.deltaTime;
            //Debug.LogError("sasso");
        }
        else
        {
            //Debug.LogError("carta");
            bar.actualBar -= usagePerShot;
        }

    }

    public void NoAmmo()
    {
        if (isOvertime)
        {
            //Debug.LogError("sasso sega");
            bar.actualBar += usagePerShot * oldDeltaTime;
        }
        else
        {
            //Debug.LogError("carta sega");
            bar.actualBar += usagePerShot;
        }
    }
}
