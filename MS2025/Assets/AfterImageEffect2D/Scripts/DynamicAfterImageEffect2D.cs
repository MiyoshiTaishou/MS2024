//  DynamicAfterImageEffect2D.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System.Collections.Generic;
using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// 動く残像
  /// </summary>
  public class DynamicAfterImageEffect2D : AfterImageEffect2DBase {

    //残像のフレーム間隔、残像を表示する距離の範囲
    private float _interval, _distanceRange;

    //残像を生成するための情報のリスト
    private List<DynamicAfterImageEffect2DInfo> _infoList = new List<DynamicAfterImageEffect2DInfo>();

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Color color, Material material, float interval, float distanceRange, bool ignoreTimeScale){
      base.Init(color, material, ignoreTimeScale);
      _interval      = interval;
      _distanceRange = distanceRange;
    }

    /// <summary>
    /// 残像を生成するための情報を初期化する
    /// </summary>
    public void InitInfo(DynamicAfterImageEffect2DInfo initialInfo){
      _infoList.Clear();
      _infoList.Add(initialInfo);

      initialInfo.ApplySprite(_spriteRenderer);
      initialInfo.ApplyTransform(transform);
    }

    //=================================================================================
    //更新
    //=================================================================================

    private void LateUpdate() {
      if(_originalSpriteRenderer != null){
        return;
      }
      //元のレンダラーがなくなった場合は情報にnullを設定し、現在持っている情報の所まで更新するように
      AddInfoAndUpdateState(null);
    }

    /// <summary>
    /// DynamicAfterImageEffect2DInfoを追加し、状態を更新する
    /// </summary>
    public void AddInfoAndUpdateState(DynamicAfterImageEffect2DInfo info){
      if(info != null){
        _infoList.Add(info);
      }

      //レイヤー更新(後のやつほど後ろに行くように)
      if(_originalSpriteRenderer != null){
        _spriteRenderer.sortingOrder = _originalSpriteRenderer.sortingOrder - Mathf.RoundToInt(1 + _interval * 100);
      }

      //現在の位置に該当する情報の時間を計算、その時間より1つ後の情報番号取得
      float targetTime = (_ignoreTimeScale ? Time.unscaledTime : Time.time) - _interval;
      int afterInfoNo = 0;
      for (int i = 0; i < _infoList.Count; i++) {
        afterInfoNo = i;
        if(_infoList[i].GenerationTime >= targetTime){
          break;
        }
      }

      //前後の情報から位置などを調整
      int beforeInfoNo = Mathf.Max(afterInfoNo - 1, 0);
      DynamicAfterImageEffect2DInfo.AdjustTransform(_spriteRenderer, _infoList[beforeInfoNo], _infoList[afterInfoNo], _ignoreTimeScale);

      //本体と位置が近い場合は表示しないように
      if (_originalSpriteRenderer != null && Vector3.Distance(transform.position, _originalSpriteRenderer.transform.position) < _distanceRange) {
        _spriteRenderer.enabled = false;
      }
      else if(!_spriteRenderer.enabled){
        _spriteRenderer.enabled = true;
      }

      //不要な情報を破棄
      for (int i = 0; i < beforeInfoNo; i++) {
        _infoList.RemoveAt(0);
      }

      //情報が設定されず、残りの情報もなくなったら残像を無効にする
      if(info == null && _infoList.Count <= 2){
        gameObject.SetActive(false);
      }
    }

  }

}