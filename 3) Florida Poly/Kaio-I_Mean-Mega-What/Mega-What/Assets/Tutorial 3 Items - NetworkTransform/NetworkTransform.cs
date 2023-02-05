using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE; // Network componet need a network engine


public class NetworkTransform : NetworkComponent
{
    // the Last Pos rot and Scale of your object
    public Vector3 LastPosition;
    public Vector3 LastRotation;
    public Vector3 LastScale; // CONGRATS... ATM YOUR USELESSSSSSSSSSSSSS
    //Min and Max threshold
    public float MinThreshold = .1f;
    public float MaxThreshold = .5f;
    
    public bool Smooth = true;
    // how fast it Moves
    public float NavMeshSpeed = 1;
    public float RotationSpeed = Mathf.PI / 2;



    public static Vector3 VectorFromString(string value)
    {
        char[] temp = { '(', ')' };
        string[] args = value.Trim(temp).Split(',');
        return new Vector3(float.Parse(args[0].Trim()), float.Parse(args[1].Trim()), float.Parse(args[2].Trim()));
    }

    public override void HandleMessage(string flag, string value)
    {
      if(flag == "POS")
        {
            Vector3 temp = NetworkTransform.VectorFromString(value);
            if((temp-this.transform.position).magnitude > MaxThreshold || !Smooth) // this is a differnet valuse on
            {
                // want to use saparinfly
                this.transform.position = temp;
            }
            LastPosition = temp;
        }

        if (flag == "ROT")
        {
            Vector3 temp = NetworkTransform.VectorFromString(value);
            if((temp -this.transform.rotation.eulerAngles).magnitude < MaxThreshold || !Smooth)
            {
                Quaternion qt = new Quaternion();
                qt.eulerAngles = temp;
                this.transform.rotation = qt;
            }
            LastRotation = temp;
        }

    if(flag == "SCALE")
        {
            Vector3 temp = NetworkTransform.VectorFromString(value);
            this.transform.localScale = temp;
            LastScale = temp;
        }





    }

    public override void NetworkedStart()
    {
      //
    }

    public override IEnumerator SlowUpdate()
    {
        while (MyCore.IsConnected)
        {
            if (IsServer)
            {
                float DistCheck = (this.transform.position - LastPosition).magnitude;
                if (DistCheck > MinThreshold)
                {
                    Debug.Log("sending Update on Position!");
                    SendUpdate("POS", this.transform.position.ToString("F2")); // or is it still f1? GIVES DIGET AFTER DECIMAL
                    LastPosition = this.transform.position;
                }
                float CheckRotation = (this.transform.rotation.eulerAngles - LastRotation).magnitude;
                if (CheckRotation > MinThreshold)
                {
                    SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString("F2"));
                    LastRotation = this.transform.rotation.eulerAngles;              
                }

                float CheckScale = (this.transform.localScale - LastScale).magnitude;
                if (CheckScale > MinThreshold)
                {
                    SendUpdate("SCALE", this.transform.localScale.ToString("F3"));
                    LastScale = this.transform.localScale;
                }
                if (IsDirty)
                {
                    SendUpdate("POS", this.transform.position.ToString("F2"));
                    SendUpdate("ROT", this.transform.rotation.eulerAngles.ToString("F2"));
                    SendUpdate("SCALE", this.transform.localScale.ToString("F3"));
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.05f);
            // this is the best performance you can get with this network engine???
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsClient && Smooth)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, LastPosition, .2f);
            Quaternion qt = new Quaternion();
            qt.eulerAngles = Vector3.Lerp(this.transform.rotation.eulerAngles, LastRotation, RotationSpeed * Time.deltaTime);
            this.transform.rotation = qt;
        }
    }
}
