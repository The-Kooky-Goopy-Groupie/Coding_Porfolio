using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;


[RequireComponent(typeof(Network_Rigid_Body))]
public class Rigid_Body_Controler : NetworkComponent // This is the Assignment 3 version of the script duplicateded to save it as a backup WARNING - Old Objects still have the other controler swap it if needed
{
    public Rigidbody MyRig;
    public float speed = 6;
    public Vector3 LastMove;
    public Vector3 LastAngularVel;
    public GameObject Shootpoint;
    public bool Shootready = true;
    
    public override void HandleMessage(string flag, string value)
    {
        if (flag =="MOVE" && IsServer)
        {
            string[] args = value.Split(',');
            float h = float.Parse(args[0]);
            float v = float.Parse(args[1]);
            //    LastMove = new Vector3(h, 0, v) * speed;
            LastMove = transform.forward * v * speed;
            LastAngularVel = new Vector3(0,h,0) * speed;
        }
        if (flag == "SHOT")
        {
            if (Shootready == true && IsServer)
            {  // Spawn bullet somehow, make move forward // 3 is for the bullet owner to make the bullet belong to player
                GameObject Bullet = MyCore.NetCreateObject(3, Owner, Shootpoint.transform.position, Shootpoint.transform.rotation);
                Bullet.GetComponent<Rigidbody>().velocity = transform.forward * speed * 2;
                Shootready = false;
                StartCoroutine(CorutineForBullet());
            }
        }
    }

    public override void NetworkedStart()
    {
        //throw new System.NotImplementedException();
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
            MyRig.useGravity = false;
        }
        while (IsConnected)
        {
            if (IsLocalPlayer)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if ((LastMove - new Vector3(h, 0, v)).magnitude > .1f)
                {
                    LastMove = new Vector3(h, 0, v);
                    SendCommand("MOVE", h + "," + v);
                }
                if (Input.GetAxisRaw("Jump")> 0 && Shootready == true)
                {
                    Shootready = false;
                    StartCoroutine(CorutineForBullet());
                    SendCommand("SHOT","1");
                }
                // do not adjust the speed here this is this client
            }

           if (IsServer)
            {

                //  MyRig.velocity = this.transform.forward * 15;
                //  MyRig.angularVelocity = new Vector3(0, Mathf.PI / 2, 0);

            }
            yield return new WaitForSeconds(.1f);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        MyRig = GetComponent<Rigidbody>();
        if (MyRig == null)
        {
            throw new System.Exception("ERROR RIGID BODY MISSING, JUST LIKE YOUR MUSSLES.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            MyRig.velocity = LastMove;
            MyRig.angularVelocity = LastAngularVel;// set last of each
            // things like acceleration, lerp, smoothing go here

        }
        if (IsClient)
        {
            if (MyRig.velocity.magnitude > .1f)
            {
                 //this.transform.forward = new Vector3(MyRig.velocity.x, 0, MyRig.velocity.z).normalized; // Cant use this for the tank version we'll have to move this since no longer cosmetic
                // normalized is at the end because, don't care mag just use direction
            }
        }
    }
    public IEnumerator CorutineForBullet()
    {
        yield return new WaitForSeconds(0.5f);
        Shootready = true;// wait half a frame
    }
}
/*
 Shooter then i thought
But tougher then i thought
multiple choice, fill in the blank, multiple answers

Will primarily cover the powerpoints, coding is mainly only the slides
Implicit game design 1
Stuff you should have learned from the powerpoint too 
the 4 layers - the main functonality, how they relate, NO VIRTUALIZATION or THREADING
variables know
no debuging questions
Conceptional synchronizing game
basic questions on syncronizing player movement and variables but from the concpet 
Powerpoint > over the video
During classtime it will be done

 
 */
