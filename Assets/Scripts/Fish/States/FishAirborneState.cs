using UnityEngine;

public class FishAirborneState : FishBaseState
{
    public FishAirborneState(FishController controller) : base(controller) { }

    public override void Enter()
    {
        controller.rb.useGravity = true;
    }

    public override void Tick(float deltaTime)
    {
        if (controller.IsUnderwater)
            controller.StateMachine.ChangeState(controller.IdleState);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        // limited air steering, mostly ballistic
        Vector2 move = controller.MoveInput;
        Vector3 dir = controller.cameraRig.Forward * move.y + controller.cameraRig.Right * move.x;
        controller.rb.AddForce(dir.normalized * controller.acceleration * 0.5f, ForceMode.Acceleration);
    }

    public override void Exit()
    {
        controller.rb.useGravity = false;
    }
}