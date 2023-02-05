using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class NetworkCharScript : NetworkComponent
{
    // this is bassically  the player controloer location
    public int HP = 10;
    public Text UiText;
    public string NameTxt = "Coolness";
    // team does not need to be synchronized
    public int Team;
    
    public override void HandleMessage(string flag, string value)
    {
       // nothing is needed here at the moment
    }

    public override void NetworkedStart()
    {
       // nothing
    }

    public override IEnumerator SlowUpdate()
    {
        foreach (LobbyManagerScript lp in GameObject.FindObjectsOfType<LobbyManagerScript>()) // finding every network player lobby script and making an array of them
        {
            if(lp.Owner == this.Owner) // checks that this person is the script owner
            {
                NameTxt = lp.LastMessage;
                UiText.text = NameTxt;
                yield return new WaitUntil(() => lp.IsReady);
                Team = lp.Team;
                switch(Team) // used to change the objects color
                {
                    case 0:
                        this.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
                        break;
                    case 1:
                        this.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 255);
                        break;
                    case 2:
                        this.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
                        break;
                    case 3: // Green 
                        this.GetComponent<Renderer>().material.color = new Color(255, 255, 0, 128);
                        
                        break;
                }
            }
        }
        yield return new WaitForSeconds(.1f);
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
