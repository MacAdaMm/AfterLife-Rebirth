using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ShadyPixel.UI
{
    public class UIAutoSelectButtonOnMouseOver : MonoBehaviour, IPointerMoveHandler
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _button.Select();
        }
    }
}
