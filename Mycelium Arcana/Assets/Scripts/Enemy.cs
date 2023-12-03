using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    [SerializeField] ParticleSystem _particleSystem;

    [Header("Info")]
    public string enemyName;
    public float moveSpeed;

    public int curHp;
    public int maxHp;

    public float chaseRange;
    public float attackRange;

    private PlayerController targetPlayer;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    public string objectToSpawnOnDeath;

    public AudioSource dmgA;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    public HeaderInfo healthBar;
    public SpriteRenderer sr;
    public Rigidbody2D rig;

    public bool isdead = false;

    public static Enemy instance;

    void Start()
    {
        healthBar.Initialize(enemyName, maxHp);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (targetPlayer != null)
        {
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                Attack();
            }
            else if (dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }
            else
            {
                rig.velocity = Vector2.zero;
            }
        }
        DetectPlayer();
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        targetPlayer.photonView.RPC("TakeDamage", targetPlayer.photonPlayer, damage);

    }

    void DetectPlayer()
    {
        if (Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            lastPlayerDetectTime = Time.time;
            foreach (PlayerController player in GameManager.instance.players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (player == targetPlayer)
                {
                    if (dist > chaseRange)
                        targetPlayer = null;
                }
                else if (dist < chaseRange)
                {
                    if (targetPlayer == null)
                        targetPlayer = player;
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        //dmgA.Play();
        curHp -= damage;
        healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        if (curHp <= 0)
        {
            Die();
        }
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }

    void Die()
    {
        isdead = true;
        if (objectToSpawnOnDeath != string.Empty)
            PhotonNetwork.Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(gameObject);
    }
}