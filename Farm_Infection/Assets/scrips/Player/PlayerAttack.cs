using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Player_Movimiento movement;
    public HotbarSelector hotbar;

    [Header("Ataque")]
    public float attackDuration = 0.6f;   
    public float hitTime = 0.35f;        

    private bool isAttacking = false;
    private ChoppableTree currentTree;   

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            ItemData tool = hotbar.GetSelectedItem();
            if (tool != null && tool.isTool && currentTree != null)
            {
                StartCoroutine(AttackRoutine(tool));
            }
        }
    }

    IEnumerator AttackRoutine(ItemData tool)
    {
        isAttacking = true;
        movement.canMove = false;          

        animator.SetTrigger("Atacar");    

        yield return new WaitForSeconds(hitTime);

        if (currentTree != null)
        {
            currentTree.Hit(tool);
        }

        float remaining = Mathf.Max(0f, attackDuration - hitTime);
        yield return new WaitForSeconds(remaining);

        movement.canMove = true;           
        isAttacking = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        var tree = other.GetComponent<ChoppableTree>();
        if (tree != null)
            currentTree = tree;
    }

    private void OnTriggerExit(Collider other)
    {
        var tree = other.GetComponent<ChoppableTree>();
        if (tree != null && tree == currentTree)
            currentTree = null;
    }
}
