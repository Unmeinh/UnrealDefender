using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    public GameObject goldObject;

    private void Start()
    {
        SpawnGold();    
    }

    void SpawnGold()
    {
        GameObject myGold = Instantiate(goldObject,   new Vector3(Random.Range(-8f, 8.5f), 5.5f, 0), Quaternion.identity);
        myGold.GetComponent<Gold>().dropToYPos = Random.Range(-2f, 1f);
        Invoke("SpawnGold", Random.Range(4, 10));
    }
}
