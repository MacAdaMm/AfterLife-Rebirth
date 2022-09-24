using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadyPixel.UI
{
	public class UIAutoSetCanvasWorldCamera : MonoBehaviour
	{
		private Canvas _canvas;

		protected void Awake()
		{
			_canvas = GetComponent<Canvas>();
			SceneManager.sceneLoaded += OnSceneLoaded;
			UpdateWorldCamera();
		}

		protected void OnEnable()
		{
			UpdateWorldCamera();
		}

		protected void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		protected void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if(loadSceneMode == LoadSceneMode.Additive) { return; }
			
			UpdateWorldCamera();
		}
    
		protected void UpdateWorldCamera()
		{
			_canvas.worldCamera = Camera.main;
		}
	}
}