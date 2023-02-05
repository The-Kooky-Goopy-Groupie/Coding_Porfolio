using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;

public class GameCharacter : NetworkComponent
{
    public string Pname;
   // below not used this time
   // public int Color;
   // public Text MyTextbox;
   // public int Score = 0;
   // public GameObject AttackBox;

   
    public override void HandleMessage(string flag, string value)
    {
        if(flag == "PNAME")
        {
            Pname = value;
            if (IsServer)
            {
                SendUpdate("PNAME", Pname);
            }
        }
        if(flag == "MOVE")
        {
            string[] args = value.Split(',');
            Vector3 moveTemp = new Vector3(int.Parse(args[0]), 0, int.Parse(args[1]))*3;//I should use existing Y velocity to prevent floating. 
            gameObject.GetComponent<Rigidbody>().velocity = moveTemp;
        }
        if (flag =="SCENE_INFO")
        {
            GameObject temp = GameObject.Find(value); //0_1
            if(temp != null)
            {
                this.transform.position = temp.transform.position;
            }
        }
     }


    public override IEnumerator SlowUpdate()
    {

        if (IsLocalPlayer)
        {
            SendCommand("PNAME", GameObject.FindObjectOfType<PlayerManager>().PNAME);
            SendCommand("SCENE_INFO", GameObject.FindObjectOfType<PlayerManager>().LastScene.ToString()+"_" + GameObject.FindObjectOfType<PlayerManager>().CurrentScene.ToString());

            GameObject.FindObjectOfType<PlayerManager>().LastScene = GameObject.FindObjectOfType<PlayerManager>().CurrentScene;
        }
    
        while (true)
        {
            //Player contoller
            if (IsLocalPlayer)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                SendCommand("MOVE", h + "," + v);
            }

            if(IsServer)
            {

                if(IsDirty)
                {
                    SendUpdate("PNAME", Pname);
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    //Collisions

    //Triggers
    private void OnTriggerEnter(Collider other)
    {
  if(other.tag == "Finish" && IsLocalPlayer)
        {
            MyCore.Disconnect(MyCore.LocalPlayerId);
            int desired_Scene = int.Parse(other.name.Split('_')[1]); //DOOR_@
            UnityEngine.SceneManagement.SceneManager.LoadScene(desired_Scene);
        }
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
