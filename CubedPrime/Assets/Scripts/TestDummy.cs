using TMPro;
using UnityEngine;

public class TestDummy : Enemy
{
    public int TotalDamageDealt; 
    private int currentDPS = 0;
    private int _DPS = 0; 
    private double _currentTime = 0f;
    public TMP_Text totalDamageText;
    public TMP_Text DPSText;
    public override void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        TotalDamageDealt += damageAmount;
        _DPS += damageAmount;
        Debug.Log(_DPS);
        _writeDamageNumber();
        healthBar.fillAmount = (float)_health / startHealth;
        if (_health <= 0)
        {
            resetHealth();
        }
    }
    public override void enemyBehaviour() {
        _currentTime += Time.deltaTime;
        _writeDPS();
        if(_currentTime >= 1.0f) {
            if(_DPS > currentDPS) currentDPS = _DPS;
            _currentTime = 0;
            _DPS = 0;
        }
    }
    public override void init() {
        
    }
    public void resetTotalDamage() {
        TotalDamageDealt = 0;
        currentDPS = 0;
        _writeDamageNumber();
        _writeDPS();
        resetHealth();
    }
    private void _writeDamageNumber() {
        totalDamageText.text = TotalDamageDealt.ToString();
    }
    private void _writeDPS() {
        DPSText.text = currentDPS.ToString();
    }

    
}
