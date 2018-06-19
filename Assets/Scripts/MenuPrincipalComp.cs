using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalComp: MonoBehaviour {


    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);

        if ( UnityAdControler.showAds)
        {
            UnityAdControler.ShowAd();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
