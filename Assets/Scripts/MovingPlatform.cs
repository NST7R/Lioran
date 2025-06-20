using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public float waitTime = 2f;
    public int startingPoint;
    public Transform[] points;

    private int i;
    private Rigidbody2D rb;
    private Vector2 targetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        transform.position = points[startingPoint].position;
        i = startingPoint;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            targetPos = points[i].position;
            while (Vector2.Distance(rb.position, targetPos) > 0.01f)
            {
                Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(waitTime);
            i = (i + 1) % points.Length;
        }
    }
}
