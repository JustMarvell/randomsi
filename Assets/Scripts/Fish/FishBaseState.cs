using UnityEngine;

public abstract class FishBaseState
{
    protected FishController controller;

    public FishBaseState(FishController controller)
    {
        this.controller = controller;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void FixedTick(float fixedDeltaTime);
    public abstract void Exit();
}