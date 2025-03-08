using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //public static LoadScene Instance;
    public GameObject ShopSceneLocation;
    public ItemAreaManager itemAreaManager;
    /*private void Awake()
    {
        Instance = this;
    }*/

    public void LoadToScene(int sceneId)
    {
        StartCoroutine(LoadSceneThread(sceneId));
    }
    IEnumerator LoadSceneThread(int sceneId)
    {
        if (sceneId == 0)    //��ת����ҳ��
        {
            //LoadingUI.Instance.StartFading(1);
            yield return new WaitForSeconds(1.2f);
            SceneManager.LoadScene("MainScene"); 
            //LoadingUI.Instance.StartFading(0);
        }
        else if (sceneId == 1)    //��ת����Ϸ���� 
        {
            if (SceneManager.GetActiveScene().name == "MainScene")   //�������������Ϸ����
            {
                //LoadingUI.Instance.StartFading(1);
                yield return new WaitForSeconds(1.2f);
                SceneManager.LoadScene("RegicideGameScene");
                //LoadingUI.Instance.StartFading(0);
            }
            else    //���Ǵ���������� �� �ر��̵곡��
            {
                itemAreaManager.StartCopyToGameScene();//����Ʒ�����Ƶ���Ϸ����
                //LoadingUI.Instance.StartFading(1);
                yield return new WaitForSeconds(1.2f);
                MyEventSystem.Instance.TriggerShopLoadOff();
                SceneManager.UnloadSceneAsync("Shop");

                Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
                foreach (GameObject rootObject in gameScene.GetRootGameObjects())
                {
                    if (rootObject.CompareTag("MainCamera"))
                    {
                        rootObject.GetComponent<Camera>().enabled = true;
                        //rootObject.GetComponent<Camera>().enabled = true;
                        break;
                    }
                }
                //LoadingUI.Instance.StartFading(0);
            }

        }
        else if (sceneId == 2)   //�����̵곡��
        {
            MyEventSystem.Instance.TriggerEnterShopScene();
            //LoadingUI.Instance.StartFading(1);  //�����䰵
            yield return new WaitForSeconds(1.2f);
            SceneManager.LoadSceneAsync("Shop", LoadSceneMode.Additive);

            Scene shopScene = SceneManager.GetSceneByName("Shop");
            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //�������ӳ���λ��
            {
                if (rootObject.name == "AllObjects")
                {
                    rootObject.transform.position = ShopSceneLocation.transform.position;
                    break;
                }
            }
            //LoadingUI.Instance.StartFading(0);  //��������
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;  //�ر�ԭ�������
        }
    }
    public void ReLoadGameScene()   //���¼�����Ϸ����
    {
        SceneManager.LoadScene("RegicideGameScene");
    }
    public void ExitGame()  
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
