using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;


    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;
	public string startingSceneName = "StageSelect";
	//public string initialStartingPositionName = "DoorToMarket";
	//public SaveData playerSaveData;
    
	public static SceneController Instance;
    
    private bool isFading;

	StageManager stageManager;

	IEnumerator Start ()
    {
		if (Instance == null) {
			Instance = this;
		}

		stageManager = GetComponent<StageManager> ();

		faderCanvasGroup.alpha = 1f;

		//playerSaveData.Save (PlayerMovement.startingPositionKey, initialStartingPositionName);

		yield return StartCoroutine (LoadSceneAndSetActive (startingSceneName));

		StartCoroutine (Fade (0f));
    }

	public void GoToStage(int stageNumber){
		if (!isFading)
		{
			AfterSceneLoad = stageManager.StageInit;
			StartCoroutine (FadeAndSwitchScenes ("StageMain" + stageNumber.ToString("D2")));
		}
	}

	public void TurnEnd(){
		if (!isFading)
		{
			AfterSceneLoad = stageManager.TurnInit;
			string activeSceneName = SceneManager.GetActiveScene ().name;
			StartCoroutine (FadeAndSwitchScenes (activeSceneName));
		}
	}

    public void FadeAndLoadScene (SceneReaction sceneReaction)
    {
        if (!isFading)
        {
            StartCoroutine (FadeAndSwitchScenes (sceneReaction.sceneName));
        }
    }


	public void FadeAndLoadScene (string sceneName)
	{
		if (!isFading)
		{
			StartCoroutine (FadeAndSwitchScenes (sceneName));
		}
	}

    private IEnumerator FadeAndSwitchScenes (string sceneName)
    {
        yield return StartCoroutine (Fade (1f));

        if (BeforeSceneUnload != null)
            BeforeSceneUnload ();

        yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);

        yield return StartCoroutine (LoadSceneAndSetActive (sceneName));

        if (AfterSceneLoad != null)
            AfterSceneLoad ();
        
        yield return StartCoroutine (Fade (0f));
    }


    private IEnumerator LoadSceneAndSetActive (string sceneName)
    {
        yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene (newlyLoadedScene);
    }


    private IEnumerator Fade (float finalAlpha)
    {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs (faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately (faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }
}
