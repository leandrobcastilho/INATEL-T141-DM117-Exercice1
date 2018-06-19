using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComp : MonoBehaviour
{

    public static bool pausado;

    [SerializeField]
    private GameObject menuPausePanel;

    /// <summary>
    /// Metodo para reiniciar a scene
    /// </summary>
    public void Restart()
    {
        //SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Metodo para pausar o jogo
    /// </summary>
    /// <param name="isPausado"></param>
    public void SetPauseMenu(bool isPausado)
    {
        pausado = isPausado;

        //Se o jogo estiver pausado, timescale recebe 0
        Time.timeScale = (pausado) ? 0 : 1;
        //Habilita/Desabilita o menu pause
        menuPausePanel.SetActive(pausado);
    }
    /// <summary>
    /// Metodo para carregar uma scene
    /// </summary>
    /// <param name="nomeScene"></param>
    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }

    // Use this for initialization
    void Start () {
        pausado = false;

#if !UNITY_ADS
        SetPauseMenu(false);
#endif

//#if UNITY_ANDROID
//        SetPauseMenu(false);
//#endif
//#if UNITY_IOS
//        SetPauseMenu(false);
//#endif
//#if UNITY_STANDALONE
//        SetPauseMenu(false);
//#endif
    }

    // Update is called once per frame
    void Update () {

    }
}
