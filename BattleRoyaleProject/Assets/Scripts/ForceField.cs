using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float shrinkWaitTime;
    public float shrinkAmount;
    public float shrinkDuration;
    public float minShrinkAmount;

    public int playerDamage;

    private float lastShrinkEndTime;
    private bool shrinking;
    private float targetDiameter;
    private float lastPlayercheckTime;
    void Start()
    {
        lastShrinkEndTime = Time.time;
        targetDiameter = transform.localScale.x;
    }

    // Update will manage the shrinking and damaging
    void Update()
    {
        if(shrinking)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * targetDiameter, (shrinkAmount / shrinkDuration) * Time.deltaTime);
        }

        if (transform.localScale.x == targetDiameter)
        {
            shrinking = false;
        }

        else
        {
            // can we shrink again?
            if(Time.time - lastShrinkEndTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmount)
            {
                Shrink();
            }
        }
    }

    // Shrink will calculate a new diameter and begin to shrink
    void Shrink()
    {
        shrinking = true;

        // make sure we don't shrink below the min amount
        if(transform.localScale.x - shrinkAmount > minShrinkAmount)
        {
            targetDiameter -= minShrinkAmount;
        }

        else
        {
            targetDiameter = minShrinkAmount;
        }

        lastShrinkEndTime = Time.time + shrinkDuration;
    }

    // CheckPlayers loops through the players every second and checks their distance
    // from the center. If they're outside the force field, damage them.
    void CheckPlayers()
    {
        if(Time.time - lastPlayercheckTime > 1.0f)
        {
            //lastPlayerCheckT
        }
    }

}
