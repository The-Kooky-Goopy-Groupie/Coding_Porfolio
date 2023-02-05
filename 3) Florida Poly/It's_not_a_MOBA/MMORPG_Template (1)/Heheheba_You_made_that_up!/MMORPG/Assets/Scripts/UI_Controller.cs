using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UIController());

    }
    public IEnumerator UIController()
    {
        while(true)
        {
            yield return new WaitUntil(() => GameObject.FindObjectOfType<NETWORK_ENGINE.NetworkCore>().IsConnected);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitWhile(() => GameObject.FindObjectOfType<NETWORK_ENGINE.NetworkCore>().IsConnected);
            SceneManager.LoadScene(0);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
