using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class DropCard : MonoBehaviour
{
    private Vector3 destin;

    void Start()
    {
         destin = transform.position;   //此脚本挂载于弃牌堆物体，所以目的地为自己
    }

    public void StartDropCards(List<GameObject> cardList)
    {
        StartCoroutine(DropCards(cardList));
    }

    IEnumerator DropCards(List<GameObject> cardList)   //动画版弃牌
    {
        
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject poker = cardList[i];
            if (poker == null) continue;
            Vector3 start = poker.transform.position;
            Vector3 dest = destin;

            float duration = 0.2f;
            float elapsed = 0f;
            // 移动卡牌到目标位置
            // 假设动画持续时间为 0.5 秒
            SEManager.Instance.MovingCard();
            while (elapsed < duration)
            {
                poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 确保卡牌位置设置为目标位置
            poker.transform.position = dest;
            poker.transform.SetParent(transform);
            poker.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<DiscardPile>().AddToList(poker);
        }
        OptionalBox.Instance.CancelAll();
        this.GetComponent<DiscardPile>().SortCard();
    }
}
