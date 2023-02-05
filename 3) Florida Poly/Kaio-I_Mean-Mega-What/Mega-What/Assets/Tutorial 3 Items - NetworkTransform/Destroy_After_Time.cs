using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Destroy_After_Time : NetworkComponent
{
    public float WaitTime;
    public override void HandleMessage(string flag, string value)
    {
       // throw new System.NotImplementedException();
    }

    public override void NetworkedStart()
    {
        // throw new System.NotImplementedException();
    }

    public override IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(WaitTime);
        MyCore.NetDestroyObject(this.NetId);
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
