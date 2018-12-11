using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadePanel : MonoBehaviour {

    public Animator anim;
    public string sceneName;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();    
    }

    public void ChangeScenes(string sceneName)
    {
        this.sceneName = sceneName;
        anim.SetTrigger("FadeIn");
    }

    public void FadeIn()
    {
        SceneManager.LoadScene(sceneName);
    }

}
