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

        private bool _doesSaveDataExist;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonPressed);
            _continueButton.onClick.AddListener(OnContinueButtonPressed);
            _quitButton.onClick.AddListener(OnQuitButtonPressed);

            _doesSaveDataExist = GameManager.SaveDataExists();
            if (_doesSaveDataExist == false)
            {
                _continueButton.gameObject.SetActive(false);
                _defaultSelected = _newGameButton.gameObject;
            }
            else
            {
                _defaultSelected = _continueButton.gameObject;
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
            GameManager.Instance.LoadNewGame();
        }

        private void OnContinueButtonPressed()
        {
            GameManager.Instance.LoadLastCheckpoint();
        }

        private void OnQuitButtonPressed()
        {
            GameManager.Instance.QuitApplication();
        }
    }
}