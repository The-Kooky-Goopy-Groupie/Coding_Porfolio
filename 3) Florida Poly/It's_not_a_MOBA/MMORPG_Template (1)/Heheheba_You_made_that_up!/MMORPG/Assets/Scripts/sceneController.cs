using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour {

    public bool beenUsed = false;
	// Use this for initialization
	void Start ()
    {
        //Debug.Log("start");
        //Debug.Log("The scene name for the first scene is " + SceneManager.GetSceneAt(0).name);
        SceneManager.sceneLoaded+=SceneChanger;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SceneChanger(Scene s, LoadSceneMode m)
    {
        //Debug.Log("Called SceneChanger.");
        Debug.Log("The scene name is " + s.name);
        //Debug.Log("The scene name for the first scene is " + SceneManager.GetSceneByBuildIndex(0).name);
        if (beenUsed && s.buildIndex == 0)
        {
            SceneManager.sceneLoaded -= SceneChanger;
            Destroy(this.gameObject);
        }
        
        if(s.name != SceneManager.GetSceneByBuildIndex(0).name)
        {
            Debug.Log("Setting beenUsed to true!");
            beenUsed = true;
        }
   

    }
    
    /*public void switchScenes()
    {
        if (!beenUsed)
        {
            Debug.Log("Loading game scene");
            SceneManager.LoadScene(1);//Or whatever index you want.
        }
        else
        {
            Debug.Log("Loading Menu scene");
            SceneManager.LoadScene(0);
        }
    }*/
}
