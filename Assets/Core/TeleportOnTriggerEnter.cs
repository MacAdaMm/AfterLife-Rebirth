using ShadyPixel.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Afterlife.Core
{
    [SelectionBase]
    public class TeleportOnTriggerEnter : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName = "Title";

        [SerializeField]
        private string _levelEntryId;

        public void LoadTargetScene()
        {
            SPInputManager.InputActions.Player.Disable();
            GameManager.Instance.LoadScene(_sceneName, _levelEntryId);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool isPlayerObject = collision.gameObject.CompareTag("Player");
            if (isPlayerObject)
            {
                LoadTargetScene();
            }
        }
    }
}
