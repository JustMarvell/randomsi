using UnityEngine;

public class FishSprintState : FishBaseState
{
    public FishSprintState(FishController controller) : base(controller) { }

    public override void Enter() { }

    public override void Tick(float deltaTime)
    {
        controller.ApplyRotation();

        if (controller.MoveInput.sqrMagnitude < 0.01f)
            controller.StateMachine.ChangeState(controller.IdleState);
        else if (!controller.SprintHeld)
            controller.StateMachine.ChangeState(controller.SwimState);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        controller.ApplyMovement(controller.sprintSpeed);
    }

    public override void Exit() { }
}