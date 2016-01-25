using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Score : MonoBehaviour {
    public static int score = 0;
    public static int finalscore = 0;

    public static List<int> shoppingList = new List<int> ();

    public void Awake () {
        // TODO: get the itemIDs from the inventory master

        // lactose free milk
        shoppingList.Add (54);

        // Bacon
        shoppingList.Add (45);

        // Apple juice
        shoppingList.Add (40);

        // Grapes
        shoppingList.Add (51);
    }

}

