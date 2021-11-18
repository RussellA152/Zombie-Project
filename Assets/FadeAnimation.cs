
using UnityEngine;
using System.Collections;
public class FadeAnimation : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(FadeDelay());
    }
    IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(4f);
        animator.SetTrigger("FadeOut");
        Debug.Log("trigger FADEOUT!");
    }
}
