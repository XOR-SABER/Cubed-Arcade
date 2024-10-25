using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameModeManager : MonoBehaviour {
    public float spawnDistance; 
    public float enemySpawnTime; 
    public float WeaponSpawnTime;
    public float TurretSpawnTime;
    public float BossSpawnTime;
    public TextMeshProUGUI timeText;
    public DropTableStats enemiesLowTier;
    public DropTableStats enemiesMiddleTier;
    public DropTableStats enemiesHighTier;
    public GameObject[] turrets;
    public DropTableStats lowTierWeapons;
    public DropTableStats midTierWeapons;
    public DropTableStats highTierWeapons;
    public Transform[] enemySpawnLocations;
    public Transform[] turretSpawnLocations;
    public Transform[] weaponSpawnLocations;
    private float _time; 
    private float _enemy_current_spawn_time;
    private float _turret_current_spawn_time;
    private float _boss_current_spawn_time;
    private float _weapon_current_spawn_time;
    private Transform _player_ref;
    public LayerMask weaponMask;
    public LayerMask enemyMask;
    public Transform bossSpawnLoc;
    public GameObject boss;

    private void Start() {
        _player_ref = PlayerStats.instance.getPlayerRef().transform;
        SpawnEnemy(); 
        SpawnWeapon();
        SpawnTurret();
        if (_player_ref == null) Debug.LogError("Please set the player object with the 'Player' Tag");
    }
    private void Update() {
        _time += Time.deltaTime; 
        _enemy_current_spawn_time += Time.deltaTime;
        _weapon_current_spawn_time += Time.deltaTime;
        _turret_current_spawn_time += Time.deltaTime;
        _boss_current_spawn_time += Time.deltaTime;
        if(_weapon_current_spawn_time >= WeaponSpawnTime) {
            _weapon_current_spawn_time = 0;
            SpawnWeapon();
        }
        if(_enemy_current_spawn_time >= enemySpawnTime) {
            _enemy_current_spawn_time = 0;
            SpawnEnemy(); 
        }
        if(_turret_current_spawn_time >= TurretSpawnTime) {
            _turret_current_spawn_time = 0;
            SpawnTurret();
        }
        if(_boss_current_spawn_time >= BossSpawnTime && !PlayerStats.instance.isBossActive) {
            _boss_current_spawn_time = 0;
            SpawnBoss();
        }
        timeText.text = string.Format("Time survived: {0} seconds", math.round(_time));
        DiscordPresence.state = PlayerStats.instance.scoreText.text;
    }

    private void SpawnEnemy() {
        if (enemySpawnLocations.Length > 0) {
            if(PlayerStats.instance.currentEnemiesCount >= PlayerStats.instance.maxEnemies) return;
            int rand = UnityEngine.Random.Range(5, enemySpawnLocations.Length);
            List<Transform> locations = new List<Transform>(); 
            DecideLocations(ref locations, enemySpawnLocations);
            int number = 0;
            foreach(Transform loc in locations) {
                number++;
                PlayerStats.instance.currentEnemiesCount++;
                Instantiate(DecideEnemy(), loc.position, quaternion.identity);
                if(number >= rand) break;
            }
        }
    }

    private void SpawnWeapon() {
        if (weaponSpawnLocations.Length > 0) {
            List<Transform> locations = new List<Transform>(); 
            DecideLocations(ref locations, weaponSpawnLocations);
            foreach(Transform loc in locations) {
                RaycastHit2D hit = Physics2D.CircleCast(loc.position, 1f, Vector2.right, 1f, weaponMask);
                if(hit.collider == null) {
                    Debug.Log("WeaponSpawned!");
                    Instantiate(DecideWeapon(), loc.position, loc.rotation);
                    return;
                } else Debug.Log(hit.collider.gameObject);
            }
             Debug.Log("Weapon didn't spawn no space!");
        }
    }

    private void SpawnTurret() {
        if (turrets.Length > 0) {
            if(PlayerStats.instance.currentEnemiesCount >= PlayerStats.instance.maxEnemies) return;
            List<Transform> locations = new List<Transform>(); 
            DecideLocations(ref locations, turretSpawnLocations);
            foreach(Transform loc in locations) {
                RaycastHit2D hit = Physics2D.CircleCast(loc.position, 1f, Vector2.right,1f, enemyMask);
                if(hit.collider == null) {
                    Debug.Log("TurretSpawned!");
                    PlayerStats.instance.currentEnemiesCount++;
                    int rand = UnityEngine.Random.Range(0, turrets.Length);
                    Instantiate(turrets[rand], loc.position, loc.rotation);
                    return;
                }
            }
            Debug.Log("Turret didn't spawn no space!");
        }
    }

    private void SpawnBoss() {
        
        Instantiate(boss, bossSpawnLoc.position, quaternion.identity);
    }

    private void DecideLocations(ref List<Transform> finalLocations, Transform[] locations) { 
        if(_player_ref == null) return;
        Transform player = _player_ref.transform;
        foreach (Transform trans in locations) {
            float distance = Vector3.Distance(player.position, trans.position);
            if(distance < spawnDistance) continue;
            finalLocations.Add(trans);
        }
    }

    private GameObject DecideEnemy() {
        int randEnemy;
        int rand = UnityEngine.Random.Range(0, 10);
        if(_time >= 0 && _time < 300) {
            // The first 5 minutes
            rand -= 3;
            rand = Math.Clamp(rand, 0, 10);
        } else if(_time >= 300 && _time < 600) {
            rand -= 1;
            rand = Math.Clamp(rand, 0, 10);
            //  if the time is between 5 and 10 minutes;
            // Spawn the boss
        } else if (_time >= 600 && _time < 900)  {
            rand += 1;
            rand = Math.Clamp(rand, 0, 10);
            // if the time is between 10 and 15 minutes;
        } else if(_time >= 900) {
            rand += 3;
            rand = Math.Clamp(rand, 0, 10);
            // Shit is getting real
        }
        if(rand <= 4) {
            randEnemy = UnityEngine.Random.Range(0, enemiesLowTier.objs.Length);
            return enemiesLowTier.objs[randEnemy].obj;
        }
        if(rand > 4 && rand <= 7) {
            randEnemy = UnityEngine.Random.Range(0, enemiesMiddleTier.objs.Length);
            return enemiesMiddleTier.objs[randEnemy].obj;
        }
        if(rand > 7) {
            randEnemy = UnityEngine.Random.Range(0, enemiesHighTier.objs.Length);
            return enemiesHighTier.objs[randEnemy].obj;
        }
        randEnemy = UnityEngine.Random.Range(0, enemiesLowTier.objs.Length);
        return enemiesLowTier.objs[randEnemy].obj;
    }

    private GameObject DecideWeapon() {
        int randWeapon;
        int rand = UnityEngine.Random.Range(0, 10);
        if(_time >= 0 && _time < 300) {
            rand -= 3;
            // The first 5 minutes
            rand = Math.Clamp(rand, 0, 10);
        } else if(_time >= 300 && _time < 600) {
            rand -= 1;
            rand = Math.Clamp(rand, 0, 10);
            //  if the time is between 5 and 10 minutes;
            // Spawn the boss
        } else if (_time >= 600 && _time < 900)  {
            rand += 1;
            rand = Math.Clamp(rand, 0, 10);
            // if the time is between 10 and 15 minutes;
        } else if(_time >= 900) {
            rand += 3;
            rand = Math.Clamp(rand, 0, 10);
            // Shit is getting real
        }
        if(rand <= 4) {
            randWeapon = UnityEngine.Random.Range(0, lowTierWeapons.objs.Length);
            return lowTierWeapons.objs[randWeapon].obj;
        }
        if(rand > 4 && rand <= 7) {
            randWeapon = UnityEngine.Random.Range(0, midTierWeapons.objs.Length);
            return midTierWeapons.objs[randWeapon].obj;
        }
        if(rand > 7) {
            randWeapon = UnityEngine.Random.Range(0, highTierWeapons.objs.Length);
            return highTierWeapons.objs[randWeapon].obj;
        }
        randWeapon = UnityEngine.Random.Range(0, lowTierWeapons.objs.Length);
        return lowTierWeapons.objs[randWeapon].obj;
    }
}  