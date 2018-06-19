﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controlador : MonoBehaviour {

    private GameObject jogador;

    //[Tooltip("Quanto tempo antes de reiniciar o jogo")]
    //private float tempoEspera = 2.0f;

    [Tooltip("Uma referencia para o tile basico")]
    [SerializeField]
    private Transform tile;

    [Tooltip("Uma referencia para o osbstaculo")]
    [SerializeField]
    private Transform obstaculo;

    /// <summary>
    /// Ponto inicial do primeiro tile
    /// </summary>
    private Vector3 pontoInicial = new Vector3(0, 0, 5);

    [SerializeField]
    [Range(10, 50)]
    private int numInitSpawn;

    [SerializeField]
    [Range(1, 4)]
    private int numTileSemObs = 4;

    /// <summary>
    /// Posicao do proximo tile
    /// </summary>
    private Vector3 proxTilePos;

    /// <summary>
    /// Rotacao do proximo tile
    /// </summary>
    private Quaternion proxTileRot;

    // Use this for initialization
    void Start() {

        //Preparando o ponto inicial
        proxTilePos = pontoInicial;
        proxTileRot = Quaternion.identity;

        for (int i = 0; i < numInitSpawn; i++)
            SpawnProxTile(i >= numTileSemObs);

    }

    public void SpawnProxTile(bool spawnObs = true) {

        var novoTile = Instantiate(tile, proxTilePos,
            proxTileRot);

        //Detectar qual o local do proximo
        var proxTile = novoTile.Find("PontoSpawn");
        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;

        //Verifica se podemos criar obstaculos
        if (!spawnObs)
            return;

        //Iniciar o tratamento da criacao de osbtaculos
        var pontosObs = new List<GameObject>();

        //Varrer o tile basico para buscarmos os pontos obs
        foreach (Transform filho in novoTile) {
            if (filho.CompareTag("Obstaculo"))
                pontosObs.Add(filho.gameObject);
        }

        //Garantir que existe pelo menos um ponto de obs
        if (pontosObs.Count > 0) {

            //Pegar uma ponto spawn aleatorio
            var pontoSpawn = pontosObs[UnityEngine.Random.Range(0, pontosObs.Count)];

            //Pegamos a posicao do ponto de spawn
            var pontoSpawnPos = pontoSpawn.transform.position;

            var novoObs = Instantiate(obstaculo,
                            pontoSpawnPos,
                            Quaternion.identity);

            //Faz o obstaculo ser filho do seu ponto de spawn
            novoObs.SetParent(pontoSpawn.transform);
        }
    }

    public void DestroyObj(GameObject gameObject){

        //print("DestroyObj "+ gameObject.name);
        Destroy(gameObject);
    }

    public void ResetGame(GameObject goJogador)
    {
        jogador = goJogador;

        var gameOverMenu = GetGameOverMenu();

        gameOverMenu.SetActive(true);

        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();

        Button botaoContinue = null;
        foreach(var botao in botoes)
        {
            if (botao.name.Equals("BotaoContinue"))
            {
                botaoContinue = botao;
                break;
            }
        }

        if(botaoContinue != null)
        {
#if UNITY_ADS
            //Se o botao continue for clicado, iremos tocar o anúncio
            StartCoroutine(ShowContinue(botaoContinue));
            //botaoContinue.onClick.AddListener(UnityAdControler.ShowRewardAd);
#else
            //Se nao existe add, nao precisa mostrar o botao Continue
            botaoContinue.gameObject.SetActive(false);
#endif
        }

        ////print("ResetGame ");
        //Invoke("Reset", tempoEspera);
    }

    /// <summary>
    /// Metodo para reiniciar o jogo
    /// </summary>
    private void Reset()
    {
        //Reinicia o level
        //print("SceneName " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);
    }

    /// <summary>
    /// Busca o MenuGameOver
    /// </summary>
    /// <returns>O GameObject MenuGameOver</returns>
    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOverPainel").gameObject;
    }


    public IEnumerator ShowContinue(Button botaoContinue)
    {
        var btnText = botaoContinue.GetComponentInChildren<Text>();
        while (true)
        {
            if (UnityAdControler.proxTempoReward.HasValue && (DateTime.Now < UnityAdControler.proxTempoReward.Value))
            {
                botaoContinue.interactable = false;

                TimeSpan restante = UnityAdControler.proxTempoReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);
                btnText.text = contagemRegressiva;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                botaoContinue.interactable = true;
                botaoContinue.onClick.AddListener(UnityAdControler.ShowRewardAd);
                btnText.text = "Continue (Ver Ad)";
                break;
            }
        }
    }

}
