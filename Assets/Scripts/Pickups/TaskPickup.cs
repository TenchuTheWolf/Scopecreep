using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskPickup : Pickup
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    
    protected override void Awake()
    {
        base.Awake();
        GameTaskManager.instance.EnlistTask(this);
        GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(-90f, 90f);
    }

    public TaskPriority myTaskPriority;

    protected override void Fizzle()
    {
        GameTaskManager.instance.TaskFizzled(this);
        base.Fizzle();
    }

    protected override void Collect()
    {
        GameTaskManager.instance.TaskCollected(this);
        base.Collect();
    }

}
