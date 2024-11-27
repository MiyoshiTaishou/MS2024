//  StaticAfterImageEffect2DPlayer.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;
using System.Collections.Generic;

namespace AIE2D {

  /// <summary>
  /// 静止している残像を生成、再生するクラス
  /// </summary>
  public class StaticAfterImageEffect2DPlayer : AfterImageEffect2DPlayerBase {

    //残像のプール
    private ObjectPool _afterImagePool = null;

    //生成した残像のList
    private List<StaticAfterImageEffect2D> _afterImageList = new List<StaticAfterImageEffect2D>();

    //残像の表示間隔と表示時間、消える時にフェードにかかる時間
    [SerializeField]
    private float _span = 0.05f, _duraction = 0.1f, _fadeTime = 0.05f;

    //色とマテリアルの個数
    [SerializeField]
    private int _colorNum = 1, _materialNum = 1;

    //次の残像の色とマテリアルの番号
    private int _nextColorNo = 0, _nextMaterialNo = 0;

    //経過時間
    private float _elapsedTime = 0;

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
    public void CreateAfterImage() {
      CreateAfterImage(_targetSpriteRenderer, _afterImageNum, _span, _duraction, _fadeTime, _ignoreTimeScale);
    }

    /// <summary>
    /// 残像を生成する(既に残像がある場は削除する)
    /// </summary>
    public void CreateAfterImage(SpriteRenderer spriteRenderer, int afterImageNum = 2, float span = 0.05f, float duraction = 0.1f, float fadeTime = 0.05f, bool ignoreTimeScale = false) {
      _span      = span;
      _duraction = duraction;
      _fadeTime  = fadeTime;

      _afterImageList.Clear();

      base.CreateAfterImage(spriteRenderer, afterImageNum, ignoreTimeScale);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Init() {
      base.Init();

      //残像の複製元を作成
      StaticAfterImageEffect2D afterImage = new GameObject("AfterImage0").AddComponent<StaticAfterImageEffect2D>();
      afterImage.CreateSpriteRenderer(_targetSpriteRenderer);

      _afterImagePool = _afterImagerParent.AddComponent<ObjectPool>();
      _afterImagePool.CreateInitialPool(afterImage.gameObject);

      _nextColorNo    = 0;
      _nextMaterialNo = 0;

      _elapsedTime = 0;
    }

    //=================================================================================
    //破棄
    //=================================================================================

    //影を破棄する
    private void ReleaseAfterImage(StaticAfterImageEffect2D target){
      _afterImagePool.Release(target.gameObject);
      _afterImageList.Remove(target);
    }

    //=================================================================================
    //残像の色、マテリアル
    //=================================================================================

    /// <summary>
    /// 残像の色をListで設定する(全部違う色にする)、既に同じ色が設定されている場合はスルー
    /// </summary>
    public override bool SetColorListIfneeded(List<Color> colorList) {
      if(!base.SetColorListIfneeded(colorList)){
        return false;
      }
      _colorNum = colorList.Count;
      return true;
    }

    /// <summary>
    /// 残像のマテリアルをListで設定する(全部違うマテリアルにする)、既に同じマテリアルが設定されている場合はスルー
    /// </summary>
    public override bool SetMaterialListIfneeded(List<Material> materialList) {
      if (!base.SetMaterialListIfneeded(materialList)) {
        return false;
      }
      _materialNum = materialList.Count;
      return true;
    }

    //=================================================================================
    //状態切り替え
    //=================================================================================

    //無効状態から有効になった
    protected override void OnActive() {
      while (_afterImageList.Count > 0) {
        ReleaseAfterImage(_afterImageList[0]);
      }
    }

    //=================================================================================
    //更新
    //=================================================================================

    //状態を更新
    protected override void UpdateState() {
      //経過時間加算、残像を生成するか判定
      _elapsedTime += _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
      if(_elapsedTime < _span){
        return;
      }
      _elapsedTime = 0;

      //現在レンダラーの状態から残像の情報を作成
      AfterImageEffect2DInfo info = new AfterImageEffect2DInfo(_targetSpriteRenderer);

      //残像生成、初期化
      StaticAfterImageEffect2D afterImage = _afterImagePool.Get<StaticAfterImageEffect2D>();
      afterImage.Init(info, GetColor(_nextColorNo), GetMaterial(_nextMaterialNo), _duraction, _fadeTime, _ignoreTimeScale, ReleaseAfterImage);
      afterImage.transform.position = _targetSpriteRenderer.transform.position;

      _afterImageList.Add(afterImage);

      //次の色とマテリアルの番号を変更
      _nextColorNo++;
      _nextMaterialNo++;

      if(_nextColorNo >= _colorNum){
        _nextColorNo = 0;
      }
      if (_nextMaterialNo >= _materialNum) {
        _nextMaterialNo = 0;
      }
    }

  }

}