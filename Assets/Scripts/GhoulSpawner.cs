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
        yield return new WaitForSeconds(30f);
        if (ghoulSpawnTimer > 15)
        {
            ghoulSpawnTimer--;
            StartCoroutine(SpawnGhoulsMoreFrequentlyOverTime());
        }
    }
    IEnumerator SpawnGhoul()
    {
        
        yield return new WaitForSeconds(ghoulSpawnTimer);
        Instantiate(ghoulPrefab, this.transform.position, this.transform.rotation);
        StartCoroutine(SpawnGhoul());
    }
}
