using System.Collections;
using UnityEngine;

public class TargetMinigame : MonoBehaviour
{
    public GameObject target;

    public int score;
    private float startTime = 60.0f;
    //Min and max positions for spawning targets
    private Vector2 minCoords = new Vector2(-20, 2);
    private Vector2 maxCoords = new Vector2(20, 12);


    // Update is called once per frame
    void Update()
    {
    }

    public void StartMinigame()
    {
        score = 0;
        for (int i = 0; i < 5; i++)
        {
            SpawnTarget();
        }
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(startTime);
        FinishGame();
    }

    public void FinishGame()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < targets.Length; i++)
        {
            Destroy(targets[i]);
        }
        Debug.Log("Score: "+score.ToString());
    }

    public void SpawnTarget()
    {
        Vector3 spawnPos = new Vector3(Random.Range(minCoords.x, maxCoords.x), Random.Range(minCoords.y, maxCoords.y), 36.5f);
        GameObject newTarget = Instantiate(target, spawnPos, transform.rotation);
    }

    public void RespawnTarget(GameObject target)
    {
        target.transform.position = new Vector3(Random.Range(minCoords.x, maxCoords.x), Random.Range(minCoords.y, maxCoords.y), 36.5f);
    }
}
