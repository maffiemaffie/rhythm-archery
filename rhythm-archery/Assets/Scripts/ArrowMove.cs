using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Vector3 destination;
    private float duration;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 toDestination = destination - transform.position;
        speed = toDestination.magnitude / duration;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toDestination = destination - transform.position;
        if (toDestination.sqrMagnitude < speed * Time.deltaTime)
        {
            transform.position = destination;
            Destroy(this);
        }
        else
        {
            Vector3 direction = toDestination.normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
