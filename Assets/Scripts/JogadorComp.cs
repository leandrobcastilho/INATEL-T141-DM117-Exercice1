using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JogadorComp : MonoBehaviour {

    public enum TipoMovimentoHorizontal
    {
        Acelerometro,
        Touch
    }

    [SerializeField]
    [Tooltip("Define o tipo de movimento horizontal")]
    private TipoMovimentoHorizontal TipoMovimento = TipoMovimentoHorizontal.Touch;

    [Header("Variaveis de controle do Swipe")]
    [SerializeField]
    [Tooltip("Distancia minima para considerar um swipe")]
    [Range(1, 5)]
    private float miniDisSwipe = 2.0f;

    private float swipeMove = 2.0f;

    private Vector2 touchInicio;

    [Header("Variaveis de controle de Velociade")]
    [SerializeField]
    [Tooltip("A velocidade qua a bola ira esquivar")]
    [Range(1,10)]
    private float velocidadeEsquiva = 5.0f;

    [SerializeField]
    [Tooltip("Velocidade com qual a bola se desloca para a frente")]
    [Range(1,10)]
    private float velocidadeRolamento = 5.0f;

    /// <summary>
    /// Uma referencia para o corpo rigido
    /// </summary>
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (MenuPauseComp.pausado)
            return;

        var velocidadeHorizontal = 
                Input.GetAxis("Horizontal") 
                * velocidadeEsquiva;

//#if UNITY_STANDALONE
        //Detectando se houve clique com o botao
        if (Input.GetMouseButton(0))
        {
            velocidadeHorizontal = CalculaMovimento(Input.mousePosition);
            TocarObjeto(Input.mousePosition);
        }

//#elif UNITY_IOS || UNITY_ANDROID

        if (TipoMovimento == TipoMovimentoHorizontal.Acelerometro)
        {
            velocidadeHorizontal = Input.acceleration.x * velocidadeRolamento;
        }
        else
        {
            //Detectando se clique com o touch
            if (Input.touchCount > 0)
            {
                //Obtendo o primeiro touch
                Touch toque = Input.touches[0];
                velocidadeHorizontal = CalculaMovimento(toque.position);

                SwipeTeleporte(toque);

                TocarObjeto(toque.position);
            }
        }
//#endif

        var forcaMovimento = new Vector3(velocidadeHorizontal, 0, velocidadeRolamento);

        //Time.deltaTime retorna o tempo gasto no frame anterior
        //Assim sabemos o quanto nosso jogo está atrasado. Isso permite um game loop suave
        forcaMovimento *= (Time.deltaTime * 60);

        rb.AddForce(forcaMovimento);
    }

    /// <summary>
    /// Metodo para calcular a velocidade 
    /// </summary>
    /// <param name="posScreenSpace">As coordenadas no Screen Space (pixel)</param>
    /// <returns></returns>
    private float CalculaMovimento(Vector2 posScreenSpace) {

        var pos = Camera.main.ScreenToViewportPoint(posScreenSpace);
        float direcaoX;

        if (pos.x < 0.5)
            direcaoX = -1;
        else 
            direcaoX = +1;

        return direcaoX * velocidadeEsquiva;

    }

    private void SwipeTeleporte(Touch toque)
    {
        if( toque.phase == TouchPhase.Began)
        {
            touchInicio = toque.position;
        }else if (toque.phase == TouchPhase.Ended)
        {
            Vector2 touchFinal = toque.position;
            Vector3 direcaoMove = Vector3.zero;

            float difX = touchFinal.x - touchInicio.x;
            if (Mathf.Abs(difX) < miniDisSwipe)
            {
                return; 
            }

            if (difX < 0)
            {
                direcaoMove = Vector3.left;
            }
            else
            {
                direcaoMove = Vector3.right;
            }

               
            //Mas antes de executarmos o swipe, precisamos verifica se a bola nao ira colidir com algum obstaculo
            //Fazemos isso usando o Raycast
            RaycastHit hit;

            if (!rb.SweepTest(direcaoMove, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (direcaoMove * swipeMove));
            }

        }
    }

    /// <summary>
    /// Metodo para identificar se objetos foram tocados 
    /// </summary>
    /// <param name="pos">A poscicao tocada/clicada nada tela</param>
    private static void TocarObjeto(Vector2 pos)
    {
        //convert posicao tela em ray
        Ray posicaoTelaRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(posicaoTelaRay, out hit))
        {
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
            print(hit.transform.name);
        }
    }

}
