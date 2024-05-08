using UnityEngine;

public class WormBoss : MonoBehaviour
{
    public int startHealth = 100;
    private int _health; 
    public float guideRange = 20f;
    public float playerRange = 55f;
    public float chargeRange = 100f;
    public float normalSpeed = 25f;
    public float lazerRange = 200f;
    public float lazerMaxWidth = 25f;
    public float lazerRotationSpeed = 3.5f;
    public LayerMask layerMask; 
    public Transform headTrans;
    public Transform fireTrans;
    public Transform fireHitBox;
    public LineRenderer lineRen; 
    public LineRenderer lazerRen;
    public GameObject bossFlame;
    public GameObject smoke1;
    public GameObject smoke2;
    private GameObject _player_ref; 
    public GameObject ExplosionFX;
    // Start is called before the first frame update
    public enum boss_states {
        FOLLOW, // Code_Red will follow for the player
        CHARGE,  // Code_Red will charge at a very fast speed at the player
        LAZER, // Code_Red will charge his lazarr
        BREATH, // Code_Red will breath fire and target the player head on. 
    }
    public boss_states _current_state;
    public Tentacle tent; 
    private Vector3 _moveDir; 
    private Vector3 _prev_dir;
    private Vector3 _new_dir;
    private float _transition_duration = 2.0f; 
    private float _transition_time = 0;
    private float _prev_speed; 
    private float _find_duration = 10.0f;
    private float _charge_duration = 10f;
    private float _breath_duration = 7.5f;
    private float _lazer_duration = 10f;
    private float _state_duration = 0f;
    private BossUI _UI;
    private bool _is_dead = false; 
    private bool _isSmoke1 = false;
    private bool _isSmoke2 = false;
    void Start()
    {
        _UI = FindObjectOfType<BossUI>();
        _UI.EnableUI();
        _UI.text.SetText("Code Red");
        _UI.text.color = Color.red;
        _player_ref = GameObject.FindGameObjectWithTag("Player");
        _moveDir = Vector2.down;
        _prev_dir = _moveDir;
        _prev_speed = normalSpeed;
        _health = startHealth;
        AudioManager.instance.fadeInNewTrack("Code_RedTheme", 1.5f);
        lineRen.positionCount = 2;
        lineRen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_is_dead) return;
        _state_duration += Time.deltaTime;
        if(_prev_dir == _new_dir) SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
        headTrans.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg - 90);
        switch (_current_state) {
            case boss_states.FOLLOW:
                if(_moveDir != _new_dir) {
                    _transition_time += Time.deltaTime;
                    float lerpFactor = _transition_time / _transition_duration;
                    lerpFactor = Mathf.Clamp(lerpFactor, 0, 1);
                    _moveDir = Vector3.Lerp(_prev_dir, _new_dir, lerpFactor);
                    if (lerpFactor >= 1)
                    {
                        _prev_dir = _new_dir;
                        _transition_time = 0;
                    }
                }
                transform.position += _moveDir * Time.deltaTime;
                if(!followConditon()) {
                    _prev_dir = _moveDir;
                    SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
                }
                if(_state_duration >= _find_duration) changeState(DecideState());
            break;
            case boss_states.CHARGE:
                _moveDir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                lineRen.SetPosition(0, fireTrans.position);
                lineRen.SetPosition(1, _player_ref.transform.position);
                
                transform.position += _moveDir * Time.deltaTime;
                if(_state_duration >= _charge_duration) changeState(DecideState());
            break;
            case boss_states.BREATH:
                _moveDir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                transform.position += _moveDir * Time.deltaTime;
                

                RaycastHit2D hit = Physics2D.CircleCast(fireHitBox.transform.position, guideRange, Vector2.right, 0f, layerMask);  
                if(hit.collider != null) PlayerStats.instance.TakeDamage(1);
                if(_state_duration >= _breath_duration) changeState(DecideState());
                break;
            case boss_states.LAZER:
                _state_duration += Time.deltaTime;
                AdjustLazerWidth();
                _prev_dir = _moveDir;
                _new_dir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                _moveDir = Vector3.Lerp(_prev_dir, _new_dir, Time.deltaTime * lazerRotationSpeed + 0.001f);
                lazerRen.SetPosition(0, fireTrans.position);
                lazerRen.SetPosition(1, fireTrans.position + fireTrans.up * lazerRange);
                if(_state_duration >= 2f) PerformDualRaycastsForLaser();
                if(_state_duration >= _lazer_duration) changeState(DecideState());
            break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Setting the color of the Gizmo to red
        Gizmos.DrawWireSphere(transform.position, playerRange); // Drawing a wireframe sphere at the transform's position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(fireHitBox.position, guideRange);

        Vector2 start = fireTrans.position;
        Vector2 direction = fireTrans.up * lazerRange;
        Vector2 perpendicular = Vector2.Perpendicular(fireTrans.up).normalized * (lazerRen.startWidth / 2);

        Gizmos.color = Color.red;
        // Draw raycast for the left edge of the laser
        Gizmos.DrawLine(start - perpendicular, start - perpendicular + direction);

        // Draw raycast for the right edge of the laser
        Gizmos.DrawLine(start + perpendicular, start + perpendicular + direction);
    }

    int checkIfNearPlayer() {
        RaycastHit2D chargeCheck = Physics2D.CircleCast(transform.position, chargeRange, Vector2.right, 0f, layerMask);  
        if(chargeCheck.collider == null) return 1;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, playerRange, Vector2.right, 0f, layerMask);  
        if (hit.collider == null) return 2;
        else return 3;
    }

    void SetNewDirection(Vector3 newDirection) {
        _new_dir = newDirection;
        _transition_time = 0;
    }

    void changeState(boss_states newState) {
        _current_state = newState;
        _state_duration = 0;
        normalSpeed = _prev_speed;
        bossFlame.SetActive(false);
        lineRen.enabled = false;
        lazerRen.enabled = false;
        if(_current_state == boss_states.BREATH) {
            AudioManager.instance.PlayOnShot("Code_RedRoar1");
            speed_change(0.35f);
            bossFlame.SetActive(true);
        }
        if(_current_state == boss_states.LAZER) {
            AudioManager.instance.PlayOnShot("Code_RedChargeUp");
            speed_change(0.01f);
            lazerRen.enabled = true;
        }
        if(_current_state == boss_states.CHARGE) {
            AudioManager.instance.PlayOnShot("Code_RedTransition");
            lineRen.enabled = true;
            _prev_speed = normalSpeed;
            normalSpeed *= 0.5f;
        }
        if(_current_state == boss_states.FOLLOW) {
            AudioManager.instance.PlayOnShot("Code_RedError");
            _prev_dir = _moveDir;
            SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
        }
    }
    void PerformDualRaycastsForLaser()
    {
        Vector2 start = fireTrans.position;
        Vector2 direction = fireTrans.up;

        // Calculate the offset for the width of the laser
        Vector2 perpendicular = Vector2.Perpendicular(direction).normalized * (lazerRen.startWidth / 2);

        // Raycast on the left edge of the laser
        RaycastHit2D leftHit = Physics2D.Raycast(start - perpendicular, direction, lazerRange, layerMask);
        if (leftHit.collider != null)
        {
            Debug.Log("Hit by left ray: " + leftHit.collider.name);
            PlayerStats.instance.TakeDamage(1);
            return;
        }

        // Raycast on the right edge of the laser
        RaycastHit2D rightHit = Physics2D.Raycast(start + perpendicular, direction, lazerRange, layerMask);
        if (rightHit.collider != null)
        {
            Debug.Log("Hit by right ray: " + rightHit.collider.name);
            PlayerStats.instance.TakeDamage(1);
            return;
        }
    }

    void speed_change(float speedDelta) {
        _prev_dir = _moveDir;
        _prev_speed = normalSpeed;
        normalSpeed *= speedDelta;
    }

    bool chargeCondition() {
         if(checkIfNearPlayer() == 1) return true;
         else return false;
    }

    bool followConditon() {
         if(checkIfNearPlayer() == 2) return true;
         else return false;
    }

    bool healthIsWithinRange(float range) {
        return (double)_health / startHealth <= range;
    }

    boss_states DecideState() {
        int roll = Random.Range(1, 10);
        if(healthIsWithinRange(0.1f)) return boss_states.CHARGE; 
        if(_current_state == boss_states.LAZER) {
            if(chargeCondition()) return boss_states.CHARGE;
            if(followConditon() && roll <= 4) return boss_states.FOLLOW;
            return boss_states.BREATH;
        }
        if(_current_state == boss_states.FOLLOW) {

            if(chargeCondition()) return boss_states.CHARGE;
            if(roll <=3 && healthIsWithinRange(0.8f)) return boss_states.LAZER;
            return boss_states.BREATH;
        }
        if(_current_state == boss_states.CHARGE) {
            // Keep on charging.. 
            if(chargeCondition()) return boss_states.CHARGE;
            if(followConditon() && roll <= 3) return boss_states.FOLLOW;
            if(roll >3 && healthIsWithinRange(0.8f)) return boss_states.LAZER;
            return boss_states.BREATH;
        }
        if(_current_state == boss_states.BREATH) {
            
            if(chargeCondition()) return boss_states.CHARGE;
            if(roll <=3 && healthIsWithinRange(0.8f)) return boss_states.LAZER;
            return boss_states.FOLLOW;
        } 

        return boss_states.FOLLOW;
    }

    void AdjustLazerWidth()
    {
        float halfDuration = _lazer_duration / 2f;
        float width;
        float normalizedTime = _state_duration % _lazer_duration / halfDuration;
        if (normalizedTime < 1f) width = Mathf.Lerp(0.05f, lazerMaxWidth, normalizedTime); // Increase width
        else width = Mathf.Lerp(lazerMaxWidth, 0.05f, normalizedTime - 1f); // Decrease width
        lazerRen.startWidth = width;
        lazerRen.endWidth = width;
    }

    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        _UI.BossHealthBar.fillAmount = (float)_health / startHealth;
        if(healthIsWithinRange(0.8f) && !_isSmoke1) {
            smoke1.SetActive(true);
            AudioManager.instance.PlayOnShot("RoboticScream");
            _isSmoke1 = true;
        }
        if(healthIsWithinRange(0.4f) && !_isSmoke2) {
            smoke2.SetActive(true);
            AudioManager.instance.PlayOnShot("RoboticScream1");
            _isSmoke2 = true;
        }
        if (_health <= 0 && !_is_dead)
        {
            _is_dead = true;
            bossFlame.SetActive(false);
            lineRen.enabled = false;
            lazerRen.enabled = false;
            tent.enabled = false;
            Boss1Segments[] bodyParts = GetComponentsInChildren<Boss1Segments>();
            foreach (var body in bodyParts) {
                body.onDeath();
            }
            GameObject explosion = Instantiate(ExplosionFX, headTrans.transform.position, headTrans.transform.rotation);
            ExplosionObj exp = explosion.GetComponent<ExplosionObj>();
            exp.radius = 35f;
            Debug.Log("Hes Dead jim");
            _UI.DisableUI();
            Destroy(gameObject);
            
        }
    }

    public int getHealth() {
        return _health;
    }

    public bool isAlive() {
        return !_is_dead;
    }
}


