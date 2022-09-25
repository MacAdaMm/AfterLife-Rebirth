using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(int.MinValue)]
public class GameManager : MonoBehaviour
{
	[SerializeField] private EventSystem _eventSystem;

	public static GameManager Instance { get; private set; }

	[SerializeField] private CanvasGroup _screenFadeCanvasGroup;
	
	private bool _isLoadingScene;

	public void LoadScene(string sceneName, float fadeDuration = 0.5f, float delayBeforeFadeOut = 0.25f, float fadeOutDuration = 0.5f)
	{
		if (_isLoadingScene)
		{
			return;
		}
		
		if(_eventSystem)
		{
			_eventSystem.sendNavigationEvents = false;
		}

		_screenFadeCanvasGroup.DOComplete();
		_screenFadeCanvasGroup.alpha = 0f;
		_screenFadeCanvasGroup.gameObject.SetActive(true);
		_isLoadingScene = true;

		_screenFadeCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += () =>
		{
			SceneManager.LoadScene(sceneName);
			if (_eventSystem)
			{
				_eventSystem.sendNavigationEvents = true;
			}

			_isLoadingScene = false;
			_screenFadeCanvasGroup.DOFade(0f, fadeOutDuration).SetUpdate(true).SetDelay(delayBeforeFadeOut).SetEase(Ease.InOutSine).onComplete += () =>
			{
				_screenFadeCanvasGroup.gameObject.SetActive(false);
			};
		};
	}

	public void ReloadCurrentScene()
	{
		LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitApplication()
	{
		if (_eventSystem)
		{
			_eventSystem.gameObject.SetActive(false);
		}

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
		}
		else
		{
			if (_eventSystem)
			{
				_eventSystem.gameObject.SetActive(false);
			}
			gameObject.SetActive(false);
			Destroy(gameObject);
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
}
