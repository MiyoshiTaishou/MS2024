//  AIE2DSampleScripts.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AIE2D;

/// <summary>
/// AIE2Dのサンプル(サンプルのためかなりラフに作っており、実際のゲームで使う事はオススメ出来ません)
/// </summary>
public class AIE2DSample : MonoBehaviour {

  //残像再生用クラスとアニメーター
  private AfterImageEffect2DPlayerBase _player = null;

  //アニメーター
  private Animator _animator = null;
  private int _animatorHashX = Animator.StringToHash("x"), _animatorHashY = Animator.StringToHash("y");

  //速度
  [SerializeField]
  private float _moveSpeed = 4, _dashSpeed = 8, _scalingSpeed = 4, _rotationSpeed = 500;

  //色
  [SerializeField]
  private List<Color> _colorList = new List<Color>();
  private int _colorNo = 0;

  //マテリアル
  [SerializeField]
  private List<Material> _materialList = new List<Material>();
  private int _materialNo = 0;

  //=================================================================================
  //初期化
  //=================================================================================

  private void Awake() {
    _player   = gameObject.GetComponent<AfterImageEffect2DPlayerBase>();
    _animator = gameObject.GetComponent<Animator>();
  }

  //=================================================================================
  //更新
  //=================================================================================

  private void Update() {
    //XとYの入力を取得し、移動距離を設定
    Vector3 moveDistance = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    if (_animator != null && moveDistance.magnitude > 0) {
      _animator.SetFloat(_animatorHashX, moveDistance.x);
      _animator.SetFloat(_animatorHashY, moveDistance.y);
    }

    //ダッシュボタンを押しているかで速度を変えるように
    if(Input.GetKey(KeyCode.Space)){
      moveDistance *= _dashSpeed;
    }
    else{
      moveDistance *= _moveSpeed;
    }

    //移動処理
    transform.localPosition += moveDistance * Time.deltaTime;

    //スケール変更
    float scale = transform.localScale.x;

    if (Input.GetKey(KeyCode.E)){
      scale += Time.deltaTime * _scalingSpeed;
    }
    else if (Input.GetKey(KeyCode.Q)){
      scale -= Time.deltaTime * _scalingSpeed;
    }

    scale = Mathf.Clamp(scale, 0, 5);
    transform.localScale = new Vector3(scale, scale, scale);

    //回転
    Vector3 rotation = transform.localRotation.eulerAngles;

    if (Input.GetKey(KeyCode.C)) {
      rotation.z += Time.deltaTime * _rotationSpeed;
    }
    else if (Input.GetKey(KeyCode.Z)) {
      rotation.z -= Time.deltaTime * _rotationSpeed;
    }

    transform.localRotation = Quaternion.Euler(rotation);

    //残像の表示切り替え
    if (Input.GetKeyDown(KeyCode.Return) && _player != null) {
      _player.SetActive(!_player.IsActive);
    }

    //残像の色切り替え
    if (Input.GetKeyDown(KeyCode.K) && _colorList.Count > 0) {
      if(_colorNo >= _colorList.Count){
        _colorNo = 0;
      }
      _player.SetColorIfneeded(_colorList[_colorNo]);
      _colorNo++;
    }

    //残像のマテリアル切り替え
    if (Input.GetKeyDown(KeyCode.L) && _materialList.Count > 0) {
      if (_materialNo >= _materialList.Count) {
        _materialNo = 0;
      }
      _player.SetMaterialIfneeded(_materialList[_materialNo]);
      _materialNo++;
    }

    //残像の削除
    if (Input.GetKeyDown(KeyCode.Y)) {
      Destroy(gameObject);
    }
    if (Input.GetKeyDown(KeyCode.U)) {
      _player.DestroyAfterImageIfneeded();
      Destroy(gameObject);
    }
  }

}