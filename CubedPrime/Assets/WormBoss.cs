using UnityEngine;

public class WormBoss : MonoBehaviour
{
    public float playerRange = 55f;
    public float chargeRange = 100f;
    public float normalSpeed = 25f;
    public LayerMask layerMask; 
    public Transform headTrans;
    public Transform fireTrans;
    public LineRenderer lineRen; 
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
    private float _charge_duration = 10f;
    private float _state_duration = 0f;
    void Start()
    {
        _player_ref = GameObject.FindGameObjectWithTag("Player");
        _moveDir = Vector2.down;
        _prev_dir = _moveDir;
        lineRen.positionCount = 2;
        lineRen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg;
        headTrans.rotation = Quaternion.Euler(0, 0, angle - 90);

        checkIfNearPlayer();
        switch (_current_state) {
            case boss_states.FIND_PLAYER:
                if(_prev_dir == _new_dir) {
                    SetNewDirection((_player_ref.transform.position - transform.position).normalized * normalSpeed);
                }   
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
                if(_state_duration >= _charge_duration) changeState(boss_states.FIND_PLAYER);
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
        bossFlame.SetActive(false);
        lineRen.enabled = false;

        int roll;
        if(_current_state == boss_states.FIND_PLAYER) {
            normalSpeed = _prev_speed;
            roll = Random.Range(1, 10);
            if(roll >= 5) {
                changeState(boss_states.BREATH);
                _prev_dir = _moveDir;
                _prev_speed = normalSpeed;
                normalSpeed *= 0.25f;
                bossFlame.SetActive(true);
            }
        }
    }
}
