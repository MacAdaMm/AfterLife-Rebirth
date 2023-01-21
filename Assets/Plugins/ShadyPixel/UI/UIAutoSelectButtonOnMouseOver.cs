using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ShadyPixel.UI
{
    public class UIAutoSelectButtonOnMouseOver : MonoBehaviour, IPointerEnterHandler
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.Select();
        }
    }
}
