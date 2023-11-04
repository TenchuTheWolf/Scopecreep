using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Micromanager : NPC
{
    public Transform mostRecentPathNode;
    public Transform targetPathNode;
    public float bumbleSpeed;

    protected override void Awake()
    {
        base.Awake();
        SetNextPathNode();

        Player.playerInstance.MoreMicromanaged();

    }

    private void Update()
    {
        CheckDistanceToPathNode();
    }

    private void FixedUpdate()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        Vector2 direction = targetPathNode.position - transform.position;
        Vector2 moveForce = direction.normalized * bumbleSpeed * Time.fixedDeltaTime;

        rigidBody.AddForce(moveForce, ForceMode2D.Force);

    }

    private void CheckDistanceToPathNode()
    {
        if (Vector2.Distance(transform.position, targetPathNode.position) < .1f)
        {
            SetNextPathNode();
        }
    }

    private void SetNextPathNode()
    {
        if (mostRecentPathNode == null)
        {
            Transform spawnPosition = TargetUtilities.FindClosestTransform(GameManager.gameManagerInstance.enemySpawnLocations, transform.position);
            mostRecentPathNode = spawnPosition;

        }
        List<Transform> potentialPathNodes = new List<Transform>();

        for (int i = 0; i < GameManager.gameManagerInstance.enemySpawnLocations.Count; i++)
        {
            if (GameManager.gameManagerInstance.enemySpawnLocations[i] != mostRecentPathNode)
            {
                potentialPathNodes.Add(GameManager.gameManagerInstance.enemySpawnLocations[i]);
            }
        }

        int randomIndex = Random.Range(0, potentialPathNodes.Count);

        Transform newPathNode = potentialPathNodes[randomIndex];
        targetPathNode = newPathNode;
    }

    protected override void Die()
    {
        Player.playerInstance.LessMicromanaged();

        if(untrackedNPC == true && GameTaskManager.instance.currentGameEvent != null)
        {
            //Debug.Log("The current game event is: " + GameTaskManager.instance.currentGameEvent + " starting micromanager recursion.");
            GameTaskManager.instance.StartMicroManagerRecursion(); 
        }

        base.Die();
    }




}
