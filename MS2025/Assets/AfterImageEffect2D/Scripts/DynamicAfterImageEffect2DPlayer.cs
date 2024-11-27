//  DynamicAfterImageEffect2DPlayer.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;
using System.Collections.Generic;

namespace AIE2D{
  
  /// <summary>
  /// 動く残像を生成、再生するクラス
  /// </summary>
  public class DynamicAfterImageEffect2DPlayer : AfterImageEffect2DPlayerBase {

    //生成した残像のList
    private List<DynamicAfterImageEffect2D> _afterImageList = new List<DynamicAfterImageEffect2D>();

    //残像の間の長さ(秒)、残像を表示する距離の範囲
    [SerializeField]
    private float _interval = 0.02f, _distanceRange = 0.1f;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake() {
      if (_targetSpriteRenderer != null) {
        CreateAfterImage();
      }
    }

    /// <summary>
    /// 残像を生成する(既に残像がある場は削除する)
    /// </summary>
    public void CreateAfterImage(){
      CreateAfterImage(_targetSpriteRenderer, _interval, _afterImageNum, _distanceRange, _ignoreTimeScale);
    }

    /// <summary>
    /// 残像を生成する(既に残像がある場は削除する)
    /// </summary>
    public void CreateAfterImage(SpriteRenderer spriteRenderer, float interval = 0.02f, int afterImageNum = 3, float distanceRange = 0.1f, bool ignoreTimeScale = false) {
      _interval      = interval;
      _distanceRange = distanceRange;

      _afterImageList.Clear();

      base.CreateAfterImage(spriteRenderer, afterImageNum, ignoreTimeScale);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Init() {
      base.Init();

      //残像の複製元を作成
      DynamicAfterImageEffect2D afterImage = new GameObject("AfterImage0").AddComponent<DynamicAfterImageEffect2D>();
      afterImage.transform.SetParent(_afterImagerParent.transform);
      afterImage.CreateSpriteRenderer(_targetSpriteRenderer);
      _afterImageList.Add(afterImage);

      //残像のコピー作成
      for (int i = 0; i < _afterImageNum - 1; i++) {
        _afterImageList.Add(Instantiate(afterImage.gameObject, _afterImagerParent.transform).GetComponent<DynamicAfterImageEffect2D>());
      }

      //残像の初期化
      for (int i = 0; i < _afterImageList.Count; i++) {
        _afterImageList[i].Init(GetColor(i), GetMaterial(i), _interval * (i + 1), _distanceRange, _ignoreTimeScale);
      }

    }

    //=================================================================================
    //残像の色、マテリアル
    //=================================================================================

    //各残像に色を設定する
    protected override void SetColor(int afterImageNo, Color color) {
      if(_afterImageList.Count > afterImageNo){
        _afterImageList[afterImageNo].SetColor(color);
      }
    }

    //各残像にマテリアル色を設定する
    protected override void SetMaterial(int afterImageNo, Material material) {
      if (_afterImageList.Count > afterImageNo) {
        _afterImageList[afterImageNo].SetMaterial(material);
      }
    }

    //=================================================================================
    //状態切り替え
    //=================================================================================

    //無効状態から有効になった
    protected override void OnActive() {
      DynamicAfterImageEffect2DInfo info = new DynamicAfterImageEffect2DInfo(_targetSpriteRenderer, _ignoreTimeScale);
      foreach (DynamicAfterImageEffect2D afterImage in _afterImageList) {
        afterImage.InitInfo(info);
      }
    }

    //=================================================================================
    //更新
    //=================================================================================

    //状態を更新
    protected override void UpdateState() {
      //現在レンダラーの状態から残像の情報を作成
      DynamicAfterImageEffect2DInfo info = new DynamicAfterImageEffect2DInfo(_targetSpriteRenderer, _ignoreTimeScale);

      //各残像に情報を設定し、状態を更新
      for (int i = 0; i < _afterImageList.Count; i++) {
        _afterImageList[i].AddInfoAndUpdateState(info);
      }
    }

  }

}