using UnityEngine;
using System.Collections;

public class GeneratorController : MonoBehaviour
{
    public GameObject[] generators;

	// Use this for initialization
	void Start ()
    {
        new Task(TriggerGenerators());
	}

    IEnumerator TriggerGenerators()
    {
        for (int i = 0; i < 3; i++)
        {
            if (generators[i].activeSelf == false)
            {
                generators[i].SetActive(true);
                yield return new WaitForSeconds(3);
            }
        }
    }
	
}
