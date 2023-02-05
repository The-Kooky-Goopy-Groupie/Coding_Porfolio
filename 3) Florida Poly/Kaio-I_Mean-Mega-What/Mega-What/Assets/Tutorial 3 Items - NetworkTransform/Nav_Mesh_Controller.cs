using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.AI;

// Current Issue Is not properly rendering on client end

[RequireComponent(typeof(NavMeshAgent))]
public class Nav_Mesh_Controller : NetworkComponent
{
    public Vector3 Goal; // the goal for the object to go to
    NavMeshAgent MyAgent; // the nav mesh
    public Vector3 LastGoalStorage; // used to store the original pathing of the goal
    public int HP = 3; // ammount of hits that these enemies can take

    public Vector3 PlayerInRange; // any other sort of stat values can go here like HP, Atk, Def
 
    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {
            // does nothing really here
        }
        if (IsServer)
        {
            MyAgent.SetDestination(Goal);
        }
        while (IsServer)
        {
            if(MyAgent.remainingDistance < .1f && PlayerInRange == Vector3.zero) // once in range of the goal
            {
                Goal = new Vector3(Goal.x * -1, Goal.y, Goal.z); // flips the goals back to the negative after being done.
                LastGoalStorage = Goal;
                MyAgent.SetDestination(Goal); // makes this the new goal
            }

            PlayerInRange = new Vector3 (0,0,0);
            foreach (Rigid_Body_Control lp in GameObject.FindObjectsOfType<Rigid_Body_Control>())
            {
                if ((lp.MyRig.position - this.transform.position).magnitude < 2.7f) //
                { // make this the distance away from the player object it is chasing this neededs fixing
                  //  LastGoalStorage = Goal; // perserve normal goal path
                    PlayerInRange = lp.MyRig.position;
                    Goal = PlayerInRange;  // need to find the player position here thsi needs fixing too to get player postiotion 
                    MyAgent.SetDestination(Goal);
                }
                
            }
            if(PlayerInRange == Vector3.zero) // vector3.zero = 
            {
                Goal = LastGoalStorage; // set back to orginal pathing
                MyAgent.SetDestination(Goal); // start moving back on original pathing
                                              // then send this info that all of the above here to the clients

                //  foreach (Rigid_Body_Controler lp in GameObject.FindObjectsOfType<Rigid_Body_Controler>())
            }
            yield return new WaitForSeconds(.1f);  // only update every ten frames
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        MyAgent = this.GetComponent<NavMeshAgent>();
        if(MyAgent == null)
        {
            throw new System.Exception("FAIL: You forgot your nav Mesh Agent.");
        }
    }

    public override void HandleMessage(string flag, string value)
    {
        //nothing goes in here
    }

    public override void NetworkedStart()
    {
        // Nothing of note here
    }


    void Update()
    {

        // Update is called once per frame but nothing is happening
    }

    public void OnCollisionEnter(Collision collision) //collision collision (2nd collision is the object) 
    {
        Debug.Log("a");
        if (IsServer)
        {
            Debug.Log("aA");
            if (collision.gameObject.tag == "Bullet")
            {
                Debug.Log("aaa");
                HP = HP - 1;
                // we shouldn't have to sync this since the server is the only one who needs this info, we do need to sync the death
                
                MyCore.NetDestroyObject(collision.gameObject.GetComponent<NetworkID>().NetId); // collision to get the object colliding, game object to get the game object then get componet to get it network id then after that get it net id
                Debug.Log("aaaa");

                if (HP == 0)
                {
                    MyCore.NetDestroyObject(this.NetId); // Dont even need my id 
                    Debug.Log("aaaaa");
                    //SendUpdate("Dead", ""); // we should just need the tag here to say they dead
                }
            }
        }
    }
}
