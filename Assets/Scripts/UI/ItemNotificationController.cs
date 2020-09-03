using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNotificationController : MonoBehaviour {

    #region Singleton
    public static ItemNotificationController instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of ItemNotificationController found!");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject NotificationPrefab;
    public float deleteAfter = 5f;  //seconds

    private List<ItemNotification> notificationList = new List<ItemNotification>();

    public void UpdateOrCreateNotification(Item item, int amount) {

        //if one already exists for this item, update the amount and reset the timer
        foreach (ItemNotification currentNotification in notificationList) {
            if (currentNotification.CheckItem(item)) {
                currentNotification.Increment(amount);
                currentNotification.ResetTimer();
                return;
            }
        }

        //create a new notification if one doesnt exist;
        GameObject notificationHolder = Instantiate(NotificationPrefab);
        notificationHolder.transform.SetParent(this.transform);
        ItemNotification itemNotification = notificationHolder.GetComponent<ItemNotification>();
        itemNotification.Initialise(item, amount, deleteAfter);
        notificationList.Add(itemNotification);
    }

    public void DeleteNotification(ItemNotification itemNotification) {
        notificationList.Remove(itemNotification);
        Destroy(itemNotification.gameObject);
    }
}
