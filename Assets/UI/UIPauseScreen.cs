using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ShadyPixel.UI;
using Afterlife.Core;

namespace Afterlife.UI
{
    public class UIPauseScreen : UIMenu
    {
        [Header("Resume")]
        [SerializeField] private Button _resumeButton;

        [Header("Main Menu")]
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private UIModalWindow _modalWindow;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonPressed);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonPressed);
        }

        private void OnResumeButtonPressed()
        {
            LevelManager.Current.Unpause();
        }

        private void OnMainMenuButtonPressed()
        {
            _modalWindow.Show(
                title: "Main Menu",
                message: "Are you sure you want to main menu?\nUnsaved data will be deleted.",
                confirmText: "Yes",
                confirmAction: () => GameManager.Instance.LoadScene("Title"),
                cancelText: "No");
        }
    }
}