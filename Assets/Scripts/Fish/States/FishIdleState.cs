using UnityEngine;

public class FishIdleState : FishBaseState
{
    public FishIdleState(FishController controller) : base(controller) { }

    public override void Enter() { }

    public override void Tick(float deltaTime)
    {
        if (controller.MoveInput.sqrMagnitude > 0.01f)
            controller.StateMachine.ChangeState(controller.SwimState);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        controller.ApplyMovement(0f);
    }

    public override void Exit() { }
}