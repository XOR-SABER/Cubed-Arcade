using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Segments : MonoBehaviour
{
    public WormBoss mainWorm; 
    public void TakeDamage(int damageAmount) {
        mainWorm.TakeDamage(damageAmount);
    }
}
