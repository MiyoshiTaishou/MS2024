using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    [Header("HP(float)とHPバー設定")]
    //最大HPと現在のHP。
    public float maxHp = 10;
    public float Hp;
    //Slider
    public Slider slider;

    [Space(15)]

    [Header("各エフェクト")]
    //Efect
    [SerializeField]
    [Tooltip("被ダメージエフェクト")]
    private ParticleSystem Damageparticle;

    [Space(15)]

    [Header("各サウンドエフェクト")]
    //SoundEfect
    //ならすサウンドエフェクトを入れる変数
    [Tooltip("被ダメージSE")]
    public AudioClip Damagesound;
    AudioSource audioSource;

    private Animator m_Animator;
    private Vector3 location;

    void Start()
    {
        //Sliderを最大にする。
        slider.value = 10;
        //HPを最大HPと同じ値に。
        Hp = maxHp;
        //コンポーネント取得
        audioSource = GetComponent<AudioSource>();

        m_Animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown("down"))
        {


            ////Sound1を鳴らす
            //audioSource.PlayOneShot(sound1);

            //m_Animator.SetTrigger("Hit");

            //色を赤くする
            //gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            //0.5秒後にvoid backを実行
            //Invoke("back", 0.2f);

        }

        else if (Input.GetKeyDown("up"))
        {
            m_Animator.SetTrigger("Attack");
        }

        //歩行アニメーション(後々条件とか増やして同時キー入力どうするかとか決めた方がいいと思います)
        if (Input.GetKey("w"))
        {
            m_Animator.SetBool("walkForward", true);

        }
        else
        {
            m_Animator.SetBool("walkForward", false);
        }

        if (Input.GetKey("a"))
        {
            m_Animator.SetBool("Left", true);

        }
        else
        {
            m_Animator.SetBool("Left", false);
        }

        if (Input.GetKey("d"))
        {
            m_Animator.SetBool("Right", true);

        }
        else
        {
            m_Animator.SetBool("Right", false);
        }

        if (Input.GetKey("s"))
        {
            m_Animator.SetBool("Back", true);

        }
        else
        {
            m_Animator.SetBool("Back", false);
        }
    }
    void back()
    {
        //色を元に戻す
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "PlayerAttackArea")
        {
            //HPから1を引く
            Hp = Hp - 1;

            //HPをSliderに反映 ※HPとvalueの最大値が一致してないとうまく減らないので注意
            slider.value = Hp;

            // パーティクルシステムのインスタンスを生成
            ParticleSystem newParticle = Instantiate(Damageparticle);
            // 当たったオブジェクトと最も近い位置を取得する
            Vector3 closestPoint = collider.ClosestPoint(location);
            //最も近い場所にパーティクルを生成
            newParticle.transform.position = closestPoint;
            // パーティクルを発生させる
            newParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(newParticle.gameObject, 1.0f);

            //Sound1を鳴らす
            audioSource.PlayOneShot(Damagesound);

            m_Animator.SetTrigger("Hit");
        }

        Debug.Log(collider.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

    }

    public float CheckHPPercentage() {
        return (Hp / maxHp) * 100;
    }
}