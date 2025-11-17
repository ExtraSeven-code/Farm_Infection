using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrigger : MonoBehaviour
{
    private ChoppableTree tree;
    private bool playerInside;

    private void Awake()
    {
        tree = GetComponentInParent<ChoppableTree>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    public bool IsPlayerInside()
    {
        return playerInside;
    }

    public ChoppableTree GetTree()
    {
        return tree;
    }
}
