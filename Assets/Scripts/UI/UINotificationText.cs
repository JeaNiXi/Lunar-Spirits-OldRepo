using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UINotificationText : MonoBehaviour
    {
        [SerializeField] Text text;

        private const float TEXT_ANIMATION_TIME = 2.0f;
        private const float TEXT_HOLD_TIME = 2.0f;

        private IEnumerator FadeTextToFullAlpha(float aT, float hT)
        {
            while (text.color.a < 1.0f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime / aT);
                yield return null;
            }
            yield return new WaitForSeconds(hT);
            StartCoroutine(FadeTextToZeroAlpha(aT));
        }
        private IEnumerator FadeTextToZeroAlpha(float aT)
        {
            while (text.color.a > 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime / aT);
                yield return null;
            }
            DeleteNotification();
        }
        public void SetText(string newText)
        {
            text.text = newText;
        }
        public void StartDisplay()
        {
            StartCoroutine(FadeTextToFullAlpha(TEXT_ANIMATION_TIME, TEXT_HOLD_TIME));
        }
        private void DeleteNotification()
        {
            Destroy(gameObject);
        }
    }
}