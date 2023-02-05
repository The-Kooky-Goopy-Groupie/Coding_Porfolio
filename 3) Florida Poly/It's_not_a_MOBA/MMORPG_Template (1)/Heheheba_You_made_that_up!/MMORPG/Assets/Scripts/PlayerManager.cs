using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    Process Server2;
    Process Server3;
    bool isServer = false;

    // player info goes here
    public string PNAME; // player name
    public int LastScene = 0; // last scene player was in
    public int CurrentScene = 0;
    // Start is called before the first frame update
    void Start()
    {


        SceneManager.sceneLoaded += SceneChanger;

        string[] args = System.Environment.GetCommandLineArgs();
       
        int goalScene = 0;
        
        for(int i = 0; i < args.Length; i++)
        {
            if (args[i] == "SERVER1"){
                // spawn server 2 and 3

                // can place a debug for checking
                goalScene = 1;
                isServer = true;

                Server2 = new Process();
                Server2.StartInfo.FileName =  args[0];
                Server2.StartInfo.Arguments = "SERVER2";
                Server2.StartInfo.CreateNoWindow = false;
                Server2.Start();



                //3
                // can place a debug for checking
                goalScene = 1;
                Server3 = new Process();
                Server3.StartInfo.FileName =  args[0];
                Server3.StartInfo.Arguments = "SERVER3";
                Server3.StartInfo.CreateNoWindow = false;
                Server3.Start();

            }
            if(args[i] == "SERVER2")
            {
                isServer = true;
                goalScene = 2;
            }
            if (args[i] == "SERVER3")
            {
                isServer = true;
                goalScene = 3;
            }
        }


        if(goalScene != 0)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(goalScene);
        }
    }


    public  void SetName (string p)
    {
        PNAME = p;
    }

    public void StartClient()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void SceneChanger(Scene s, LoadSceneMode m)
    {
        CurrentScene = s.buildIndex;
        StartCoroutine(SlowStart());
    }

    IEnumerator SlowStart()
    {
        yield return new WaitForSeconds(.5f); // wait here very imporant as it gives it time to reconnect
        if (isServer)
        {
            GameObject.FindObjectOfType<NETWORK_ENGINE.NetworkCore>().StartServer();
        }
        else
        {
            GameObject.FindObjectOfType<NETWORK_ENGINE.NetworkCore>().StartClient();
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
