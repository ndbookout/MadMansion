﻿using UnityEngine;
using System.Collections;

public class KillGhoul : MonoBehaviour
{
    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.layer == 16)
            Destroy(collide.gameObject);
    }
}
