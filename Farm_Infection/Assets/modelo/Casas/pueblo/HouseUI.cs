using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseUI : MonoBehaviour
{
    public Image foodBar;
    public Image healthBar;

    public void Refresh(HouseResident house)
    {
        if (house == null) return;

        if (foodBar != null)
            foodBar.fillAmount = house.GetFoodPercent();

        if (healthBar != null)
            healthBar.fillAmount = house.GetHealthPercent();
    }
}
