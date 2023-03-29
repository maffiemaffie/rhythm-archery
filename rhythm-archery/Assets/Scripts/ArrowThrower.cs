using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private GameObject target;
    private float radius;

    [SerializeField]
    private float duration = 0.1f; // 100 ms delay

    // Start is called before the first frame update
    void Start()
    {
        radius = target.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Method instantiates a new arrow to be fired at a specified radius.
    /// </summary>
    /// <param name="to">The 0-1 distance from the center of the target that the arrow will strike.</param>
    public void Throw(float to)
    {
        float angle = Random.Range(0, Mathf.PI * 2);

        GameObject thisArrow = Instantiate(arrow, Vector3.zero, Quaternion.identity);

        Vector3 arrowTarget = new(
            target.transform.position.x + radius * to * Mathf.Cos(angle),
            target.transform.position.y + radius * to * Mathf.Sin(angle),
            target.transform.position.z);
        thisArrow.GetComponent<ArrowMove>().SetTarget(arrowTarget);
        thisArrow.GetComponent<ArrowMove>().SetDuration(duration);
    }
}