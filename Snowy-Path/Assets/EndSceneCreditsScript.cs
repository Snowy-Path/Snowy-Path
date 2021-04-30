using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndSceneCreditsScript : MonoBehaviour
{
    public PlayableDirector creditScroller;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fireRateCoroutine());
    }

    IEnumerator fireRateCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        creditScroller.Play();
    }
}
