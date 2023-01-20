using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;
using ShadyPixel.SaveLoad;
using System;

namespace Afterlife.Core
{
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
			public string CheckPointSceneName = "Sandbox";
			public string CheckPointId;
		}

		public string CurrentCheckpointId { get => _saveData.CheckPointId; }
		public string CurrentCheckpointSceneName { get => _saveData.CheckPointSceneName; }

		[SerializeField] private bool _logEvents = true;

		[SerializeField]
		[Tooltip("Should the game data be loaded on Awake(), If FALSE game data will have default values.\n" +
			"Will only work in editor mode.")]
		private bool _editorLoadGameDataOnAwake = false;

        [SerializeField]
		private CanvasGroup _screenFadeCanvasGroup;

		private static SaveData _saveData = new SaveData();
		private string _targetLevelEntryPointId;
		private bool _isLoadingScene;

		public static GameManager Instance { get; private set; }

		public static bool SaveDataExists()
		{
			return SaveManager.FileExists();
		}

		public static void Save()
        {
			SaveManager.Save();
			//Debug.Log("Game data saved.", this);
        }

		public static void LoadData()
		{
			SaveManager.Load();
			//Debug.Log("Game data loaded.", this);
		}

		public static void SetCheckpoint(SavePoint savePoint)
		{
			_saveData.CheckPointId = savePoint.GetComponent<LevelEntryPoint>().Id;
			_saveData.CheckPointSceneName = SceneManager.GetActiveScene().name;
			//Debug.Log("Checkpoint Set", savePoint);
		}

		public void LoadNewGame()
		{
			DeleteSaveFile();
			Instance.LoadScene("Sandbox");
		}

		public static void DeleteSaveFile()
		{
            if (SaveDataExists())
            {
				SaveManager.DeleteSaveFile();
				_saveData = new SaveData();
				Debug.Log("Save data deleted.");
			}
		}

		public void LoadLastCheckpoint()
		{
			Instance.LoadScene(_saveData.CheckPointSceneName, _saveData.CheckPointId);
		}

		public void LoadScene(string sceneName, float fadeDuration = 0.15f, float delayBeforeFadeOut = 0.1f, float fadeOutDuration = 0.25f)
		{
			if (_isLoadingScene)
			{
				return;
			}

			Save();

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

		private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (loadSceneMode == LoadSceneMode.Additive)
			{
				return;
			}

			LevelManager.Current?.SetEntryPoint(_targetLevelEntryPointId);
			_targetLevelEntryPointId = null;
			Debug.Log($"Scene Loaded: {SceneManager.GetActiveScene().name}", this);
		}

		public void QuitApplication()
		{
			//Debug.Log("Application Quit", this);

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
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

				if (Application.isEditor && _editorLoadGameDataOnAwake == false)
				{
					Debug.Log("Skipping initial data load..", this);
					return;
				}

                if (SaveDataExists())
                {
					LoadData();
					Debug.Log("Loaded game data from file..", this);
				}
                else
                {
					Debug.Log("No game data to load..", this);
                }
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
			if (EditorUtility.DisplayDialog("Delete Save Data", "Are you sure you want to delete current save data?", "Yes", "No"))
			{
				DeleteSaveFile();
			}
		}

		[MenuItem("Utility/Reload From Checkpoint")]
		private static void EditorReloadFromCheckpoint()
		{
			Instance.LoadLastCheckpoint();
		}
#endif
	}
}