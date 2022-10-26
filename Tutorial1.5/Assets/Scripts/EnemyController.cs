using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 position1 = new Vector2(0.0f, 0.0f);
    public Vector2 position2 = new Vector2(2.0f, 0.0f);

    Vector2 currentTargetDestination;



    void Start()
    {
        transform.position = position1;
        currentTargetDestination = position2;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTargetDestination, 5 * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentTargetDestination) <= 0.5)
        {

            if (currentTargetDestination == position1)
            {
                currentTargetDestination = position2;
            }
            else
            {
                currentTargetDestination = position1;
            }
        }
    }
}
