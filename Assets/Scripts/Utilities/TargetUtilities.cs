using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetUtilities
{
    public static Quaternion GetRotationTowardTarget(Vector2 targetPosition, Vector2 myPosition)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return targetRotation;
    }

    public static void RotateSmoothlyTowardTarget(Vector3 targetPos, Transform myTransform, float speed)
    {
        Quaternion targetRotation = GetRotationTowardTarget(targetPos, myTransform.position);
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, speed * Time.fixedDeltaTime);
    }

    public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float distance)
    {
        Vector3 P = distance * Vector3.Normalize(B - A) + A;
        return P;
    }

    public static Transform FindClosestTransform(List<Transform> transformList, Vector2 compareLocation)
    {
        Transform closestTransform = null;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < transformList.Count; i++)
        {
            float distance = Vector2.Distance(compareLocation, transformList[i].position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                closestTransform = transformList[i];
            }
        }

        return closestTransform;
    }

}
