using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float shrinkWaitTime; // how long the field waits (in seconds) after finishing a shrink before we can shrink again
    public float shrinkAmount; // how much the field should shrink everytime it shrinks
    public float shrinkDuration; // how long the shrinking process takes (eases from current -> target size overtime)
    public float minShrinkAmount; // minimum diamater field is allowed to shrink so the field doesn't completely disappear

    public int playerDamage; // how much damage players take when outside the field

    private float lastShrinkEndTime;
    private bool shrinking;
    private float targetDiameter;
    private float lastPlayerCheckTime;
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
            if (transform.localScale.x == targetDiameter)
            {
                shrinking = false;
            }
        }        
        else
        {
            float shrinkTime = Time.time - lastShrinkEndTime;
            Debug.Log(shrinkTime);
            Debug.Log(shrinkWaitTime);
            float shrinkAmount = transform.localScale.x;
            Debug.Log(shrinkAmount);
            Debug.Log(minShrinkAmount);
            // can we shrink again?
            if(Time.time - lastShrinkEndTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmount)
            {
                Debug.Log("Shrink is being called");
                Shrink();
            }
        }

        CheckPlayers();
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

        Debug.Log(targetDiameter);
    }

    // CheckPlayers loops through the players every second and checks their distance
    // from the center. If they're outside the force field, damage them.
    void CheckPlayers()
    {
        if(Time.time - lastPlayerCheckTime > 1.0f)
        {
            lastPlayerCheckTime = Time.time;

            // loop through all players
            foreach(PlayerController player in GameManager.instance.players)
            {
                if(player.dead || !player)
                {
                    continue;
                }

                if(Vector3.Distance(Vector3.zero, player.transform.position) >= transform.localScale.x)
                {
                    player.photonView.RPC("TakeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }

}
