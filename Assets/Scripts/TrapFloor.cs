using UnityEngine;
using System.Collections;

public class TrapFloor : MonoBehaviour
{
    float time;
    bool breakFloor;
     
    void OnCollisionEnter(Collision collide)
    {
        time = 0;
        breakFloor = true;
    }

    void Update()
    {
        if (breakFloor)
        {
            time += Time.deltaTime;

            if (time > 0.5f)
            {
                Destroy(this.gameObject);
            }

            Debug.Log(time);
        }
    }
}
