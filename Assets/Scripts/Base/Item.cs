using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;
        [field: SerializeField] private int quantity;

        private float animationDuration = 0.3f;

        private void Awake()
        {
            Debug.Log(itemSO.ID + " " + gameObject.name);
        }
        public ItemSO GetItem()
        {
            return itemSO;
        }
        public int GetQuantity()
        {
            return quantity;
        }
        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public void DeleteItem()
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PickupAnimation());
        }

        public IEnumerator PickupAnimation()
        {
            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.zero;
            float currentTime = 0;
            while (currentTime < animationDuration)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / animationDuration);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}