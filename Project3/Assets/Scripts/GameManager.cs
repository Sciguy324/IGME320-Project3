using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;


public class GameManager : MonoBehaviour
{
    public List<Enemy> enemies;
    //Used to store unactive exp
    private List<GameObject> expObjectMagazine;
    public GameObject expPrefeb;
    public GameObject levelUpScreen;

    public List<GameObject> buttonList; // List of all the button prefabs for the level up screen

    private static GameManager instance;

    public GameObject option1; // Upgrade panel option 1
    public GameObject option2; // Upgrade panel option 2
    public GameObject option3; // Upgrade panel option 3

    //spawn related
    public SpawnManager spawnManager;
    public float spawnTime = 8f;
    public int nextSpawnCount =3;
    //every spawn, up this by 1, when it gets to 3, add 1 additional enemy to spawn
    int upTickEnemySpawnCount = 0;
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
    public void SpawnEXP(Vector3 position, int value)
    {
        GameObject expObject;
        if (expObjectMagazine.Count != 0)
        {
            expObject = expObjectMagazine[0];
            expObject.transform.position = position;
            expObject.SetActive(true);
            expObjectMagazine.Remove(expObject);
        }
        else
        {
            expObject = Instantiate(expPrefeb, position, Quaternion.identity);
        }
        expObject.GetComponent<ExpScript>().value = value;
        spawnManager.currentEnemyCount--;
    }
    public void ReturnEXP(GameObject exp)
    {
        exp.SetActive(false);
        expObjectMagazine.Add(exp);
    }
    public void LevelUp()
    {
        // Randomly choose three different numbers (buttons from the List)
        List<int> randList = new List<int>(); // Will hold the three random numbers
        int counter = 0;
        do
        {
            int randomNum = Random.Range(0, buttonList.Count - 1); // Random number between 0 and the last index of the buttonList
            if(!randList.Contains(randomNum)) // If it isn't already chosen
            {
                randList.Add(randomNum); // Add it to the list of random numbers
                counter++; // Adjust counter accordingly
            }
        } while (counter != 3);

        // Change the appropiate stuff for option1
        GameObject option1Text = option1.transform.GetChild(0).gameObject;
        GameObject option1Button = option1.transform.GetChild(1).gameObject;
        option1Text.GetComponent<TextMeshProUGUI>().text = buttonList[randList[0]].name;
        option1Button = buttonList[randList[0]];

        // Change the appropiate stuff for option2
        GameObject option2Text = option2.transform.GetChild(0).gameObject;
        GameObject option2Button = option2.transform.GetChild(1).gameObject;
        option2Text.GetComponent<TextMeshProUGUI>().text = buttonList[randList[1]].name;
        option2Button = buttonList[randList[1]];

        // Change the appropiate stuff for option1
        GameObject option3Text = option3.transform.GetChild(0).gameObject;
        GameObject option3Button = option3.transform.GetChild(1).gameObject;
        option3Text.GetComponent<TextMeshProUGUI>().text = buttonList[randList[2]].name;
        option3Button = buttonList[randList[2]];

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
            spawnTime += -0.05f;
        }
        spawnManager.SpawnEnemiesCount(nextSpawnCount);
        if (nextSpawnCount < 20 && upTickEnemySpawnCount==2)
        {
            upTickEnemySpawnCount = 0;
            nextSpawnCount++;
        }
        upTickEnemySpawnCount++;
        StartCoroutine(SpawnTimer());
    }
}
