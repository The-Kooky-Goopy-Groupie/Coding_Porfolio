using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

// Note this is the updated contorler for animations
/*
 Shooting - Works perffectly 
 Movement -  HAHAHAHAHAHAHAH I DONE IT, IT ACTIVATED YESSSSSSSSSSSSSSS
 Jump - I have no idea, it just does what it wants when it wants to wait why
 
I set up the raycast thing as shown in the book but it for some reason is still fully doing this glitch most of the time. 
However another weird thing occurs if i disable the colsions check as show in the book it does not run 



Takeaway sync the variables for animations that need to be synchronized.
 */
[RequireComponent(typeof(Network_Rigid_Body))]
public class Rigid_Body_Control : NetworkComponent
{
    public Rigidbody MyRig;
    public float speed = 6;

    public Vector3 LastMove;
    public Vector3 LastAngularVel;

    public GameObject Shootpoint;
    public bool Shootready = true;

    public Animator MyAnimator;
    public SpriteRenderer MySpriteRender;

    public bool CanJump = true;
    
    public override void HandleMessage(string flag, string value)
    {
        if (flag =="MOVE" && IsServer)
        {
            string[] args = value.Split(',');
            float h = float.Parse(args[0]);
            float v = float.Parse(args[1]);
            //    LastMove = new Vector3(h, 0, v) * speed;
            LastMove = transform.right * h * speed;
            if (h > 0)
            {
                MySpriteRender.flipX = false; // right
            }
            if(h < 0)
            {
                MySpriteRender.flipX = true; // left
            }
            if(v > 0 && CanJump == true) // check that are on the ground and can do a jump input
            {
                MyRig.velocity += new Vector3(0,v,0)* speed *2; // v gives jump senstivity while 1 makes constant
                CanJump = false; // this can jump get set to false automatically
                SendUpdate("JUMP_ANI", "1");
            }
            //LastAngularVel = new Vector3(0, MyRig.velocity.y, 0) * speed;
        }
        
        // both
        if (flag == "SHOOT_ANI")
        {
            if (value == "1")
            {
                Shootready = false;
                MyAnimator.SetInteger("Animate", 2); // CHECK HERE
            }
            else
            {
                Shootready = true;
                MyAnimator.SetInteger("Animate", 0);
            }

        }

       if (flag == "JUMP_ANI")
            {
                if (value == "1")
                {
                    CanJump = false;
                    MyAnimator.SetInteger("Animate", 3); // CHECK HERE
                }
                else
                {
                    CanJump = true;
                    MyAnimator.SetInteger("Animate", 0);
                }

            }
        
        if (flag == "SHOT")
        {
            if (Shootready == true && IsServer)
            {  // Spawn bullet somehow, make move forward // 3 is for the bullet owner to make the bullet belong to player
                //GameObject Bullet = MyCore.NetCreateObject(1, Owner, Shootpoint.transform.position, Shootpoint.transform.rotation); // make sure to the correct array point
                if (MySpriteRender.flipX == false)
                {
                    GameObject Bullet = MyCore.NetCreateObject(1, Owner, Shootpoint.transform.position, Shootpoint.transform.rotation);
                    Bullet.GetComponent<Rigidbody>().velocity = transform.right * speed * 2; //        transform.right THIS WORK besides well the whole not fireing to the correct way
                }
                else
                {
                    GameObject Bullet = MyCore.NetCreateObject(1, Owner, transform.position - transform.right * .4f, Shootpoint.transform.rotation);// should be point 4 to left of char
                    Bullet.GetComponent<Rigidbody>().velocity = transform.right * speed * -2;
                }
                SendUpdate("SHOOT_ANI", "1");
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
                if ((LastMove - new Vector3(h, v, 0)).magnitude > .1f)
                {
                    LastMove = new Vector3(h, v, 0); // this sends the movement
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

            //CHEKING SPOT IF NOT RETURN HERE
            if (IsServer) {
                if (!CanJump)
                {
                    RaycastHit floorcheck;
                    if (Physics.Raycast(this.transform.position, this.transform.up * -1, out floorcheck))
                    {
                        if (Vector3.Distance(this.transform.position, floorcheck.point) < 1.65f && MyRig.velocity.y <= 0)
                        {
                            CanJump = true;
                        }
                    }
                }
                    // CHECK TO SEE IF ON THE GROUND HERE using raycast check the blue booklet for this
                    // then set the can jump back to true here if able
                    // Also see about adjusting animation
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
        MyAnimator = this.GetComponent<Animator>(); // is fine like this
        MySpriteRender = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            MyRig.velocity = LastMove + new Vector3(0, MyRig.velocity.y ,0);// New code added here to comensate for falling

        }
        if (IsClient)
        {
            if (MyRig.velocity.magnitude > .1f)
            {
                if (MyAnimator == null)
                {
                    this.transform.forward = MyRig.velocity.normalized;
                }
                else
                {
                    if (Mathf.Abs(MyRig.velocity.x) > Mathf.Abs(MyRig.velocity.z)) // this'll need an overhaul
                    {
                        MyAnimator.SetInteger("Animate", 1);
                        if (MyRig.velocity.x > 0.1f)
                        {
                            MySpriteRender.flipX = false; // right always goes to else when right
                                                          //MyAnimator.SetInteger("Animate", 1);
                        }
                        else
                        {
                            MySpriteRender.flipX = true; // left 
                                                         //MyAnimator.SetInteger("Animate", 1);
                        }
                    }
                    
                }
            }
          else if (Shootready == true  && CanJump == true) // AHAHAHAHAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHA AH AHHHHHHHHHHHHH
            {
                // this should also work fo the jump as it is cheking the velocity magnitude being more then .1 therefore as long as he moving and not shootin it should be good.
                MyAnimator.SetInteger("Animate", 0); // we need this here to make the idles run properly, but it breaks shoot and jump
            }
        }
    }
    public IEnumerator CorutineForBullet()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsServer)
        {
            SendUpdate("SHOOT_ANI", "0");
        }
        /// MyAnimator.SetInteger("Animate", 0); // don't place the shoot animation here.
        Shootready = true;// wait half a frame
    }


 public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") 
        {

            if (IsServer)
            {
                SendUpdate("JUMP_ANI", "0");
            }
   //    CanJump = true; // set jumping to true while on the ground
            // ALSO SOMETHING TO TURN OFF GRAVITY FROM CONTINUOUSLY GOING UP
        } 
    }
 public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") // USE  " collision.gameObject.tag " for the object you collide with!
        {
           
          //  CanJump = false; // else mnake it false when you leave the floor? or should we simply make it when we press jump?... thinking it latter now
        }
    }
    // now we need to set up where when vertical is > 0  add velocity equal to speed
}


/*
Trigger does not sync usually because it to fast, can use velocity on the client side to set animation?
 

//this.transform.forward = new Vector3(MyRig.velocity.x, 0, MyRig.velocity.z).normalized; // Cant use this for the tank version we'll have to move this since no longer cosmetic
                // normalized is at the end because, don't care mag just use direction
 
 
 
 
 Check lan Game is working
 Copy contets of lan scene minus Lan manager into the online scene
 Wan Manage 0 network core spawn prefab array populate
has to be exactly like it is in lan scence
 Public / Private IP and port range you have to set
 */
