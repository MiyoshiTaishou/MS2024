//  AfterImageEffect2DInfo.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// 残像を生成するのに必要な情報をまとめたクラス
  /// </summary>
  public class AfterImageEffect2DInfo {

    //位置、スケール、角度
    private Vector3 _position = Vector3.zero, _localScale = Vector3.zero, _rotation = Vector3.zero;
    public  Vector3 Position   { get { return _position; } }
    public  Vector3 LocalScale { get { return _localScale; } }
    public  Vector3 Rotation   { get { return _rotation; } }

    //画像
    private Sprite _sprite = null;

    //反転設定
    private bool _flipX = false, _flipY = false;

    //=================================================================================
    //初期化
    //=================================================================================

    public AfterImageEffect2DInfo(SpriteRenderer spriteRenderer) {
      _position   = spriteRenderer.transform.position;
      _localScale = spriteRenderer.transform.localScale;
      _rotation   = spriteRenderer.transform.rotation.eulerAngles;

      _sprite = spriteRenderer.sprite;
      _flipX  = spriteRenderer.flipX;
      _flipY  = spriteRenderer.flipY;
    }

    //=================================================================================
    //反映
    //=================================================================================

    /// <summary>
    /// 保存されているSpriteとFlipを反映する
    /// </summary>
    public void ApplySprite(SpriteRenderer spriteRenderer) {
      spriteRenderer.sprite = _sprite;
      spriteRenderer.flipX  = _flipX;
      spriteRenderer.flipY  = _flipY;
    }

    /// <summary>
    /// 保存されているTransformの情報(位置、スケール、角度)を反映する
    /// </summary>
    public void ApplyTransform(Transform transform) {
      transform.position   = _position;
      transform.localScale = _localScale;
      transform.rotation   = Quaternion.Euler(_rotation);
    }

  }

}