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
        if (sceneId == 0)    //跳转到主页面
        {
            //LoadingUI.Instance.StartFading(1);
            yield return new WaitForSeconds(1.2f);
            SceneManager.LoadScene("MainScene"); 
            //LoadingUI.Instance.StartFading(0);
        }
        else if (sceneId == 1)    //跳转到游戏场景 
        {
            if (SceneManager.GetActiveScene().name == "MainScene")   //从主界面进入游戏场景
            {
                //LoadingUI.Instance.StartFading(1);
                yield return new WaitForSeconds(1.2f);
                SceneManager.LoadScene("RegicideGameScene");
                //LoadingUI.Instance.StartFading(0);
            }
            else    //不是从主界面进入 即 关闭商店场景
            {
                itemAreaManager.StartCopyToGameScene();//把物品栏复制到游戏场景
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
        else if (sceneId == 2)   //附加商店场景
        {
            MyEventSystem.Instance.TriggerEnterShopScene();
            //LoadingUI.Instance.StartFading(1);  //渐渐变暗
            yield return new WaitForSeconds(1.2f);
            SceneManager.LoadSceneAsync("Shop", LoadSceneMode.Additive);

            Scene shopScene = SceneManager.GetSceneByName("Shop");
            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //调整附加场景位置
            {
                if (rootObject.name == "AllObjects")
                {
                    rootObject.transform.position = ShopSceneLocation.transform.position;
                    break;
                }
            }
            //LoadingUI.Instance.StartFading(0);  //渐渐变亮
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;  //关闭原场景相机
        }
    }
    public void ReLoadGameScene()   //重新加载游戏场景
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
