using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObsComp : MonoBehaviour {

    //[Tooltip("Quanto tempo antes de reiniciar o jogo")]
    //private float tempoEspera = 2.0f;

    [SerializeField]
    [Tooltip("Referencia para a explosao")]
    private GameObject explosao;

    private Controlador controlador;

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.GetComponent<JogadorComp>()) {
            //Destroy(collision.gameObject);
            ObjetoTocado();
            //controlador.ResetGame();
            ////Invoke("Reset", tempoEspera);

            collision.gameObject.SetActive(false);

            controlador.ResetGame(collision.gameObject);
        }

    }

    ///// <summary>
    ///// Metodo para reiniciar o jogo
    ///// </summary>
    //private void Reset() {
    //
    //    //Reinicia o level
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    /// <summary>
    /// Metodo para verificar se o obstaculo foi tocado
    /// </summary>
    public void ObjetoTocado()
    {
        print("Aqui");
        if (explosao)
        {

            var particulas = Instantiate(explosao, transform.position, Quaternion.identity);
            Destroy(particulas, 1.0f);
        }
        controlador.DestroyObj(this.gameObject);
        //Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        controlador = GameObject.FindObjectOfType<Controlador>();
    }

}
