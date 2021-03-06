﻿using UnityEngine;
using System.Collections;
public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player;
    // Use this for initialization

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _inventory = _player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()  // if press E checks in the distance of 1.3 vectors if there is an item and if this item is already in the inventory, if not it gets pick uped and shown in the Invenotry and de Object will be deleted, if it is in the inventory it cant be picked up
	{
		if (_inventory != null && Input.GetKeyDown (KeyCode.E)) {
			float distance = Vector3.Distance (this.gameObject.transform.position, _player.transform.position);

			if (distance <= 1.3) {
				bool check = _inventory.checkIfItemAllreadyExist (item.itemID, item.itemValue);
				if (check)
					Destroy (this.gameObject);
				else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height)) {
					_inventory.addItemToInventory (item.itemID, item.itemValue);
					_inventory.updateItemList ();
					_inventory.stackableSettings ();
					Destroy (this.gameObject);

                    if (Score.shoppingList.Contains(item.itemID)) {
                        Score.score += 100;
                    } else {
                        Score.score -= 100;
                    }
				}
			}
		}
	}
}
