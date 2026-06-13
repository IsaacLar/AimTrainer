using UnityEngine;

public class TargetMGameButton : Button
{
    public TargetMinigame targetMinigame;
    public override void OnHit()
    {
        targetMinigame.StartMinigame();
    }
}
