using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkcupGames
{
    public class BounceOnClick : MonoBehaviour
    {
        private Button button;
        private Vector2 localScale;

        private void Awake()
        {
            button = GetComponent<Button>();
            if(button != null) button.onClick.AddListener(Bounce);
            localScale = transform.localScale;
        }

        public void Bounce()
        {
            transform.localScale = localScale;
            EasyEffect.Bounce(gameObject, 0.1f, strength: 0.2f);
        }
    }
}