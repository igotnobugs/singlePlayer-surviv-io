using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour 
{
    [Header("Health")]
    [SerializeField] private Image healthBar = null;

    [Header("Weapons and Ammo")]
    [SerializeField] private Image ammoIcon = null;
    [SerializeField] private Image ammoIconSprite = null;
    [SerializeField] private TextMeshProUGUI ammoLoadedText = null;
    [SerializeField] private TextMeshProUGUI ammoAvailableText = null;
    [SerializeField] private Image reloadBar = null;
    [SerializeField] private Image weaponIcon = null;
    [SerializeField] private Image weaponCycleImage = null;
    [SerializeField] private TextMeshProUGUI weaponName = null;
    [SerializeField] private WeaponChange weaponChangeButton = null;

    [Header("Buttons")]
    [SerializeField] private Button reloadButton = null;  

    [Header("Other Panels")]
    [SerializeField] private RectTransform gameOver = null;
    [SerializeField] private TextMeshProUGUI gamerOverKiller = null;
    [SerializeField] private TextMeshProUGUI unitsLeftText = null;
    [SerializeField] private TextMeshProUGUI unitsTotalText = null;

    private Unit unitTracked = null;
    private Weapon weaponTracked;
    private Ammo ammoTracked;
    private ItemStack ammoStack;
    private bool isTrackingStart = false;
    private bool isThisPlayer;

    private void Awake() {
        gameOver.gameObject.SetActive(false);
        isTrackingStart = false;
    }

    public void StartTracking(Unit unit, bool isPlayer = true) {
        unitTracked = unit;
        unitTracked.Inventory.OnInventoryUpdated += UpdateUI;
        UpdateUI();
        isTrackingStart = true;
        isThisPlayer = isPlayer;
        weaponChangeButton.SetTrackedUnit(unitTracked);
    }

    private void Update() {
        unitsLeftText.text = GameManager.unitsLeft.ToString();
        unitsTotalText.text = "/ " + GameManager.unitsTotal.ToString();

        if (!isTrackingStart) return;

        if (unitTracked == null) {
            healthBar.fillAmount = 0;
            return;
        }

        if (weaponTracked == null) {
            Debug.Log("NULL WEAPON FOR " + unitTracked.UnitName + " CHECK AGAIN");
            UpdateUI();
            return;
        }

        ammoLoadedText.text = weaponTracked.AmmoLoaded.ToString();

        if (weaponTracked as Gun) {
            reloadBar.fillAmount = (weaponTracked as Gun).GetReloadPercent();
        }

        healthBar.fillAmount = unitTracked.GetHealthRatio();
    }

    private void UpdateUI() {
        if (unitTracked == null) return;
        weaponTracked = unitTracked.Inventory.equippedWeapon;

        if (weaponTracked == null) return;

        //Reload button
        if (isThisPlayer) {
            if (weaponTracked.AmmoRequired == null) {
                reloadButton.gameObject.SetActive(false);
            }
            else {
                reloadButton.gameObject.SetActive(true);
                if (weaponTracked as Gun) {
                    reloadButton.onClick.AddListener(() => ReloadButtonPress());
                }
            }
        } else {
            reloadButton.gameObject.SetActive(false);
        }

        //Weapon Button
        weaponIcon.sprite = weaponTracked.itemIcon;
        weaponName.text = weaponTracked.ItemName;
        if (unitTracked.Inventory.GetWeaponCount() > 1) {
            weaponCycleImage.gameObject.SetActive(true);
        }
        else {
            weaponCycleImage.gameObject.SetActive(false);
        }


        //Ammo Tracker
        if (weaponTracked.AmmoRequired != ammoTracked) {
            ammoTracked = weaponTracked.AmmoRequired;

            if (weaponTracked.AmmoRequired == null ) {
                ammoIcon.gameObject.SetActive(false);
            } else {
                ammoIcon.gameObject.SetActive(true);
                ammoIconSprite.sprite = ammoTracked.itemIcon;
                ammoIconSprite.color = ammoTracked.color;
            }

            ammoStack = unitTracked.Inventory.activeAmmoStack;
        }

        if (weaponTracked.AmmoRequired != null && ammoStack != null) {
            ammoAvailableText.text = ammoStack.itemAmount.ToString();
            return;
        }

        ammoAvailableText.text = "0";
    }

    public void ShowGameOver(string killerName) {
        gameOver.gameObject.SetActive(true);
        gamerOverKiller.text = killerName;
    }

    private void ReloadButtonPress() {
        (weaponTracked as Gun).Reload();
    }
}
