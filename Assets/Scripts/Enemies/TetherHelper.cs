using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TetherHelper : MonoBehaviour
{
    public VisualEffect tetherEffect;
    public Vector3 Pos1Micromanager { get; private set; }
    public Vector3 Pos2SuspendPoint1 { get; private set; }
    public Vector3 Pos3SuspendPoint2 { get; private set; }
    public Vector3 Pos4Player { get; private set; }

    private void Update()
    {
        CalculateTransforms();
    }

    /// <summary>
    /// Gets the positions of this Micromanager and the Player and calculates two anchor point positions between them.
    /// </summary>
    public void CalculateTransforms()
    {
        if(Player.playerInstance == null)
        {
            return;
        }

        Pos1Micromanager = transform.position;
        Pos4Player = Player.playerInstance.transform.position;
        Vector2 directionToPlayer = Player.playerInstance.transform.position - transform.position;

        Vector2 midPoint1 = TargetUtilities.LerpByDistance(Pos1Micromanager, Pos4Player, directionToPlayer.magnitude * .33f);
        Pos2SuspendPoint1 = midPoint1;

        Vector2 midPoint2 = TargetUtilities.LerpByDistance(Pos1Micromanager, Pos4Player, directionToPlayer.magnitude * .66f);
        Pos3SuspendPoint2 = midPoint2;

        SetTetherTransforms();
    }

   
    public void SetTetherTransforms()
    {
        tetherEffect.SetVector3("Position 1", Pos1Micromanager);
        tetherEffect.SetVector3("Position 2", Pos2SuspendPoint1);
        tetherEffect.SetVector3("Position 3", Pos3SuspendPoint2);
        tetherEffect.SetVector3("Position 4", Pos4Player);
    }


}
