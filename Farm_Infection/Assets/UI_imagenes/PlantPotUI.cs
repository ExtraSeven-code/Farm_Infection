using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantPotUI : MonoBehaviour
{
    public Image fruitIcon;
    public Image waterIcon;
    public Image handIcon;

    [Header("Distancia de visibilidad")]
    public float showDistance ;
    public Transform player;   

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void Update()
    {
        if (player == null || canvas == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool shouldShow = dist <= showDistance;

        if (canvas.enabled != shouldShow)
            canvas.enabled = shouldShow;
    }

    public void Refresh(PlantPot pot)
    {
        if (pot == null || !pot.HasSeed)
        {
            SetAll(false, false, false);
            return;
        }

        // Fruta
        if (pot.PlantedSeed != null)
        {
            Sprite s = null;

            if (pot.PlantedSeed.harvestItem != null && pot.PlantedSeed.harvestItem.icon != null)
                s = pot.PlantedSeed.harvestItem.icon;
            else if (pot.PlantedSeed.icon != null)
                s = pot.PlantedSeed.icon;

            if (s != null)
            {
                fruitIcon.sprite = s;
                fruitIcon.enabled = true;
            }
            else
            {
                fruitIcon.enabled = false;
            }
        }

        if (waterIcon != null)
            waterIcon.enabled = !pot.IsWatered && !pot.IsFullyGrown;

        if (handIcon != null)
            handIcon.enabled = pot.IsFullyGrown;
    }

    void SetAll(bool fruit, bool water, bool hand)
    {
        if (fruitIcon != null) fruitIcon.enabled = fruit;
        if (waterIcon != null) waterIcon.enabled = water;
        if (handIcon != null) handIcon.enabled = hand;
    }
}
