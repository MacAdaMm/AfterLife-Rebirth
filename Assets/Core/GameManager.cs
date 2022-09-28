using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;

[RequireComponent(typeof(SaveHandler))]
[DefaultExecutionOrder(int.MinValue)]
public class GameManager : MonoBehaviour, ISaveable
{
	[System.Serializable]
	public class SerializableVector2
    {
		public float X;
		public float Y;

		public SerializableVector2(Transform transform)
        {
			X = transform.position.x;
			Y = transform.position.y;
        }
    }

	[System.Serializable]
	public class SaveData
    {
		public string CheckPointSceneName = "SandBox";
		public string CheckPointId = null;
    }
	
	[SerializeField]
	private string _levelEntryPointId;
	public static GameManager Instance { get; private set; }

	private static SaveData _saveData = new SaveData();

	[SerializeField] private CanvasGroup _screenFadeCanvasGroup;
	
	private bool _isLoadingScene;

	public void LoadScene(string sceneName, float fadeDuration = 0.5f, float delayBeforeFadeOut = 0.25f, float fadeOutDuration = 0.5f)
	{
		if (_isLoadingScene)
		{
			return;
		}

		_screenFadeCanvasGroup.DOComplete();
		_screenFadeCanvasGroup.alpha = 0f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_isLoadingScene = true;

        

		_screenFadeCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += () =>
		{
			//SaveManager.Save();
			SceneManager.LoadScene(sceneName);

			_isLoadingScene = false;
			_screenFadeCanvasGroup.DOFade(0f, fadeOutDuration).SetUpdate(true).SetDelay(delayBeforeFadeOut).SetEase(Ease.InOutSine).onComplete += () =>
			{
				_screenFadeCanvasGroup.gameObject.SetActive(false);
			};
		};
	}
	public void LoadScene(string scenename, string levelEntryPointId, float fadeDuration = 0.5f, float delayBeforeFadeOut = 0.25f, float fadeOutDuration = 0.5f)
    {
		_levelEntryPointId = levelEntryPointId;
		LoadScene(scenename, fadeDuration, delayBeforeFadeOut, fadeOutDuration);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode mode)
    {
		if(mode == LoadSceneMode.Additive)
        {
			return;
        }
		SaveManager.Load();
		LevelManager.Current?.SetEntryPoint(_levelEntryPointId);
		_levelEntryPointId = null;

	}

	public void QuitApplication()
	{

		_screenFadeCanvasGroup.alpha = 0f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_screenFadeCanvasGroup.DOFade(1f, 0.25f).SetEase(Ease.InOutSine).onComplete += () =>
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			Application.Quit();
		};
	}

	protected void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}
		else
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

    private void OnDestroy()
    {
        if (Instance == this)
        {
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
			Instance = null;
		}
    }

    protected void Start()
	{
		_screenFadeCanvasGroup.alpha = 1f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_screenFadeCanvasGroup.DOFade(0f, 1f).SetDelay(0.15f).SetEase(Ease.InOutSine).onComplete += () =>
		{
			_screenFadeCanvasGroup.gameObject.SetActive(false);
		};
	}

	public void SetCheckpoint(SavePoint savePoint)
	{
		_saveData.CheckPointId = savePoint.GetComponent<LevelEntry>().Id;
		_saveData.CheckPointSceneName = SceneManager.GetActiveScene().name;
	}

	[MenuItem("Utility/Save Game")]
	private static void EditorSave()
    {
		SaveManager.Save();
    }
	[MenuItem("Utility/Load Game")]
	private static void EditorLoad()
    {
		SaveManager.Load();
    }
	[MenuItem("Utility/Delete Save")]
	private static void EditorDelete()
	{
		SaveManager.Delete();
	}

	[MenuItem("Utility/Reload Scene")]
	private static void EditorReloadScene()
	{
		Instance.LoadScene(SceneManager.GetActiveScene().name);
	}
	[MenuItem("Utility/Reload From Checkpoint")]
	private static void EditorReloadFromCheckPoint()
    {
		Instance.LoadScene(_saveData.CheckPointSceneName, _saveData.CheckPointId);
	}
	public void ReloadFromCheckPoint()
	{
		SaveManager.Load();
		Instance.LoadScene(_saveData.CheckPointSceneName, _saveData.CheckPointId);
	}

	public object CaptureState()
    {
		return _saveData;
    }

    public void RestoreState(object state)
    {
		_saveData = (SaveData)state;
    }
}
