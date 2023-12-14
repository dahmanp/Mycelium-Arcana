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

    //Animation
    private Animator anim;
    Vector2 movement;

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

    private void Start()
    {
        anim = GetComponent<Animator>();
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

        //animation
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        lastAttackTime = Time.time;
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);

        //swordA.Play();
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            totalDmgDealt += damage;
            if (enemy.curHp >= 0 || enemy.isdead == true)
            {
                enemiesKilled++;
            }
        } else if (hit.collider != null && hit.collider.gameObject.CompareTag("Boss"))
        {
            Boss boss = hit.collider.GetComponent<Boss>();
            boss.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            totalDmgDealt += damage;
            if (boss.curHp >= 0 || boss.isdead == true)
            {
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

    public void submitDMGAmt()
    {
        int temptime = -Mathf.RoundToInt(totalDmgDealt * 1000.0f);
        Leaderboard2.instance.SetLeaderboardEntry(temptime);
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