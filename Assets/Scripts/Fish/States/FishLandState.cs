using UnityEngine;

// Scaffold for future on-land movement (e.g. shark beaching itself near islands)
public class FishLandState : FishBaseState
{
    public FishLandState(FishController controller) : base(controller) { }

    public override void Enter() { }
    public override void Tick(float deltaTime) { }
    public override void FixedTick(float fixedDeltaTime) { }
    public override void Exit() { }
}