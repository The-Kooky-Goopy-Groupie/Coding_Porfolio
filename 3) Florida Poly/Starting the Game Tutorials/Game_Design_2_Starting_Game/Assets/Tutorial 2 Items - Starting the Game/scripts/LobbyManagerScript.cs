using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;

public class LobbyManagerScript : NetworkComponent
{
    // Why these were choosen is the dropdown gives an int and bool is given from the toggle, the int can also repersent the type as well.
    public int  Character; // index for cap is 0 cubey is 1, not always possible to do this but if order is perserved you can use math to get this.
    public int  Team;
    public bool IsReady = false;
    
    // The UI Elements 
    public Dropdown CharacterSelect; // i really shouldn't have to explain these two.
    public Dropdown TeamSelect;
    public Toggle   ReadyToggle;
    public Image    LobbyBackground;

    // status message code
    public Text statusMessage;
    public string LastMessage = "";
    public InputField ChatBubble;

    public override void HandleMessage(string flag, string value)
    {
        switch (flag) // check for any of the flag messages
        {
            case "TEAM": // for if a team signal is sent
                Team = int.Parse(value.ToString());
                if(IsServer)
                {
                    SendUpdate("TEAM", value); // used to send back the team values to the client
                }
                if (IsClient) {
                    switch (Team)
                    {// check that my order matches, bugged that it only changes bkgn color, and this doesnt make the color right.
                        case 0: // RED
                            LobbyBackground.color = new Color(255, 0, 0, 128);
                            break;
                        case 1: //Blue
                            LobbyBackground.color = new Color(0, 0, 255, 128);
                            break;
                        case 2: // Green 
                            LobbyBackground.color = new Color(0, 255, 0, 128);
                            break;
                        case 3: // Green 
                            LobbyBackground.color = new Color(255, 255, 0, 128);
                            break;
                    }  
                 }
                break;
        //check the character selection    
        case "CHAR":
            Character= int.Parse(value.ToString()); // checks over the value of the charactor string and returns it
                if (IsServer)
                {
                    SendUpdate("CHAR", value);
                }
         break;
        //checks if the player is ready
            case "READY":
                IsReady = bool.Parse(value.ToString());
                if (IsClient)
                {
                    ReadyToggle.isOn = IsReady;
                    /*disable the toggle
                     if(IsReady)
                    {
                    ReadyToggle.interactable = false;
                    }*/
                }
                if (IsServer)
                {
                    SendUpdate("READY", value); // used to check if everyone is ready and send back the info
                    if (IsReady) 
                    {//-1 is wrong, put owner if it controled by the player, -1 is for the server objects, since the server is creating this for the player we must use owner
                     //  MyCore.NetCreateObject(Character, Owner, this.transform.position - new Vector3(0, .5f, 0), Quaternion.identity);
                     // this is used in unsyncronized start for the above and the toggle interactable off
                     // thus the above spawns the player character, used for asyncronyis start.
                    }
                }
                break;

            case "OIS":
                    //  LastMessage = value;  would be an optimization as it checks regardless of the sever. 
                    if (IsServer)
                    {
                        LastMessage = value; // set the last message sent by client to the last message variable.
                        SendUpdate("OIS", value); // This sends the last message that was stored to the clients 
                    }
                    if (IsClient)
                    {
                        LastMessage = value; //shows that it stores the value 
                        statusMessage.text = LastMessage; //render then the value that is outputed. you can statusMessage.text = value
                    }
                break;
        }
    }

   

    public override IEnumerator SlowUpdate()
    {
        if (!IsLocalPlayer) // makes sure only the one who is the local player can use the string.
        {
            ReadyToggle.interactable = false; 
            TeamSelect.gameObject.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            ChatBubble.interactable = false;
        }

        switch (Owner) // used to display the 4 canvas onto the screen. more hackish, would be better to have a canvas that has a grid layout and makes panels
        {
            case 0:
                this.transform.position = new Vector3(-5.5f, 5.5f, 10);
                break;
            case 1:
                this.transform.position = new Vector3(5.5f, 5.5f, 10);
                break;
            case 2:
                this.transform.position = new Vector3(-5.5f, -5.5f, 10);
                break;
            case 3:
                this.transform.position = new Vector3(5.5f, -5.5f, 10);
                break;
        }

        while (MyCore.IsConnected) // test  if Mycore. is needed
        {
            if (IsLocalPlayer)
            {
                // input can go here
                // Any local player only notifications
            }
            if (IsClient)
            {
                // everyone who is on the server is there
            }
            if (IsServer) 
            {
                // all logic that affects the gamestates goes here
                if (IsDirty) // is used to check that all the objects are up to date
                {
                    SendUpdate("READY", IsReady.ToString());
                    SendUpdate("CHAR", Character.ToString());
                    SendUpdate("TEAM", Team.ToString());
                    SendUpdate("OIS", LastMessage);
                    IsDirty = false; // having the Is Dirty actually solves the default check issue that could occur
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }



 // below are sending the commands to the server codes
    public void SetTeam(int t)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("TEAM", t.ToString());
            // What need to do in the editor
        }
    }
    public void SetCharacter(int c)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("CHAR", c.ToString());// no  Char = c;
        }
    }
    public void SetReady(bool r)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("READY", r.ToString());
        }
    }

    public void OnInputString(string statusMessage) //InputString we want to use this where after we press enter we send the string from client to serever rather then a bol
    {
        if (MyId.IsInit && IsLocalPlayer) // what does this mean?
        {
            SendCommand("OIS", statusMessage.ToString());  // used to check for the the string and send 
                                                           //OIS = OnInputString
                                                           // Can't have any network componets that have the same flag names
        }
    }

    //None of the below is really needed
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public override void NetworkedStart()
    {
        // nothing needed here  
    }
}

// Current Issue - Doesn't show the UI at all, acts as if it instantly goes into the game
// things to note,
// build seems to say it is still building the network componet even though is different object