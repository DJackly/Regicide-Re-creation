using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatementManager : MonoBehaviour
{
    public static StatementManager Instance;
    public GameObject ActivateArea;
    public GameObject Unactivate;
    public List<GameObject> StatementList= new List<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    public void SetStatement(int index,bool b)
    {
        if (b) StatementList[index].transform.parent = ActivateArea.transform;
        else StatementList[index].transform.parent = Unactivate.transform;
    }
    public void SetStatementCount(int index,int count)
    {
        if (StatementList[index].transform.parent != ActivateArea.transform) return;//¸Ã×´Ì¬Î´¼¤»î
        StatementList[index].GetComponent<Statement>().SetCount(count);
    }
}
