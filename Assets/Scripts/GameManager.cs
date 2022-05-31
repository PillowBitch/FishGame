using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    Start = 0,
    Play = 1,
    End = 2,
    Fail = 3,
    Cutscene = 4,
    Pause = 5,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState = GameState.Start;

    public GameObject failUI;
    public GameObject menuUI;
    public GameObject levelSelectUI;
    public GameObject pauseUI;

    public Image menuFade;

    public int currentLevelIndex;

    public int debugStartStage;

    public Camera mainCamera;

    UnityEvent failEvent;

    public Animator transition;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ChangeGameState(GameState.Start);

        if (failEvent == null)
            failEvent = new UnityEvent();

        StartCoroutine(LoadSceneAsync(1));
        currentLevelIndex = 1;
    }
   
    public void StartStage()
    {
        LoadSceneTransition(debugStartStage);
    }

    public void LevelSelect()
    {
        menuUI.SetActive(false);
        levelSelectUI.SetActive(true);

        StartCoroutine(FadeImage(false, menuFade, 0.6f));
    }

    public void BackToMain()
    {
        levelSelectUI.SetActive(false);
        menuUI.SetActive(true);

        StartCoroutine(FadeImage(true, menuFade, 0.6f));
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Resume()
    {
        ChangeGameState(GameState.Play);
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        menuFade.color = new Color(0, 0, 0, 0);
        //UnloadScene(currentLevelIndex);
        //StartCoroutine(LoadSceneAsync(1));
        LoadSceneTransition(1);

    }

    public void Failure()
    {
        ChangeGameState(GameState.Fail);
        failUI.SetActive(true);

        Image img = failUI.GetComponentInChildren<Image>();

        StartCoroutine(FadeImage(false, img, 0.6f));

        if (gameState == GameState.Play)
        {
            gameState += 3;
        } else
        {
            gameState--;
        }

        

    }

    public static void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Start:
                instance.menuUI.SetActive(true);
                instance.failUI.SetActive(false);
                instance.pauseUI.SetActive(false);
                break;
            case GameState.Play:
                instance.menuUI.SetActive(false);
                instance.failUI.SetActive(false);
                instance.levelSelectUI.SetActive(false);
                instance.pauseUI.SetActive(false);
                Time.timeScale = 1;

                instance.menuFade.color = new Color(0, 0, 0, 0);
                
                break;
            case GameState.Fail:
                instance.failEvent.Invoke();
                break;
            case GameState.End:

                break;
            case GameState.Cutscene:
                instance.menuFade.color = new Color(0, 0, 0, 0);
                instance.menuUI.SetActive(false);
                instance.failUI.SetActive(false);
                instance.levelSelectUI.SetActive(false);
                break;
            case GameState.Pause:
                instance.pauseUI.SetActive(true);
                instance.menuFade.color = new Color(0, 0, 0, 0.6f);
                Time.timeScale = 0;
                break;
            default:
                Debug.LogError("Invalid GameState given: " + gameState);
                break;
        }

        instance.gameState = gameState;
    }

    public void ListenFailure(UnityAction listener)
    {
        instance.failEvent.AddListener(listener);
    }
 
    IEnumerator FadeImage(bool fadeAway, Image img, float transparancyFactor)
    {

        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = transparancyFactor; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= transparancyFactor; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);

                yield return null;
            }

        }
        
    }

    IEnumerator FadeImage(Image img, float transparancyFactor)
    {
        // fade from transparent to opaque
        for (float i = 0; i <= transparancyFactor; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(img.color.r, img.color.g, img.color.b, i);

            yield return null;
        }
    }


        int transitionLoad;

    public void LoadSceneTransition(int sceneIndex)
    {
        transition.SetBool("Transition", true);
        transitionLoad = sceneIndex;
    }

    public void TransitionLoad()
    {
        LoadScene(transitionLoad);
        if (transitionLoad == 1)
            ChangeGameState(GameState.Start);
        transition.SetBool("Transition", false);
    }

    public void TransitionReload()
    {
        transition.SetBool("Transition", true);
        transitionLoad = currentLevelIndex;
    }

    public void LoadScene(int sceneIndex)
    {
        //if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(sceneIndex))
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            int lastScene;
            lastScene = SceneManager.GetActiveScene().buildIndex;
            UnloadScene(lastScene);
            StartCoroutine(LoadSceneAsync(sceneIndex));


            currentLevelIndex = sceneIndex;
        }
    }


    public void UnloadScene(int sceneIndex)
    {
        AudioController.StopAllAudio();
        failEvent.RemoveAllListeners();
        StartCoroutine(UnloadSceneAsync(sceneIndex));
    }

    public void ReloadScene()
    {
        UnloadScene(currentLevelIndex);
        StartCoroutine(LoadSceneAsync(currentLevelIndex));
    }

    void SceneHasLoaded(int sceneIndex)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {

        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        SceneHasLoaded(sceneIndex);
    }

    IEnumerator UnloadSceneAsync(int sceneIndex)
    {
        AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(sceneIndex, UnloadSceneOptions.None);
        while (!sceneUnload.isDone)
        {
            yield return null;
        }
    }

}
