using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdControler : MonoBehaviour {

    //Tipo que pode ser null
    public static DateTime? proxTempoReward = null;

    /// <summary>
    /// Variavel de controle se devemos ou nao mostrar ads
    /// </summary>
    public static bool showAds = true;

    /// <summary>
    /// Metodo para invocar anuncios
    /// </summary>
    public static void ShowAd()
    {

#if UNITY_ADS

        //Opcoes para o ad
        ShowOptions opcoes = new ShowOptions();
        opcoes.resultCallback = Unpause;

        if (Advertisement.IsReady()) {
            //Mostra o anuncio
            Advertisement.Show(opcoes);
        }
        //Pausar o jogo enquanto
        //o ad esta sendo mostrad
        MenuPauseComp.pausado = true;
        Time.timeScale = 0;
#endif
    }
    /// <summary>
    /// Metodo para mostrar ad com recompensa
    /// </summary>
    public static void ShowRewardAd()
    {

#if UNITY_ADS

        proxTempoReward = DateTime.Now.AddSeconds(15);
        if (Advertisement.IsReady()) {
            // Pausar o jogo
            MenuPauseComp.pausado = true;
            Time.timeScale = 0f;
            //Outra forma de criar a 
            //instancia do ShowOptions e setar o callback
            var opcoes = new ShowOptions {
                resultCallback = TratarMostrarResultado
            };
            
            Advertisement.Show(opcoes);
        }
#endif
    }

    private static void Unpause(ShowResult obj)
    {
        //Quando o anuncio acabar 
        //sai do modo pausado
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }

#if UNITY_ADS
    public static void TratarMostrarResultado(ShowResult result) {

        switch (result) {
            case ShowResult.Finished:
                // Anuncio mostrado. Continue o jogo
                GameObject.FindObjectOfType<Controlador>().Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad pulado. Faz nada");
                break;
            case ShowResult.Failed:
                Debug.LogError("Erro no ad. Faz nada");
                break;
        }

    // Saia do modo pausado
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }
#endif

}
