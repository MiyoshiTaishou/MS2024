using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    //最大HPと現在のHP。
    public float maxHp = 10;
    float Hp;
    //Slider
    public Slider slider;

    //Efect
    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
    private ParticleSystem particle;

    //SoundEfect
    //ならすサウンドエフェクトを入れる変数
    public AudioClip sound1;
    AudioSource audioSource;

    private Animator m_Animator;

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
        //if (Input.GetKeyDown("down"))
        //{
        //    //HPから1を引く
        //    Hp = Hp - 1;

        //    //HPをSliderに反映 ※HPとvalueの最大値が一致してないとうまく減らないので注意
        //    slider.value = Hp;

        //    // パーティクルシステムのインスタンスを生成
        //    ParticleSystem newParticle = Instantiate(particle);
        //    // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする
        //    newParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 4.5f, this.transform.position.z - 1);
        //    // パーティクルを発生させる
        //    newParticle.Play();
        //    // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
        //    Destroy(newParticle.gameObject, 1.0f);

        //    //Sound1を鳴らす
        //    audioSource.PlayOneShot(sound1);

        //    m_Animator.SetTrigger("Hit");

        //    //色を赤くする
        //    //gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        //    //0.5秒後にvoid backを実行
        //    //Invoke("back", 0.2f);

        //}

        //else if (Input.GetKeyDown("up"))
        //{
        //    m_Animator.SetTrigger("Attack");
        //}

        ////歩行アニメーション(後々条件とか増やして同時キー入力どうするかとか決めた方がいいと思います)
        //if (Input.GetKey("w"))
        //{
        //    m_Animator.SetBool("walkForward", true);

        //}
        //else
        //{
        //    m_Animator.SetBool("walkForward", false);
        //}

        //if (Input.GetKey("a"))
        //{
        //    m_Animator.SetBool("Left", true);

        //}
        //else
        //{
        //    m_Animator.SetBool("Left", false);
        //}

        //if (Input.GetKey("d"))
        //{
        //    m_Animator.SetBool("Right", true);

        //}
        //else
        //{
        //    m_Animator.SetBool("Right", false);
        //}

        //if (Input.GetKey("s"))
        //{
        //    m_Animator.SetBool("Back", true);

        //}
        //else
        //{
        //    m_Animator.SetBool("Back", false);
        //}


        void back()
        {
            //色を元に戻す
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }

}