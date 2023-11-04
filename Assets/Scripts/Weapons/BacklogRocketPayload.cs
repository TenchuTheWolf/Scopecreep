using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacklogRocketPayload : MonoBehaviour
{

    private float myDamageRadius;
    private float myPullRadius;
    public float backlogRocketDamage = 1f;
    public float damageInterval;
    public float backlogPayloadDuration = 3f;
    public PointEffector2D pullRadiusPointEffector;
    public CircleCollider2D damageRadiusCollider;
    public DamageZone damageZone;

    /// <summary>
    /// Sets up the base stats and references of the Backlog Rocket.
    /// </summary>
    private void Awake()
    {
        myDamageRadius = Player.playerInstance.specWepBacklogMissDamageRange;
        myPullRadius = Player.playerInstance.specWepBacklogMissPullRange;
        damageRadiusCollider.radius = myDamageRadius;
        damageZone.myDamageAmount = backlogRocketDamage;
        damageZone.myDamageInterval = damageInterval;
        damageZone.myDamageInterval = damageInterval;
        Destroy(gameObject, backlogPayloadDuration);
    }



}
