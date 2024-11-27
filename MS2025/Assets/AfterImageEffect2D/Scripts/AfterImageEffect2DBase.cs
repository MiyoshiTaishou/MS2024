//  AfterImageEffect2DBase.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;

namespace AIE2D {

  /// <summary>
  /// 残像のベース(親)クラス
  /// </summary>
  public class AfterImageEffect2DBase : MonoBehaviour {

    //残像とその元のレンダラー
    [SerializeField]
    protected SpriteRenderer _originalSpriteRenderer = null, _spriteRenderer = null;

    //TimeScaleを無視するか
    protected bool _ignoreTimeScale = false;

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// SpriteRenderer生成
    /// </summary>
    public void CreateSpriteRenderer(SpriteRenderer originalSpriteRenderer) {
      _originalSpriteRenderer = originalSpriteRenderer;
      _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

      _spriteRenderer.sortingLayerName = _originalSpriteRenderer.sortingLayerName;

      transform.position = _spriteRenderer.transform.position;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Color color, Material material, bool ignoreTimeScale) {
      SetColor(color);
      SetMaterial(material);

      _ignoreTimeScale = ignoreTimeScale;
    }

    //=================================================================================
    //設定
    //=================================================================================

    /// <summary>
    /// 残像の色を設定する
    /// </summary>
    public void SetColor(Color color) {
      _spriteRenderer.color = color;
    }

    /// <summary>
    /// 残像マテリアルを設定する
    /// </summary>
    public void SetMaterial(Material material) {
      _spriteRenderer.sharedMaterial = material;
    }

  }

}