using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadyPixel.UI
{
    public class UIMenu : MonoBehaviour
    {
        [Space]
        [SerializeField] protected GameObject _defaultSelected;

        public bool IsVisable => gameObject.activeInHierarchy;

        protected GameObject _previousSelected;

        private void OnEnable()
        {
            Show();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            _previousSelected = EventSystem.current.currentSelectedGameObject;

            if (EventSystem.current && _defaultSelected)
            {
                EventSystem.current.SetSelectedGameObject(_defaultSelected);
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_previousSelected);
            _previousSelected = null;
        }
    }
}