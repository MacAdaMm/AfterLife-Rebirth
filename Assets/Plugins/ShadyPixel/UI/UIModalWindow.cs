using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace ShadyPixel.UI 
{
    public class UIModalWindow : UIMenu
    {
        [Header("Root Objects")]
        [SerializeField] private Transform _overlay;
        [SerializeField] private Transform _windowRoot;

        [Header("Title")]
        [SerializeField] private TextMeshProUGUI _titleLabel;

        [Header("Message")]
        [SerializeField] private TextMeshProUGUI _messageLabel;

        [Header("Confirm Button")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private TextMeshProUGUI _confirmLabel;

        private UnityAction _confirmAction;

        [Header("Cancel Button")]
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TextMeshProUGUI _cancelLabel;

        private UnityAction _cancelAction;

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        }

        public void Show(string title, string message, string confirmText, UnityAction confirmAction, string cancelText = "", UnityAction cancelAction = null)
        {
            gameObject.SetActive(true);
            _overlay.gameObject.SetActive(true);

            _titleLabel.SetText(title);

            _messageLabel.SetText(message);

            _confirmLabel.SetText(confirmText);

            _confirmAction = confirmAction;
            _confirmButton.onClick.AddListener(_confirmAction);
            _confirmButton.onClick.AddListener(Hide);

            if(string.IsNullOrWhiteSpace(cancelText))
            {
                _cancelButton.gameObject.SetActive(false);
            }
            else
            {
                _cancelLabel.SetText(cancelText);
                _cancelButton.gameObject.SetActive(true);
                _cancelButton.onClick.AddListener(Hide);
                if (cancelAction != null)
                {
                    _cancelAction = cancelAction;
                    _cancelButton.onClick.AddListener(_cancelAction);
                }
                
            }

            _windowRoot.gameObject.SetActive(true);
        }
    }
}