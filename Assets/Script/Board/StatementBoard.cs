using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatementBoard : MonoBehaviour
{
    public static StatementBoard Instance { get; private set; }
    public GameObject Title;
    public GameObject Descr;
    public GameObject DetectingBlock; //”√”⁄ºÏ≤‚ «∑Ò≥¨≥ˆ∆¡ƒª”“±ﬂ‘µ

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowBoard(Statement s)
    {
        gameObject.SetActive(true);
        Title.GetComponent<TextMeshProUGUI>().text = s.statementName;
        Descr.GetComponent<TextMeshProUGUI>().text = s.description;
    }
    public void HideBoard()
    {
        gameObject.SetActive(false);
    }

    
}
