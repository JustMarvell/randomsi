using UnityEngine;

public class FishSwimState : FishBaseState
{
    public FishSwimState(FishController controller) : base(controller) { }

    public override void Enter() { }

    public override void Tick(float deltaTime)
    {
        if (controller.MoveInput.sqrMagnitude < 0.01f)
            controller.StateMachine.ChangeState(controller.IdleState);
        else if (controller.SprintHeld)
            controller.StateMachine.ChangeState(controller.SprintState);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        if (!controller.IsUnderwater)
        {
            controller.StateMachine.ChangeState(controller.AirborneState);
            return;
        }

        controller.ApplyMovement(controller.swimSpeed);
    }

    public override void Exit() { }
}