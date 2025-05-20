using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public float before_load = 10f;
    public string name;


    public void LoadScene()
    {
        // Debug.Log("help");
        StartCoroutine(Load_after_delay());
    }

    private IEnumerator Load_after_delay()
    {
        yield return new WaitForSeconds(before_load);
        SceneManager.LoadScene(name);

    }
}
