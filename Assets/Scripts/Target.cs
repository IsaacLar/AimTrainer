using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float respawnDelay = 3.0f;
    public MeshRenderer targetRenderer;
    public SphereCollider targetCollider;
    public TargetMinigame targetMinigame;

    private void Start()
    {
        targetMinigame = GameObject.FindGameObjectWithTag("TargetMinigame").GetComponent<TargetMinigame>();
    }

    public void OnHit()
    {
        //Increment player score
        targetMinigame.score++;
        //Prompt another target to be spawned
        targetMinigame.SpawnTarget();
        //Target Deletes itself
        Destroy(gameObject);
    }
}
