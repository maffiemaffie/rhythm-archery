using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Vector3 target;
    private float duration;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 toTarget = target - transform.position;
        speed = toTarget.magnitude / duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || duration == null) return;

        Vector3 toTarget = target - transform.position;
        if (toTarget.sqrMagnitude < speed * Time.deltaTime)
        {
            transform.position = target;
            Destroy(this);
        }
        else
        {
            Vector3 direction = toTarget.normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}
