//  StaticAfterImageEffect2D.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System;
using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// 静止している残像
  /// </summary>
  public class StaticAfterImageEffect2D : AfterImageEffect2DBase {

    //残像の表示時間、消える時にフェードにかかる時間、経過時間
    private float _duraction, _fadeTime, _elapsedTime;

    //初期状態の不透明度
    private float _initialAlpha;

    //表示が終わった時に破棄するためのコールバック
    private Action<StaticAfterImageEffect2D> _releaseAction = null;

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(AfterImageEffect2DInfo info, Color color, Material material, float duraction, float fadeTime, bool ignoreTimeScale, Action<StaticAfterImageEffect2D> releaseAction) {
      base.Init(color, material, ignoreTimeScale);

      info.ApplySprite(_spriteRenderer);
      info.ApplyTransform(transform);

      _initialAlpha = _spriteRenderer.color.a;

      _duraction     = duraction;
      _fadeTime      = fadeTime;
      _releaseAction = releaseAction;

      _elapsedTime = 0;

      UpdateState();
    }

    //=================================================================================
    //更新
    //=================================================================================

    private void LateUpdate() {
      _elapsedTime += _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
      UpdateState();
    }

    private void UpdateState(){
      //レイヤー更新(後のやつほど後ろに行くように)
      if(_originalSpriteRenderer != null){
        _spriteRenderer.sortingOrder = _originalSpriteRenderer.sortingOrder - Mathf.RoundToInt(1 + _elapsedTime * 100);
      }

      //不透明度更新
      if(_elapsedTime > _duraction){
        Color color = _spriteRenderer.color;
        color.a = _initialAlpha * (1f - (_elapsedTime - _duraction) / _fadeTime);
        _spriteRenderer.color = color;
      }

      //終了判定
      if(_elapsedTime >= _duraction + _fadeTime){
        _releaseAction(this);
      }
    }

  }


}