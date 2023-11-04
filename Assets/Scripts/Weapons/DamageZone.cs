using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [HideInInspector]
    public float myDamageInterval;
    [HideInInspector]
    public float myDamageAmount;
    private List<Collider2D> damagedByThisEffectList = new List<Collider2D>();

    public void OnTriggerStay2D(Collider2D target)
    {
        Health targetHealth = target.GetComponent<Health>();

        if (target.gameObject.tag == "Enemy" && damagedByThisEffectList.Contains(target) == false)
        {
            targetHealth.ReduceCurrentHealth(1, false);
            damagedByThisEffectList.Add(target);
            StartCoroutine(RemoveColliderOnDelay(target));
        }
    }


    public IEnumerator RemoveColliderOnDelay(Collider2D target)
    {
        yield return new WaitForSeconds(myDamageInterval);
        damagedByThisEffectList.Remove(target);

    }
}
