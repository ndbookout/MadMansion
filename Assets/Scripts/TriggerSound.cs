using UnityEngine;
using System.Collections;

public class TriggerSound : MonoBehaviour
{
    private AudioSource objAudio;

	// Use this for initialization
	void Start ()
    {
        objAudio = GetComponent<AudioSource>();
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (objAudio.isPlaying != true)
                objAudio.Play();
        }    
    }
}
