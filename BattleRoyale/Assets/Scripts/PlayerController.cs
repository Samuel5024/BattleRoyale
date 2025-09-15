using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Components")]
    public Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();
    }

    // Move checks for keyboard input and then sets our velocity
    void Move()
    {
        // get the input axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // calculate a direction relative to where we're facing
        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;

        //preserve vertical movement
        dir.y = rig.velocity.y;

        // set that as our velocity
        rig.velocity = dir;
    }

    // TryJump checks to see if player is standing on the ground and if so, add upward force
    void TryJump()
    {
        // start slightly above the bottom of the player
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        // distance to check = half the capsule height + a little buffer
        float checkDistance = 0.6f;

        // perform the raycast
        if(Physics.Raycast(origin, Vector3.down))
        {

        }
    }
}
