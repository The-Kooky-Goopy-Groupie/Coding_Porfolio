using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class GameManage : NetworkComponent
    {
    public bool GameStarted = false; // makes sure game is not started at the start

    public override void HandleMessage(string flag, string value)
    {
        if(flag =="GAMESTART") // checks that a game has started
        {
            GameStarted = true;
            foreach (LobbyManagerScript lp in GameObject.FindObjectsOfType<LobbyManagerScript>())
            {
                lp.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }


    public override IEnumerator SlowUpdate()
    {
        // where the maagic happens
        while (!GameStarted && IsServer) // assumse that the game can start at the start
        {
            bool readyGo = true;
            int count = 0;
            foreach (LobbyManagerScript lp in GameObject.FindObjectsOfType<LobbyManagerScript>())
            {
                if (!lp.IsReady) // checking if the check is not ready
                {
                    readyGo = false;
                    break;
                }
                count++;
            }
         if (count < 2)
            {
                readyGo = false;
            }
            GameStarted = readyGo;
            yield return new WaitForSeconds(2);
        }
        if (IsServer)
        {
            SendUpdate("GAMESTART", GameStarted.ToString());

            foreach (LobbyManagerScript lp in GameObject.FindObjectsOfType<LobbyManagerScript>())
            {
                MyCore.NetCreateObject(lp.Character, lp.Owner, lp.transform.position - new Vector3(0, .5f, 0), Quaternion.identity);
            }

          
        }

        while (IsServer)
        {
            if(IsDirty)
            {
                SendUpdate("GAMESTART", GameStarted.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(1);
        }
    }
    


    // Start is called before the first frame update
    void Start()
    {
        GameStarted = false; // oh hey your not useless this time! good for you! 
    }// i still feel pretty useless sometimes like right now.
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void NetworkedStart()
    {
       // nothing goes here
    }

}
