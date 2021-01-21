using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemNotification : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image icon;

    private Item item;
    private int amount;
    private float createdAt;
    private float deleteAfter;

    void Start() {
        ResetTimer();
    }

    void Update() {
        if (Time.time >= (createdAt + deleteAfter)) {
            ItemNotificationController.instance.DeleteNotification(this);
        }
    }

    public void Initialise(Item item, int amount, float deleteAfter) {
        text.SetText("+" + amount + " " + item.itemName);
        icon.sprite = item.icon;
        this.item = item;
        this.amount = amount;
        this.deleteAfter = deleteAfter;
    }

    public void Increment(int amount) {
        this.amount += amount;
        text.SetText("+" + this.amount + " " + item.itemName);
    }

    public void ResetTimer() {
        createdAt = Time.time;
    }

    public bool CheckItem(Item item) {
        if (this.item == item) {
            return true;
        }
        return false;
    }
}
