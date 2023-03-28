using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private float duration = 0.1f; // 100 ms delay

    [SerializeField]
    private Vector3 center = Vector3.zero;

    [SerializeField]
    private float spread = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Method instantiates a new arrow to be fired at a specified radius.
    /// </summary>
    /// <param name="to">The distance from the center of the target that the arrow will strike.</param>
    public void Throw(float to)
    {
        float angle = Random.Range(0, Mathf.PI * 2);

        GameObject thisArrow = Instantiate(arrow, center, Quaternion.identity);

        Vector3 target = new(center.x + to * Mathf.Cos(angle), center.y + to * Mathf.Sin(angle));
        thisArrow.GetComponent<ArrowMove>().SetTarget(target);
        thisArrow.GetComponent<ArrowMove>().SetDuration(duration);
    }
}