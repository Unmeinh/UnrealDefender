using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HumanSpawner : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime30 = new(30f);
    public Transform[] spawnPoints;

    public HumanProb[] humanProbs;
    private List<GameObject> probList = new();
    public List<HumanWave> humanWaves = new();
    // public int maxHumanPerWave = 15;
    // private int humanCounter = 0;
    // int currentWaveIndex = 0; // 0 → 4
    public int currentWave = 0;
    private int currentSpawned = 0;
    private int totalInWave = 0;
    private bool isStopSpawning = false;
    private float spawnRate = 0;

    public Slider progressBar;

    private void Start()
    {
        GetHumanWave();
        GetHumanList();
        // InvokeRepeating(nameof(SpawnHuman), 2, 5);
        
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (!isStopSpawning && currentSpawned == totalInWave)
        {
            isStopSpawning = true;
            Invoke(nameof(IncreaseWave), 20);
        }
    }

    void IncreaseWave()
    {
        if (currentWave >= 4)
            return;
        currentWave++;
        GetHumanWave();
        GetHumanList();
    }

    void GetHumanWave()
    {
        if (humanWaves.Count > 0)
        {
            HumanWave humanWave = humanWaves.Find(wave => wave.index == currentWave);
            currentSpawned = 0;
            isStopSpawning = false;
            probList.Clear();
            totalInWave = humanWave.maxHumanPerWave;
            spawnRate = humanWave.spawnRate;
        }
    }

    void GetHumanList()
    {
        for (int i = 0; i < totalInWave; i++)
        {
            GameObject random = GetRandomHuman();
            if (probList.Count == totalInWave)
                return;
            if (random)
                probList.Add(random);
        }
    }

    IEnumerator SpawnLoop()
    {
        yield return _waitForSecondsRealtime30;
        while (true)
        {
            SpawnHuman();

            float delay = spawnRate;

            yield return new WaitForSeconds(delay);
        }
    }

    void SpawnHuman()
    {
        if (isStopSpawning)
            return;
        int r = Random.Range(0, spawnPoints.Length);
        GameObject rHuman = probList[Random.Range(0, probList.Count)];
        GameObject newHuman = Instantiate(rHuman, spawnPoints[r].position, Quaternion.identity);
        currentSpawned++;
        float waveProgress = (float)currentSpawned / totalInWave;
        float percentPerWave = 1f / 5;
        float value = currentWave * percentPerWave
                    + waveProgress * percentPerWave;

        progressBar.value = value;
        // progressBar.value = currentWave * 20f + waveProgress * 20f;
    }

    public GameObject GetRandomHuman()
    {
        float total = 0f;

        foreach (var hp in humanProbs)
        {
            total += hp.probability;
        }

        float roll = Random.value * total;

        float current = 0f;

        foreach (var hp in humanProbs)
        {
            current += hp.probability;

            if (roll < current)
            {
                return hp.human;
            }
        }

        return null;
    }

}

[System.Serializable]
public class HumanProb
{
    public GameObject human;
    public float probability;
}

[System.Serializable]
public class HumanWave
{
    public int index;
    public int maxHumanPerWave;
    public float spawnRate;
}