using UnityEngine;

public class FishStateMachine
{
    public FishBaseState CurrentState { get; private set; }

    public void Initialize(FishBaseState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(FishBaseState newState)
    {
        if (CurrentState == newState) return;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Tick(float deltaTime) => CurrentState?.Tick(deltaTime);
    public void FixedTick(float fixedDeltaTime) => CurrentState?.FixedTick(fixedDeltaTime);
}