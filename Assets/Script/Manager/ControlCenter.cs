using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCenter : MonoBehaviour
{
    public static ControlCenter Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {     
            if (Input.GetKeyDown(KeyCode.H))
            {
                GotoShop();
            }
        }
        if(Input.GetKey(KeyCode.R)) 
        {
            LoadingUI.Instance.ForceToCloseUI();
        }
        //°´DR¿É´¥·¢ÃþÅÆ
    }
    public void GotoShop()
    {
        StartCoroutine(EnterShop());
    }
    IEnumerator EnterShop()
    {
        yield return new WaitForSeconds(1f);
        this.GetComponent<LoadScene>().LoadToScene(2);
    }
}
