using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Level Data")]
    public int nextLevelIndex = -1;

    public string ambience = "none";

    [Header("Autoscroller Logic")]
    public bool chaser = false;
    public Transform chaserStart;
    public Transform chaserEnd;
    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public GameObject cameraChild;

    [Header("Food Logic")]
    public int foodRequirement = 100;
    public float currentFood = 0;
    public float totalFoodInLevel = 0;

    public float foodPercent;

    public List<Food> foods = new List<Food>();

    bool enoughFood = false;

    [Header("Camera Settings")]
    public Camera levelCamera;
    public Camera editingCamera;
    public float cameraSize = 5;

    [Header("Canvas Settings")]
    public Canvas levelCanvas;

    public Slider progressBar;
    public TMP_Text percentText;

    public GameObject failUI;

    public bool useFoodBar = true;


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameManager.ChangeGameState(GameState.Play);
        GameManager.instance.ListenFailure(FailureMet);

        if (editingCamera != null)
            editingCamera.gameObject.SetActive(false);

        levelCamera = GameManager.instance.mainCamera;
        SetupCamera();
        levelCanvas.worldCamera = levelCamera;
        //GameManager.instance.failUI = failUI;

        if(ambience != "none")
        {
            AudioController.PlayAudio(ambience);
        }

        if (nextLevelIndex <= 0)
        {
            Debug.LogWarning("Invalid index: " + nextLevelIndex);
        }
        if (!useFoodBar)
        {
            progressBar.gameObject.SetActive(false);
            percentText.gameObject.SetActive(false);

        }

        if(chaser)
        {
            if(chaserStart != null && chaserEnd != null)
            {
                levelCamera.transform.position = chaserStart.position;

                // Keep a note of the time the movement started.
                startTime = Time.time;

                // Calculate the journey length.
                journeyLength = Vector3.Distance(chaserStart.position, chaserEnd.position);
            }
            else
            {
                Debug.LogError("ChaserStart and/or ChaserEnd: undefined.");
            }
        }

    }

    public void Update()
    {
        /*if(!chaser && levelCamera.transform.position != Vector3.zero)
        {
            levelCamera.transform.position = new Vector3(0,0,-10);
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.gameState == GameState.Play)
            {
                GameManager.ChangeGameState(GameState.Pause);
            }
            else if (GameManager.instance.gameState == GameState.Pause)
            {
                GameManager.ChangeGameState(GameState.Play);
            }
        }

        if (currentFood >= foodRequirement)
        {
            enoughFood = true;
        }
        float f = 0;
        foreach (Food item in foods)
        {
            f += item.eaten;
        }
        currentFood = f;
        foodPercent = Mathf.Clamp01(currentFood / foodRequirement);
        progressBar.value = foodPercent;
        percentText.text = (int)(foodPercent * 100) + "%";

        if (chaser)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            levelCamera.transform.position = Vector3.Lerp(chaserStart.position, chaserEnd.position, fractionOfJourney);
            cameraChild.transform.position = levelCamera.transform.position;
        }
    }

    public void SetupCamera()
    {
        levelCamera.transform.position = new Vector3(0, 0, -10);
        levelCamera.orthographicSize = cameraSize;
    }

    public void AddFood(float food)
    {
        currentFood += food;
    }

    public bool WinConditionMet()
    {
        if (enoughFood)
            return true;
        return false;
    }

    public void FailureMet()
    {
        progressBar.gameObject.SetActive(false);
        percentText.gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        if (WinConditionMet() && nextLevelIndex > 0)
        {
            GameManager.instance.LoadSceneTransition(nextLevelIndex);
        }
        else
        {
            if(!WinConditionMet())
            {
                Debug.LogError("Win condition not met.");
            }
            if (nextLevelIndex  <= 0)
            {
                Debug.LogError("Invalid index: " + nextLevelIndex);
            }
        }
    }

    public void AddToList(Food food)
    {
        if(!foods.Contains(food))
        {
            foods.Add(food);
            totalFoodInLevel += food.foodCount;
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        editingCamera.orthographicSize = cameraSize;

        if(chaser)
        {
            editingCamera.transform.position = chaserStart.position;
            cameraChild.transform.position = chaserStart.position;
        }
    }
}
