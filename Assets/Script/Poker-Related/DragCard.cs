using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragCard : MonoBehaviour
{
    private RaycastHit2D hit;

    public void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.GetRayIntersection(ray);
                if (hit.transform == this.transform)
                {
                    GetComponent<ClickPoker>().Click();
                }
            }
        }
        
    }
}
