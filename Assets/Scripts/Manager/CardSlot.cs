using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class CardSlot : MonoBehaviour
{
    public Sprite defenderSprite;
    public GameObject defenderObject;
    public Image icon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textPrice;
    private GameManager gmg;
    public Image fill;
    public HumanSpawner humanSpawner;
    public float cardCD;
    public int cardWave;

    float timer;
    public int price;
    private bool isBuyable = false;
    private bool isLocking = true;

    private void Start()
    {
        gmg = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.GetComponent<Button>().onClick.AddListener(BuyDefender);
        Checking();
    }

    private void Update()
    {
        Checking();
    }

    private void Checking()
    {
        if (humanSpawner.currentWave == cardWave && isLocking)
        {
            isLocking = false;
            SetCardCD();
        }
        if (isLocking)
            return;
        if (gmg.golds < price)
            textPrice.color = new Color(255, 0, 0, 255);
        if (gmg.golds >= price)
            textPrice.color = new Color(255, 255, 255, 255);

        if (isBuyable)
            return;

        timer -= Time.deltaTime;

        fill.fillAmount = timer / cardCD;

        if (timer <= 0)
        {
            isBuyable = true;
            fill.fillAmount = 0;
        }
    }

    private void BuyDefender()
    {
        if (isBuyable && !isLocking && gmg.golds >= price)
        {
            gmg.BuyDefender(defenderObject, defenderSprite, gameObject.GetComponent<CardSlot>());
        }
    }

    public void SetCardCD()
    {
        isBuyable = false;
        timer = cardCD;
    }

    private void OnValidate()
    {
        if (defenderSprite)
        {
            icon.enabled = true;
            icon.sprite = defenderSprite;
            textPrice.text = price.ToString();
        }
        else
            icon.enabled = false;
    }
}
