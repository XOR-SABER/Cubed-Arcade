using UnityEngine;

public class WormBoss : MonoBehaviour
{
    public int startHealth = 100;
    private int _health; 
    public float playerRange = 55f;
    public float chargeRange = 100f;
    public float normalSpeed = 25f;
    public float lazerRange = 200f;
    public float lazerMaxWidth = 25f;
    public LayerMask layerMask; 
    public Transform headTrans;
    public Transform fireTrans;
    public LineRenderer lineRen; 
    public LineRenderer lazerRen;
    public GameObject bossFlame;
    private GameObject _player_ref; 
    // Start is called before the first frame update
    public enum boss_states {
        FIND_PLAYER, // Moris will search for the player
        CHARGE,  // Moris will charge at a very fast speed at the player
        SCREAM, // Moris will call out for help 
        LAZER, // Moris will charge his lazarr
        BREATH, // Moris will breath fire and target the player head on. 
        TRANS, // Moris will transtion to His Second phase
        LAZER2, // Moris is a dick
        DEFENSIVE, // Moris Will repel the player.
        LAST_STAND, // Moris will stop moving and Shoot missles at the player.. 
        DEATH, // Moris will live on forever, as a weapon.. 

    }
    public boss_states _current_state;
    private Vector3 _moveDir; 
    private Vector3 _prev_dir;
    private Vector3 _new_dir;
    private float _transition_duration = 2.0f; 
    private float _transition_time = 0;
    private float _prev_speed; 
    private float _find_duration = 12.5f;
    private float _charge_duration = 10f;
    private float _breath_duration = 7.5f;
    private float _lazer_duration = 10f;
    private float _state_duration = 0f;
    void Start()
    {
        _player_ref = GameObject.FindGameObjectWithTag("Player");
        _moveDir = Vector2.down;
        _prev_dir = _moveDir;
        _prev_speed = normalSpeed;
        _health = startHealth;
        lineRen.positionCount = 2;
        lineRen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_current_state != boss_states.LAZER) {
            float angle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg;
            headTrans.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        if(_prev_dir == _new_dir) {
            SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
        }   
        checkIfNearPlayer();
        switch (_current_state) {
            case boss_states.FIND_PLAYER:
                _state_duration += Time.deltaTime;
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
                if(_state_duration >= _find_duration) changeState(boss_states.LAZER);
            break;
            case boss_states.CHARGE:
                _state_duration += Time.deltaTime;
                _moveDir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                lineRen.SetPosition(0, fireTrans.position);
                lineRen.SetPosition(1, _player_ref.transform.position);
                
                transform.position += _moveDir * Time.deltaTime;
                if(_state_duration >= _charge_duration) changeState(boss_states.FIND_PLAYER);
            break;
            case boss_states.BREATH:
                _state_duration += Time.deltaTime;
                _moveDir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                transform.position += _moveDir * Time.deltaTime;
                if(_state_duration >= _breath_duration) changeState(boss_states.FIND_PLAYER);
                break;
            case boss_states.LAZER:
                lazerRen.enabled = true;
                _state_duration += Time.deltaTime;
                AdjustLazerWidth();
                _moveDir = (_player_ref.transform.position - transform.position).normalized * normalSpeed;
                lazerRen.SetPosition(0, fireTrans.position);
                lazerRen.SetPosition(1, fireTrans.position + fireTrans.up * lazerRange);

                Vector3 direction = _player_ref.transform.position - headTrans.position;
                Quaternion rotation = Quaternion.AngleAxis((Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 270, Vector3.forward);
                headTrans.rotation = Quaternion.Slerp(headTrans.rotation, rotation, Time.deltaTime * 1.5f);
                // transform.position += _moveDir * Time.deltaTime;
                if(_state_duration >= _lazer_duration) changeState(boss_states.FIND_PLAYER);
            break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Setting the color of the Gizmo to red
        Gizmos.DrawWireSphere(transform.position, playerRange); // Drawing a wireframe sphere at the transform's position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
    }

    void checkIfNearPlayer() {
        if(_current_state == boss_states.LAZER) return;
        if(_current_state == boss_states.BREATH) return;
        if(_current_state == boss_states.CHARGE) return;
        
        RaycastHit2D chargeCheck = Physics2D.CircleCast(transform.position, chargeRange, Vector2.right, 0f, layerMask);  
        if(chargeCheck.collider == null) {
            changeState(boss_states.CHARGE);
            lineRen.enabled = true;
            _prev_speed = normalSpeed;
            normalSpeed *= 0.5f;
            return;
        }
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, playerRange, Vector2.right, 0f, layerMask);  

        if (hit.collider == null) {
            if(_current_state != boss_states.FIND_PLAYER) changeState(boss_states.FIND_PLAYER);
            _prev_dir = _moveDir;
            SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
            return;
        }
    }

    void SetNewDirection(Vector3 newDirection) {
        _new_dir = newDirection;
        _transition_time = 0;
    }

    void changeState(boss_states newState) {
        Debug.Log("State Changed to " + newState);
        _current_state = newState;
        _state_duration = 0;
        normalSpeed = _prev_speed;
        bossFlame.SetActive(false);
        lineRen.enabled = false;
        lazerRen.enabled = false;

        int roll = Random.Range(1, 10);
        if(roll >= 7 && _current_state == boss_states.FIND_PLAYER) {
            _current_state = boss_states.BREATH;
            _prev_dir = _moveDir;
            _prev_speed = normalSpeed;
            normalSpeed *= 0.25f;
            bossFlame.SetActive(true);
            return;
        }
        if(!((double)_health / startHealth <= 0.8f)) return;
        if(roll >= 3 && _current_state != boss_states.LAZER) {
            _current_state = boss_states.LAZER;
            _prev_dir = _moveDir;
            _prev_speed = normalSpeed;
            normalSpeed *= 0.01f;
        }
    }

    void AdjustLazerWidth()
    {
        float halfDuration = _lazer_duration / 2f;
        float width;
        float normalizedTime = _state_duration % _lazer_duration / halfDuration;
        if (normalizedTime < 1f)
        {
            width = Mathf.Lerp(0.05f, lazerMaxWidth, normalizedTime); // Increase width
        }
        else
        {
            width = Mathf.Lerp(lazerMaxWidth, 0.05f, normalizedTime - 1f); // Decrease width
        }
        Debug.Log("Lazer width" + width);
        lazerRen.startWidth = width;
        lazerRen.endWidth = width;
    }

    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        Debug.Log("Health: " + _health);
        // healthBar.fillAmount = (float)health / startHealth;
        if (_health <= 0)
        {
            Debug.Log("Do something here");
        }
    }
}


