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
         destin = transform.position;   //�˽ű����������ƶ����壬����Ŀ�ĵ�Ϊ�Լ�
    }

    public void StartDropCards(List<GameObject> cardList)
    {
        StartCoroutine(DropCards(cardList));
    }

    IEnumerator DropCards(List<GameObject> cardList)   //����������
    {
        
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject poker = cardList[i];
            if (poker == null) continue;
            Vector3 start = poker.transform.position;
            Vector3 dest = destin;

            float duration = 0.2f;
            float elapsed = 0f;
            // �ƶ����Ƶ�Ŀ��λ��
            // ���趯������ʱ��Ϊ 0.5 ��
            SEManager.Instance.MovingCard();
            while (elapsed < duration)
            {
                poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // ȷ������λ������ΪĿ��λ��
            poker.transform.position = dest;
            poker.transform.SetParent(transform);
            poker.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<DiscardPile>().AddToList(poker);
        }
        OptionalBox.Instance.CancelAll();
        this.GetComponent<DiscardPile>().SortCard();
    }
}
