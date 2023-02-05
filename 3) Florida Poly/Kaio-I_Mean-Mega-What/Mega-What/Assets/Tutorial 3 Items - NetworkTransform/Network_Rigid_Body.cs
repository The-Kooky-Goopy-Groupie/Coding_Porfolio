using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
[RequireComponent(typeof(Rigidbody))]
public class Network_Rigid_Body : NetworkComponent
{


    public Vector3 LastPosition;
    public Vector3 LastRotation;
    public Vector3 LastVelocity; // CONGRATS... ATM YOUR USEFULLLL
    public Vector3 LastAngular;

    public Vector3 OffsetVelocity;


    public float Threshold = .1f;
    public float EThreshold = 2.5f;

    public bool UseOffsetVelocity = true;

    public Rigidbody MyRig;
    bool recvPacket = false;


    public static Vector3 VectorFromString(string value)
    {
        char[] temp = { '(', ')' };
        string[] args = value.Trim(temp).Split(',');
        return new Vector3(float.Parse(args[0].Trim()), float.Parse(args[1].Trim()), float.Parse(args[2].Trim()));
    }

    public override void HandleMessage(string flag, string value)
    {
        if(!recvPacket && IsClient)
        {
            recvPacket = true;
        }
        if (flag == "POS"&& IsClient)
        {
            LastPosition = VectorFromString(value);
            float d = (MyRig.position - LastPosition).magnitude;
            if(d > EThreshold || !UseOffsetVelocity || LastVelocity.magnitude < .1)
            {
                OffsetVelocity = Vector3.zero;
                MyRig.position = LastPosition;
            }
            else if (LastVelocity.magnitude > .1)
            {
                OffsetVelocity = (LastPosition - MyRig.position);
            }
        }

        if (flag == "VEL" && IsClient)
        {
            LastVelocity = VectorFromString(value);
         /*   if (LastVelocity.magnitude < .05f)
            {
                OffsetVelocity = Vector3.zero;
                MyRig.position = LastPosition;
            }
            if (UseOffsetVelocity)
            {
                MyRig.velocity = LastVelocity + OffsetVelocity;
            }
            else 
            {
                MyRig.velocity = LastVelocity;
            } */
        }

        if(flag == "ROT" && IsClient)
        {
            LastRotation = VectorFromString(value);
            MyRig.rotation = Quaternion.Euler(LastRotation);
        }

        if (flag == "ANG" && IsClient) 
        {
            LastAngular = VectorFromString(value);
           // MyRig.angularVelocity = LastAngular;
        }

    }




    public override void NetworkedStart()
    {
       // throw new System.NotImplementedException();
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
            MyRig.useGravity = false;
        }
        while (true)
        {
            if (IsServer)
            {
                if ((LastPosition - MyRig.position).magnitude > Threshold)
                {
                    SendUpdate("POS", MyRig.position.ToString("F3"), false); // or is it still f1? GIVES DIGET AFTER DECIMAL
                    LastPosition = MyRig.position;
                }
                if ((LastVelocity - MyRig.velocity).magnitude > Threshold)
                {
                    SendUpdate("VEL", MyRig.velocity.ToString("F3"), false); // or is it still f1? GIVES DIGET AFTER DECIMAL
                    LastVelocity = MyRig.velocity;
                }


                if ((LastRotation - MyRig.rotation.eulerAngles).magnitude > Threshold)
                {
                    SendUpdate("ROT", MyRig.rotation.eulerAngles.ToString("F3"), false); // or is it still f1? GIVES DIGET AFTER DECIMAL
                    LastRotation = MyRig.rotation.eulerAngles;
                }


                if ((LastAngular - MyRig.angularVelocity).magnitude > Threshold)
                {
                    SendUpdate("ANG", MyRig.angularVelocity.ToString("F3"), false); // or is it still f1? GIVES DIGET AFTER DECIMAL
                    LastAngular = MyRig.angularVelocity;
                }

                if (IsDirty) {
                    SendUpdate("POS", MyRig.position.ToString("F3"), false);
                    SendUpdate("VEL", MyRig.velocity.ToString("F3"), false);
                    SendUpdate("ROT", MyRig.rotation.eulerAngles.ToString("F3"), false);
                    SendUpdate("ANG", MyRig.angularVelocity.ToString("F3"), false);
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyRig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient && recvPacket) { 
            if (LastVelocity.magnitude < .05f)
            {
                OffsetVelocity = Vector3.zero;
                MyRig.position = LastPosition;
            }

            if (UseOffsetVelocity)
            {
                MyRig.velocity = LastVelocity + OffsetVelocity;
            }
            else
            {
                MyRig.velocity = LastVelocity;
            }

            if(LastAngular.magnitude < .05f)
            {
                MyRig.rotation= Quaternion.Euler(LastRotation);
                LastAngular = new Vector3(0,0,0);
            }
            else
            {
                MyRig.angularVelocity = LastAngular;
            }
            
        }
    }
}
