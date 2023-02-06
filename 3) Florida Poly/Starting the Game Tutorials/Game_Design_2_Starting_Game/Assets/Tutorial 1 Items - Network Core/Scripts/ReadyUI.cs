using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;

public class ReadyUI : NetworkComponent
{
    // critical  data that needs to be synced -needs to be contre
    public bool IsReady = false;

    // UI element that will be set in editor
    public Text statusMessage; //used to change the text message
    public Toggle toggle; // for our button

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "TOG") {
            if (IsServer)
            {
                IsReady = bool.Parse(value);
                SendUpdate("TOG", value);
            }
            if (IsClient)
            {
                IsReady = bool.Parse(value);
                
                if(!IsLocalPlayer)
                {
                    toggle.isOn = IsReady;
                }
                
                if (IsReady) // this area works
                {
                    statusMessage.text = "Player is Ready!";
                }
                else 
                {
                    statusMessage.text = "Player is Not ready!";
                }
            
            }
        }
    }
    // only the instance that shares the forign network ID value will get this message

   public override void NetworkedStart(){}

    public override IEnumerator SlowUpdate()
    {
        this.transform.position = new Vector3(-5 + Owner*10, 0, 0);
        if (!IsLocalPlayer)
        {
            toggle.interactable = false;
        }
        while(IsServer)
        {
            if (IsDirty)
            {
                SendUpdate("TOG", IsReady.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(.1f);// keep in while loop or this causes a crash
        }
      
    }

    void Start(){}
  // Update is called once per frame
    void Update() {}
    public void OnToggleClick(bool t)
    {
        if (MyId.IsInit && IsLocalPlayer)
        {
            // IsReady=t would be wrong here gives client control
            SendCommand("TOG", t.ToString());
        }            
    }
}

// What we know - something is wrong with the UI itself showing up outside of the editor, the canvas for the NPC dummy object does not spawn, and will 