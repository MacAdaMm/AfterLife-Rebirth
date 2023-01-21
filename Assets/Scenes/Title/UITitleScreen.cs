using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Afterlife.Core;
using ShadyPixel.UI;

namespace Afterlife.UI
{
    public class UITitleScreen : UIMenu
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _quitButton;

        [SerializeField] private UIModalWindow _modalWindow;

        private bool _doesSaveDataExist;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonPressed);
            _continueButton.onClick.AddListener(OnContinueButtonPressed);
            _quitButton.onClick.AddListener(OnQuitButtonPressed);

            _doesSaveDataExist = GameManager.SaveDataExists();
            if (_doesSaveDataExist)
            {
                _defaultSelected = _continueButton.gameObject;
            }
            else
            {
                _continueButton.gameObject.SetActive(false);
                _defaultSelected = _newGameButton.gameObject;
            }
        }

        private void OnDestroy()
        {
            _newGameButton.onClick.RemoveListener(OnNewGameButtonPressed);
            _continueButton.onClick.RemoveListener(OnContinueButtonPressed);
            _quitButton.onClick.RemoveListener(OnQuitButtonPressed);
        }

        private void OnNewGameButtonPressed()
        {
            if (_doesSaveDataExist)
            {
                _modalWindow.Show(
                    title: "New Game",
                    message: "Are you sure you want to start a new game?\nCurrent save data will be deleted.",
                    confirmText: "Yes",
                    confirmAction: NewGameConfirm,
                    cancelText: "No");
            }
            else
            {
                NewGameConfirm();
            }
        }

        private void NewGameConfirm()
        {
            GameManager.Instance.LoadNewGame();
        }

        private void OnContinueButtonPressed()
        {
            GameManager.Instance.LoadLastCheckpoint();
        }

        private void OnQuitButtonPressed() 
        {
            _modalWindow.Show(
                title: "Quit Game", 
                message: "Are you sure you want to quit?", 
                confirmText: "Yes", 
                confirmAction: OnQuitConfirm, 
                cancelText: "No");
        }

        private void OnQuitConfirm()
        {
            GameManager.Instance.QuitApplication();
        }
    }
}