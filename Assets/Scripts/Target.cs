using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float respawnDelay = 3.0f;
    public MeshRenderer targetRenderer;
    public SphereCollider targetCollider;

    public void OnHit()
    {
        StartCoroutine(HitRoutine());
    }

    public IEnumerator HitRoutine()
    {
        //Hide target and disable collisions
        targetRenderer.enabled = false;
        targetCollider.enabled = false;
        //Wait 3 seconds
        yield return new WaitForSeconds(respawnDelay);
        //Re-enable everything
        targetRenderer.enabled = true;
        targetCollider.enabled = true;
    }
}
