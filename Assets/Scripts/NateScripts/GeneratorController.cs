using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorController : MonoBehaviour
{
    public GameObject[] generators;
    public GameObject[] bones;

	// Use this for initialization
	void Start ()
    {
        new Task(ActivateGenerators());
	}

    IEnumerator ActivateGenerators()
    {
        for (int i = 0; i < 3; i++)
        {
            if (generators[i].activeSelf == false)
            {
                generators[i].SetActive(true);
                yield return new WaitForSeconds(2);
            }
        }

        InstantiateBones();
    }

    void InstantiateBones()
    {
        List<GameObject> keyItems = new List<GameObject>();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("KeyItemSlot"))
            keyItems.Add(item);

        for (int i = 0; i < bones.Length; i++)
        {
            int randomNum = Random.Range(0, keyItems.Count);
            GameObject item = Instantiate(bones[i]) as GameObject;
            item.transform.position = keyItems[randomNum].transform.position;
            keyItems.RemoveAt(randomNum);
        }
    }
}
