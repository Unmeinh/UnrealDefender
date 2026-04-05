using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject currentDefender;
    public Sprite currentDefenderSprite;
    public Transform tiles;
    public TextMeshProUGUI myMoney;

    public LayerMask tileMask;
    public LayerMask goldMask;
    private AudioSource audioSource;
    public AudioSource goldAudioSource;
    public AudioClip plantSFX;
    public AudioClip goldSFX;

    public int golds;

    private CardSlot cardSlot;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        golds = 0;
    }

    private void Update()
    {
        myMoney.text = golds.ToString();

        // Tile set
        Vector2 mousePos = Mouse.current.position.ReadValue();
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero, Mathf.Infinity, tileMask);

        foreach (Transform tile in tiles)
        {
            tile.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (hit.collider && currentDefender)
        {
            hit.collider.GetComponent<SpriteRenderer>().sprite = currentDefenderSprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;
        }


        if (Mouse.current.leftButton.wasPressedThisFrame && currentDefender && hit.collider && !hit.collider.GetComponent<Tile>().hasDefender)
        {
            Planting(hit);
        }

        // Gold set
        RaycastHit2D goldHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero, Mathf.Infinity, goldMask);

        if (goldHit.collider)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Destroy(goldHit.collider.gameObject);
                IncreaseGold();
            }
        }
    }

    public void IncreaseGold()
    {
        goldAudioSource.PlayOneShot(goldSFX);
        golds += 25;
    }

    public void BuyDefender(GameObject defender, Sprite sprite, CardSlot card)
    {
        currentDefender = defender;
        currentDefenderSprite = sprite;
        cardSlot = card;
    }

    private void Planting(RaycastHit2D hit)
    {
        audioSource.PlayOneShot(plantSFX);
        Instantiate(currentDefender, hit.collider.transform.position, Quaternion.identity);
        cardSlot.SetCardCD();
        hit.collider.GetComponent<Tile>().hasDefender = true;
        golds -= cardSlot.price;
        currentDefender = null;
        currentDefenderSprite = null;
        cardSlot = null;
    }
}
