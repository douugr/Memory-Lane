using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine;


public class GameManager : MonoBehaviour {

    public const int cenarioLinhas = 4;
    [Header("Número de Linhas: 4")]
    public int cenarioColunas;
    public float offsetX;
    public float offsetY;
    [SerializeField]
    private Card primeiroCard;
    [SerializeField]
    private Sprite[] imagens;
    [SerializeField]
    private AudioSource[] sons;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private Text pontuacaoFinal;
    [SerializeField]
    private Text nomeJogador;

    private int combinacoes = 0;

    private void Start()
    {
        nomeJogador.text = "Jogador: " + PlayerPrefs.GetString("Nome");
        winScreen.SetActive(false);
        Vector3 posInicial = primeiroCard.transform.position;
        List<int> list = new List<int>();

        for (int i = 0; i < (cenarioLinhas * cenarioColunas)/2; i++)
        {
            list.Add(i);
            list.Add(i);
        }

        int[] numeros = list.ToArray();
        numeros = EmbaralhaArray(numeros);

        for (int i = 0; i < cenarioColunas; i++)
        {
            for (int a = 0; a < cenarioLinhas; a ++)
            {
                Card card;
                if(i==0 && a==0)
                {
                    card = primeiroCard;
                }
                else
                {
                    card = Instantiate(primeiroCard) as Card;
                }
                int index = a * cenarioColunas + i;
                int id = numeros[index];
                card.MudaSprite(id, imagens[id]);

                float posX = (offsetX * i) + posInicial.x;
                float posY = (offsetY * a) + posInicial.y;
                card.transform.position = new Vector3(posX, posY, posInicial.z);
                StartCoroutine(MostraCards(card));


            }
        }
    }

    IEnumerator MostraCards(Card card){
        card.revelaCard(card.cardTras);
        yield return new WaitForSeconds(2f);
        card.voltaTras();
    }

    private int[] EmbaralhaArray(int[] numeros)
    {
        int[] novoArray = numeros.Clone() as int[];
        for (int i = 0; i < novoArray.Length; i++)
        {
            int tmp = novoArray[i];
            int r = Random.Range(i, novoArray.Length);
            novoArray[i] = novoArray[r];
            novoArray[r] = tmp;

        }
        return novoArray;
    }


    private Card _primeiroRevelado;
    private Card _segundoRevelado;

    private int _pontuacao = 0;
    [SerializeField]
    public Text pontosLabel;

    public bool podeRevelar
    {
        get { return _segundoRevelado == null; }
    }

    public void CardRevelado(Card card)
    {
        if(_primeiroRevelado == null)
        {
            _primeiroRevelado = card;
        }
        else
        {
            _segundoRevelado = card;
            StartCoroutine(ChecaCombinacao());
        }
    }

    private IEnumerator ChecaCombinacao()
    {
        if(_primeiroRevelado.id == _segundoRevelado.id)
        {
            _pontuacao += 2;
            combinacoes++;

            pontosLabel.text = _pontuacao.ToString();
            sons[0].Play();

            if (combinacoes == ((cenarioLinhas * cenarioColunas) / 2))
            {
                winScreen.SetActive(true);
                pontuacaoFinal.text = "Sua pontuação foi: \n" + _pontuacao + " pontos";
                print("Voce venceu!");
            }
        }
        else
        {
            sons[1].Play();
            _pontuacao -= 1;

            if (_pontuacao < 0)
            {
                _pontuacao = 0;
            }
            pontosLabel.text = _pontuacao.ToString();
            yield return new WaitForSeconds(0.5f);
            _primeiroRevelado.voltaTras();
            _segundoRevelado.voltaTras();
           
        }
        _primeiroRevelado = null;
        _segundoRevelado = null;
    }


    public void ProximaFase()
    {
        PostRequest();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void VoltaMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    public void PostRequest()
    {
        string jogador = PlayerPrefs.GetString("Nome");
        WWWForm form = new WWWForm();
        WWWForm form2 = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        headers.Add("content-type", "application/json");
        string fase = (SceneManager.GetActiveScene().buildIndex - 1).ToString();
        var bodyData = "{ \"jogador\":\"" + jogador+ "\",\"pontuacao\":\"" + _pontuacao.ToString()+ "\",\"fase\":\"" + fase+ "\"}";
        var postData = System.Text.Encoding.UTF8.GetBytes(bodyData);
        WWW request = new WWW("https://us-central1-huddle-team.cloudfunctions.net/api/memory/douugr@gmail.com", postData, headers);
        StartCoroutine(OnResponse(request));
    }

    IEnumerator OnResponse(WWW req)
    {
        yield return req;
    }


}
