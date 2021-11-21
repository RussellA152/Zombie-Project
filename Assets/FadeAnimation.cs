
using UnityEngine;
using System.Collections;
public class FadeAnimation : MonoBehaviour
{
    public Animator animator;
    //[SerializeField] GameObject fade_in_object;
    private void Start()
    {
        animator = GetComponent<Animator>();

        //StartCoroutine(FadeDelay());
    }
    //IEnumerator FadeDelay(float time)
    //{
        //yield return new WaitForSeconds(time);
        //fade_in_object.SetActive(false);    // FOR SOME REASON, the fade_in animation will play just before the fade_out, which will cause the screen to turn black instantly instead of fading to black
        //animator.SetTrigger("FadeToRed");
        //Debug.Log("trigger FADEOUT!");
    //}
    public void FadeToRed()
    {
        animator.SetTrigger("FadeToRed");
    }
}
