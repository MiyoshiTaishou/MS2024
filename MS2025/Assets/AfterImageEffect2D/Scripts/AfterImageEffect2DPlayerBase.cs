//  AfterImageEffect2DPlayerBase.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AIE2D {
  
  /// <summary>
  /// マテリアルの設定の種類(SameTarget : 対象のレンダラーと同じにする、Collective : 全部同じのを設定する、Individual : 個別に設定する)
  /// </summary>
  public enum MaterialType {
    SameTarget, Collective, Individual
  }

  /// <summary>
  /// 残像を生成、再生するクラスのベース(親)クラス
  /// </summary>
  public class AfterImageEffect2DPlayerBase : MonoBehaviour {

    //残像を生成する親オブジェクト
    protected GameObject _afterImagerParent = null;

    //有効かどうか
    public bool IsActive{ get { return _afterImagerParent != null && _afterImagerParent.activeSelf; }}

    //残像を付ける対象のレンダラー
    [SerializeField]
    protected SpriteRenderer _targetSpriteRenderer = null;

    //残像の数
    [SerializeField]
    protected int _afterImageNum = 2;

    //TimeScaleを無視するか
    [SerializeField]
    protected bool _ignoreTimeScale = false;

    //残像の色と、色を全部同じにするかのフラグ
    [SerializeField]
    protected List<Color> _colorList = new List<Color>() { };

    [SerializeField]
    protected bool _isSameColor = true;

    //マテリアルの設定
    [SerializeField]
    private MaterialType _materialType = MaterialType.SameTarget;

    [SerializeField]
    private List<Material> _materialList = new List<Material>();

    //=================================================================================
    //初期化
    //=================================================================================

    //コンポーネントをAddした時に呼ばれる
    private void Reset() {
      //対象のSpriteRendererを取得
      _targetSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 残像を生成する(既に残像がある場は削除する)
    /// </summary>
    public void CreateAfterImage(SpriteRenderer spriteRenderer, int afterImageNum = 2, bool ignoreTimeScale = false) {
      DestroyAfterImageIfneeded();

      _targetSpriteRenderer = spriteRenderer;
      _afterImageNum        = afterImageNum;
      _ignoreTimeScale      = ignoreTimeScale;

      Init();
    }

    //初期化
    protected virtual void Init() {
      //残像を生成する親オブジェクトを生成
      _afterImagerParent = new GameObject(_targetSpriteRenderer.gameObject.name + "AfterImageEffect");
      _afterImagerParent.transform.SetParent(_targetSpriteRenderer.transform.parent);
    }

    //=================================================================================
    //残像の色
    //=================================================================================

    /// <summary>
    /// 指定したの番号の残像の色を取得する
    /// </summary>
    public Color GetColor(int no) {
      //全部同じ色にする場合は、常に最初の色を返す
      if (_isSameColor) {
        no = 0;
      }

      //指定された番号より設定された色が少ない場合、デフォルトの色を設定する
      while (_colorList.Count <= no) {
        _colorList.Add(new Color(0.5f, 0.5f, 0.5f, 0.5f));
      }

      return _colorList[no];
    }

    /// <summary>
    /// 残像の色を設定する(全部同じ色にする)、既に同じ色が設定されている場合はスルー
    /// </summary>
    public bool SetColorIfneeded(Color color) {
      if (_isSameColor && Equals(color, GetColor(0))) {
        return false;
      }

      _isSameColor  = true;
      _colorList[0] = color;

      for (int i = 0; i < _afterImageNum; i++) {
        SetColor(i, color);
      }

      return true;
    }

    /// <summary>
    /// 残像の色をListで設定する(全部違う色にする)、既に同じ色が設定されている場合はスルー
    /// </summary>
    public virtual bool SetColorListIfneeded(List<Color> colorList) {
      if (!_isSameColor && _colorList.SequenceEqual(colorList)) {
        return false;
      }

      _isSameColor = false;
      _colorList   = new List<Color>(colorList);

      for (int i = 0; i < _afterImageNum; i++) {
        SetColor(i, GetColor(i));
      }

      return true;
    }

    //各残像に色を設定する
    protected virtual void SetColor(int afterImageNo, Color color){}

    //=================================================================================
    //残像のマテリアル
    //=================================================================================

    /// <summary>
    /// 指定したの番号の残像のマテリアルを取得する
    /// </summary>
    public Material GetMaterial(int no) {
      //対象のレンダラーと同じ場合(レンダラーが設定されていない場合はnull)
      if (_materialType == MaterialType.SameTarget) {
        return _targetSpriteRenderer == null ? null : _targetSpriteRenderer.sharedMaterial;
      }
      //全部同じ色にする場合は、常に最初のマテリアルを返す
      if (_materialType == MaterialType.Collective) {
        no = 0;
      }

      //指定された番号より設定されたマテリアルが少ない場合、デフォルトのマテリアルを設定する
      while (_materialList.Count <= no) {
        _materialList.Add(GetDefultMateril());
      }

      if (_materialList[no] == null) {
        _materialList[no] = GetDefultMateril();
      }

      return _materialList[no];
    }

    //デフォルトのマテリアルを取得
    private Material GetDefultMateril() {
#if UNITY_EDITOR
      return UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
#else
      return Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
#endif
    }

    /// <summary>
    /// 残像のマテリアルを設定する(全部同じマテリアルにする)、nullを設定すると対象のレンダラーと同じする
    /// 既に同じマテリアルが設定されている場合はスルー
    /// </summary>
    public bool SetMaterialIfneeded(Material material) {
      if (_materialType == MaterialType.SameTarget && material == null) {
        return false;
      }
      if (_materialType == MaterialType.Collective && Equals(material, GetMaterial(0))) {
        return false;
      }

      if (material == null) {
        _materialType = MaterialType.SameTarget;
      }
      else {
        _materialType = MaterialType.Collective;
        if(_materialList.Count == 0){
          _materialList.Add(material);
        }
        else{
          _materialList[0] = material;
        }
      }

      for (int i = 0; i < _afterImageNum; i++) {
        SetMaterial(i, material);
      }

      return true;
    }

    /// <summary>
    /// 残像のマテリアルをListで設定する(全部違うマテリアルにする)、既に同じマテリアルが設定されている場合はスルー
    /// </summary>
    public virtual bool SetMaterialListIfneeded(List<Material> materialList) {
      if (_materialType == MaterialType.Individual && _materialList.SequenceEqual(materialList)) {
        return false;
      }

      _materialType = MaterialType.Individual;
      _materialList = new List<Material>(materialList);

      for (int i = 0; i < _afterImageNum; i++) {
        SetMaterial(i, GetMaterial(i));
      }

      return true;
    }

    //各残像にマテリアル色を設定する
    protected virtual void SetMaterial(int afterImageNo, Material material) { }

    //=================================================================================
    //状態切り替え
    //=================================================================================

    /// <summary>
    /// 状態を切り替える
    /// </summary>>
    public void SetActive(bool isActive) {
      if(_afterImagerParent == null){
        return;
      }

      //状態が変わった
      if (IsActive != isActive) {
        if(isActive){
          OnActive();
        }
        _afterImagerParent.SetActive(isActive);
      }

    }

    //無効状態から有効になった
    protected virtual void OnActive(){}

    //=================================================================================
    //更新
    //=================================================================================

    private void LateUpdate() {
      //残像の親が表示されていない時は更新しない
      if (_afterImagerParent == null || !_afterImagerParent.activeSelf) {
        return;
      }
      UpdateState();
    }

    //状態を更新
    protected virtual void UpdateState(){}

    //=================================================================================
    //削除
    //=================================================================================

    /// <summary>
    /// 残像があれば削除する
    /// </summary>
    public void DestroyAfterImageIfneeded() {
      if (_afterImagerParent != null) {
        DestroyImmediate(_afterImagerParent);
      }
    }

  }

}