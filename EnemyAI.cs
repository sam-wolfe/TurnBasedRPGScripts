using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public float timer;

    void Update() {

        if (!TurnSystem.instance.IsPlayerTurn()) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 3;
                TurnSystem.instance.NextTurn();
            }
        }

    }
}
