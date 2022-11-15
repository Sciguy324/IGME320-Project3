using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Enemy> enemies;
    //Used to store unactive exp
    private List<GameObject> expObjectMagazine;
    public GameObject expPrefeb;
    public GameObject levelUpScreen;

    private static GameManager instance;

    //spawn related
    public SpawnManager spawnManager;
    public float spawnTime = 5f;
    public int nextSpawnCount =3;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        expObjectMagazine = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEXP(Transform transform, int value)
    {
        GameObject expObject;
        if (expObjectMagazine.Count != 0)
        {
            expObject = expObjectMagazine[0];
            expObject.SetActive(true);
            expObjectMagazine.Remove(expObject);
        }
        else
        {
            expObject = Instantiate(expPrefeb, transform.position, transform.rotation);
        }
        expObject.GetComponent<ExpScript>().value = value;
    }
    public void ReturnEXP(GameObject exp)
    {
        exp.SetActive(false);
        expObjectMagazine.Add(exp);
    }
    public void LevelUp()
    {
        levelUpScreen.SetActive(true);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnTime);
        if(spawnTime>1.5)
        {
            spawnTime += -0.1f;
        }
        spawnManager.SpawnEnemiesCount(nextSpawnCount);
        if (nextSpawnCount < 20)
        {
            nextSpawnCount++;
        }
        StartCoroutine(SpawnTimer());
    }
}
