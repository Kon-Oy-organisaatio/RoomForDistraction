using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomAnimationHandler : MonoBehaviour
{
    public GameObject randomHolder;
    public RandomAnimationParams animationParams;

    private IEnumerator Start()
    {
        yield return null; // Wait one frame so all Start() methods run

        var children = new List<GameObject>();
        foreach (Transform item in randomHolder.transform)
        {
            RandomAnimator animator = item.gameObject.AddComponent<RandomAnimator>();
            animator.Init(animationParams);
            children.Add(item.gameObject);
        }
    }
}