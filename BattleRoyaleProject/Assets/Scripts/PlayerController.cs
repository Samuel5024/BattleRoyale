using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Components")]
    public Rigidbody rig;

    public int id;
    public Player photonPlayer;

    private int curAttackerId;

    public int curHp;
    public int maxHp;
    public bool dead;
    private bool flashingDamage;
    public MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // make sure that only the player who controls this player, will run the update function
        if(!photonView.IsMine || dead)
        {
            return;
        }

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();
    }

    // TakeDamage called when player gets hit
    [PunRPC]
    public void TakeDamage(int attackerId, int damage)
    {
        if(dead)
        {
            return;
        }

        curHp -= damage;
        curAttackerId = attackerId;

        // flash player red
        photonView.RPC("DamageFlash", RpcTarget.Others);

        // update the health bar UI

        // die if no health left
        if(curHp <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }

    // DamageFlash visually flashes the player red when they get hit
    [PunRPC]
    void DamageFlash()
    {
        if(flashingDamage)
        {
            StartCoroutine(DamageFlashCoRoutine());

            IEnumerator DamageFlashCoRoutine()
            {
                flashingDamage = true;
                Color defaultColor = mr.material.color;
                mr.material.color = Color.red;

                yield return new WaitForSeconds(0.05f);

                mr.material.color = defaultColor;
                flashingDamage = false;
            }
        }
    }

    // Die gets called when our health goes below 0
    [PunRPC]
    void Die()
    {
        curHp = 0;
        dead = true;

        GameManager.instance.alivePlayers--;

        // host will check win condition
        if(PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.CheckWinCondition();
        }

        // is this our local player
        if(photonView.IsMine)
        {
            if(curAttackerId != 0)
            {
                GameManager.instance.GetPlayer(curAttackerId).photonView.RPC("AddKill", RpcTarget.All);
            }

            // set the cam to spectator
            GetComponentInChildren<CameraController>().SetAsSpectator();

            //disable the hysics and hide the player
            rig.isKinematic = true;
            transform.position = new Vector3(0, -50, 0);
        }
    }

    [PunRPC]
    public void AddKill()
    {
        kills++;
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

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;

        GameManager.instance.players[id - 1] = this;

        // is this not our local player?
        if(!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
        }


    }

}
