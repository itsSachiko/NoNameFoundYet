using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Pillow", menuName = "Weapons/Melee")]
public class Melee : Weapons
{
    [Header("for All:")]
    public float range = 10f;

    [Header("Melee Line settings: ")]
    [Tooltip("if the attack is only horizontal twoards the pointer")]
    public bool IsLine;
    [Tooltip("how long it takes to swing your sword")]
    public float durationOfAttack = 1f;
    public float rangeOfLine = 10f;

    [Header("Melee Cone: ")]
    [Tooltip("if the is the one with an angle")]
    public bool isCone;
    public float angleOfAttack = 45;

    [Header("Melee 360: ")]
    [Tooltip("if the is the one that rotates")]
    public bool is360;
    public int numberOfSpiins = 1;
        
    public void Swing(Transform sword)
    {
        sword.gameObject.SetActive(true);
        if (isCone)
        {
            sword.rotation = Quaternion.AngleAxis(angleOfAttack, Vector3.forward);
            sword.gameObject.SetActive(false);
        }

        if (IsLine)
        {
            Vector3 swordScale = sword.transform.localScale;
            swordScale.x *= rangeOfLine;
            sword.transform.localScale = swordScale;
            //sword.GetComponent<MonoBehaviour>().StartCoroutine(LineAttack(sword));
        }

        if (is360)
        {
            //codice gay :3 come me

            foreach( Collider x in Physics.OverlapSphere(sword.root.position, range))
            {
                if (x.TryGetComponent(out IHp hp))
                {
                    hp.TakeDmg(damage);
                }
            }
        }
    }

    IEnumerator LineAttack(Transform sword)
    {
        Vector3 pos = sword.position;
        Vector3 endPos = sword.position + sword.parent.right * rangeOfLine;
        Vector3 lerpPos;
        while (Vector3.Distance(pos, endPos) < 0.1f)
        {
            lerpPos = Vector3.Lerp(pos, endPos, Time.deltaTime);
            sword.position = lerpPos;
            yield return null;
        }
        sword.gameObject.SetActive(false);

    }
}
