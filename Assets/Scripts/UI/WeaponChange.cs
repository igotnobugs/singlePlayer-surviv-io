using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponChange : MonoBehaviour, IPointerClickHandler 
{
    private Unit unitTracked = null;
    private bool switchEnabled;

    public void SetTrackedUnit(Unit unit, bool enableSwitch = true) {
        unitTracked = unit;
        switchEnabled = enableSwitch;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!switchEnabled) return;

        unitTracked.Inventory.EquipWeaponCycle();
    }

}
