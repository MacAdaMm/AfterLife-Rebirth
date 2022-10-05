using System.Collections;
using UnityEngine;
using ShadyPixel.Events;

public class MovementStateChangeEvent : SPEventBase
{
    public MovementController.MovementState LastState { get; }
    public MovementController.MovementState NewState { get; }
    public MovementStateChangeEvent(MovementController.MovementState lastState, MovementController.MovementState newState)
    {
        LastState = lastState;
        NewState = newState;
    }
}
public class HealthChangeEvent : SPEventBase
{
    public int Current { get; }
    public int Max { get; }
    public HealthChangeEvent(int current, int max)
    {
        Max = max;
        Current = current;
    }
}
public class DeathEvent : SPEventBase
{
    public Actor Actor { get; }
    public DeathEvent(Actor actor)
    {
        Actor = actor;
    }
}

public class Actor : MonoBehaviour
{
    public SPEventBus EventBus { get; } = new SPEventBus();

}