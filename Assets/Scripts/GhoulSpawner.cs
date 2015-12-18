using UnityEngine;
using System.Collections;

public class GhoulSpawner : MonoBehaviour {

    public GameObject ghoulPrefab;
    public float ghoulSpawnTimer = 45f;


	void Start ()
    {
        StartCoroutine(SpawnGhoul());
    }
	
	
	void Update ()
    {
	
	}
    IEnumerator SpawnGhoulsMoreFrequentlyOverTime()
    {
        yield return new WaitForSeconds(20f);
        if (ghoulSpawnTimer > 8)
        {
            ghoulSpawnTimer--;
            StartCoroutine(SpawnGhoulsMoreFrequentlyOverTime());
        }
    }
    IEnumerator SpawnGhoul()
    {
        Instantiate(ghoulPrefab, this.transform.position, this.transform.rotation);
        yield return new WaitForSeconds(ghoulSpawnTimer);
        StartCoroutine(SpawnGhoul());
    }
}
