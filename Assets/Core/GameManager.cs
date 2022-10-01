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

	public string CurrentCheckpointId { get => _saveData.CheckPointId; }
	public string CurrentCheckpointSceneName { get => _saveData.CheckPointSceneName; }

	[SerializeField]
	[Tooltip("Should the game data be loaded on Awake(), If FALSE game data will have default values.\n" +
        "Will only work in editor mode.")]
	 private bool _editorLoadGameDataOnAwake = false;

	[SerializeField] 
	private CanvasGroup _screenFadeCanvasGroup;

	private static SaveData _saveData = new SaveData();
	private bool _isLoadingScene;
	private string _targetLevelEntryPointId;

	public static GameManager Instance { get; private set; }

	public void LoadScene(string sceneName, float fadeDuration = 0.15f, float delayBeforeFadeOut = 0.1f, float fadeOutDuration = 0.25f)
	{
		if (_isLoadingScene)
		{
			return;
		}

		SaveManager.Save();

		_screenFadeCanvasGroup.DOComplete();
		_screenFadeCanvasGroup.alpha = 0f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_isLoadingScene = true;

        if (EventSystem.current)
        {
			EventSystem.current.sendNavigationEvents = false;
        }

		_screenFadeCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += () =>
		{
			
			SceneManager.LoadScene(sceneName);

			_isLoadingScene = false;
			_screenFadeCanvasGroup.DOFade(0f, fadeOutDuration).SetUpdate(true).SetDelay(delayBeforeFadeOut).SetEase(Ease.InOutSine).onComplete += () =>
			{
				_screenFadeCanvasGroup.gameObject.SetActive(false);
			};
		};
	}

	public void LoadScene(string scenename, string levelEntryPointId, float fadeDuration = 0.15f, float delayBeforeFadeOut = 0.1f, float fadeOutDuration = 0.25f)
    {
		_targetLevelEntryPointId = levelEntryPointId;
		LoadScene(scenename, fadeDuration, delayBeforeFadeOut, fadeOutDuration);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode mode)
    {
		if(mode == LoadSceneMode.Additive)
        {
			return;
        }

		SaveManager.Load();
		LevelManager.Current?.SetEntryPoint(_targetLevelEntryPointId);
		_targetLevelEntryPointId = null;
	}

	public void QuitApplication()
	{
        if (EventSystem.current)
        {
			EventSystem.current.sendNavigationEvents = false;
        }

		_screenFadeCanvasGroup.alpha = 0f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_screenFadeCanvasGroup.DOFade(1f, 1f).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += () =>
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
			if (Application.isEditor && _editorLoadGameDataOnAwake == false)
			{
				return;
			}

			SaveManager.Load();
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
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;

		_screenFadeCanvasGroup.alpha = 1f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_screenFadeCanvasGroup.DOFade(0f, 1f).SetUpdate(true).SetDelay(0.15f).SetEase(Ease.InOutSine).onComplete += () =>
		{
			_screenFadeCanvasGroup.gameObject.SetActive(false);
		};
	}

	public void SetCheckpoint(SavePoint savePoint)
	{
		_saveData.CheckPointId = savePoint.GetComponent<LevelEntryPoint>().Id;
		_saveData.CheckPointSceneName = SceneManager.GetActiveScene().name;
	}

	public void LoadGame()
	{
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

#if UNITY_EDITOR
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

	[MenuItem("Utility/Reload From Checkpoint")]
	private static void EditorReloadFromCheckpoint()
	{
		Instance.LoadGame();
	}
#endif
}
