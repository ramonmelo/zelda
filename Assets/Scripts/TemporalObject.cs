using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalObject : MonoBehaviour
{
	[Min(0.1f)]
	public float TTL;

    void Start()
    {
        Destroy(gameObject, TTL);
    }
}
