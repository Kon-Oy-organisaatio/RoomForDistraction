using UnityEngine;
using System.Collections.Generic;

public class RandomAnimationHandler : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject randomHolder;
    public RandomAnimationParams animationParams;

    public void Start()
    {
        var children = new List<GameObject>();
        foreach (Transform item in randomHolder.transform)
        {
            RandomAnimator animator = item.gameObject.AddComponent<RandomAnimator>();
            animator.Init(animationParams);
            children.Add(item.gameObject);
        }
    }
}
