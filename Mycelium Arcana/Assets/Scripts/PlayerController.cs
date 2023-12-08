using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public int gold;
    public int keys;
    public int curHp;
    public int maxHp;
    public bool dead;
    public float startTime;
    public float timeTaken;

    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    public int enemiesKilled;
    public int totalDmgDealt;
    private float lastAttackTime;

    [Header("Components")]
    public Rigidbody2D rig;
    public Player photonPlayer;
    public SpriteRenderer sr;
    //public Animator weaponAnim;
    public bool hasKey;

    [Header("Audio")]
    public AudioSource coinA;
    public AudioSource keyA;
    public AudioSource healthA;
    public AudioSource swordA;
    public AudioSource dmgA;
    public AudioSource spawn;
    public AudioSource die;

    public HeaderInfo headerInfo;

    // local player
    public static PlayerController me;

    public PlayerController instance;

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;

        if (player.IsLocal)
            me = this;
        else
            rig.isKinematic = true;

        GameManager.instance.players[id - 1] = this;
        headerInfo.Initialize(player.NickName, maxHp);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        Move();
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate)
            Attack();

        float mouseX = (Screen.width / 2) - Input.mousePosition.x;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rig.velocity = new Vector2(x, y) * moveSpeed;
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);

        //swordA.Play();
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("ytesting");
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            totalDmgDealt += damage;
            //below line doesnt work, maybe it has to do with the fact that its not as fast as id0
            if (enemy.isdead == true)
            {
                Debug.Log("ya hehe");
                enemiesKilled++;
            }
        } else if (hit.collider != null && hit.collider.gameObject.CompareTag("Boss"))
        {
            //Debug.Log("ytesting");
            Boss boss = hit.collider.GetComponent<Boss>();
            boss.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            totalDmgDealt += damage;
            //below line doesnt work, maybe it has to do with the fact that its not as fast as id0
            if (boss.isdead == true)
            {
                Debug.Log("ya hehe");
                enemiesKilled++;
            }
        }
        //weaponAnim.SetTrigger("Attack");
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        //dmgA.Play();
        if (curHp <= 0)
            Die();
        else
        {
            StartCoroutine(DamageFlash());
            IEnumerator DamageFlash()
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.05f);
                sr.color = Color.white;
            }
        }
    }

    void Die()
    {
        //die.Play();
        dead = true;
        rig.isKinematic = true;
        transform.position = new Vector3(0, 99, 0);
        Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;
        StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    IEnumerator Spawn(Vector3 spawnPos, float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);
        //spawn.Play();
        dead = false;
        transform.position = spawnPos;
        curHp = maxHp;
        rig.isKinematic = false;
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
    }

    [PunRPC]
    void Heal(int amountToHeal)
    {
        //healthA.Play();
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
    }

    [PunRPC]
    void GiveKey()
    {
        hasKey = true;
    }

    [PunRPC]
    void Magic(GameObject magicType, int dmg)
    {
        
    }
}