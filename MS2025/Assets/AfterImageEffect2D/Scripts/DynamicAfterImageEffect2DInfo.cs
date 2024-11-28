//  DynamicAfterImageEffect2DInfo.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// 動く残像を生成するのに必要な情報をまとめたクラス
  /// </summary>
  public class DynamicAfterImageEffect2DInfo : AfterImageEffect2DInfo {

    //生成した時の時間
    private float _generationTime;
    public  float  GenerationTime { get { return _generationTime; } }

    //=================================================================================
    //初期化
    //=================================================================================

    public DynamicAfterImageEffect2DInfo(SpriteRenderer spriteRenderer, bool ignoreTimeScale) : base(spriteRenderer) {
      _generationTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
    }

    //=================================================================================
    //反映
    //=================================================================================

    /// <summary>
    /// 前後の情報から現在の状態を計算し、調整する
    /// </summary>
    public static void AdjustTransform(SpriteRenderer spriteRenderer, DynamicAfterImageEffect2DInfo beforeInfo, DynamicAfterImageEffect2DInfo aftereInfo, bool ignoreTimeScale) {

      //現在の時を取得し、前後の情報の時間との差を計算
      float currentTime       = ignoreTimeScale ? Time.unscaledTime : Time.time;
      float befonInfoTimeDiff = currentTime - beforeInfo.GenerationTime, afterInfoTimeDiff = currentTime - aftereInfo.GenerationTime;
      float totalTimeDiff     = befonInfoTimeDiff + afterInfoTimeDiff;

      //前後の情報とも今と同じ時間なら表示しない
      if (totalTimeDiff <= 0) {
        spriteRenderer.enabled = false;
        return;
      }

      //前後の情報が今の時間とどれだけ近いのかを割合で計算
      float beforeInfoRate = befonInfoTimeDiff / totalTimeDiff, afterInfoRate = afterInfoTimeDiff / totalTimeDiff;

      //前後近い方の画像を表示する
      if (beforeInfoRate > afterInfoRate) {
        beforeInfo.ApplySprite(spriteRenderer);
      }
      else {
        aftereInfo.ApplySprite(spriteRenderer);
      }

      //前後の情報とその割合から現在の状態を調整する
      spriteRenderer.transform.position   = beforeInfo.Position   * beforeInfoRate + aftereInfo.Position   * afterInfoRate;
      spriteRenderer.transform.localScale = beforeInfo.LocalScale * beforeInfoRate + aftereInfo.LocalScale * afterInfoRate;
      spriteRenderer.transform.rotation   = Quaternion.Euler(beforeInfo.Rotation * beforeInfoRate + aftereInfo.Rotation * afterInfoRate);
    }

  }

}