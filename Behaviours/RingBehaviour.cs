using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    public float cosMultiplier = 1f;
    public float sinMultiplier = 1f;
    public float period = 20f;

    private void Update()
    {
        //base.transform.root.position += base.transform.right * Mathf.Cos(Time.time * 20f * cosMultiplier * period) * 10f * cosMultiplier * Time.smoothDeltaTime;
        //base.transform.root.position += base.transform.forward * Mathf.Sin((Time.time * 20f * sinMultiplier * period) + Mathf.PI/2) * 10f * sinMultiplier * Time.smoothDeltaTime;
        Vector3 newPos = new Vector3
            (
                Mathf.Cos(Time.time * period) * 10f * cosMultiplier * Time.smoothDeltaTime,
                Mathf.Sin((Time.time * period * 1.5f) + Mathf.PI / 2) * 10f * sinMultiplier * Time.smoothDeltaTime,
                0
            );
        base.transform.root.position += newPos;
    }
}