//  ObjectPool.cs
//  http://kan-kikuchi.hatenablog.com/entry/AfterImageEffect2D
//
//  Created by kan.kikuchi on 2019.01.17.

using UnityEngine;
using System.Collections.Generic;


namespace AIE2D {

  /// <summary>
  /// GameObjectを再利用するためのクラス
  /// </summary>
  public class ObjectPool : MonoBehaviour {

    private List<GameObject> _objectList = new List<GameObject>();

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// プールするオブジェクトを指定して、初期プールを作成
    /// </summary>
    public void CreateInitialPool(GameObject orizinal, int objectNum = 1) {
      //オリジナルをプールに追加し、後はそれをコピー
      orizinal.SetActive(false);
      Release(orizinal);

      List<GameObject> copyObjectList = new List<GameObject>();
      for (int i = 0; i < objectNum; i++) {
        copyObjectList.Add(Get());
      }
      foreach (GameObject copy in copyObjectList) {
        Release(copy);
      }
    }

    //=================================================================================
    //取得
    //=================================================================================

    /// <summary>
    /// オブジェクトを取得
    /// </summary>
    public GameObject Get() {
      GameObject target = null;

      //1つしかなければ複製、あればリストから取得
      if (_objectList.Count == 1) {
        target = Instantiate(_objectList[0], transform);
      }
      else {
        target = _objectList[_objectList.Count - 1];
        _objectList.Remove(target);
      }

      target.SetActive(true);

      return target;
    }

    /// <summary>
    /// オブジェクトをコンポーネントを指定して取得
    /// </summary>
    public T Get<T>() where T : Component {
      return Get().GetComponent<T>();
    }

    //=================================================================================
    //破棄
    //=================================================================================

    /// <summary>
    /// 使用したオブジェクトを戻す
    /// </summary>
    public void Release(GameObject target) {
      //非表示にし、プールへ追加
      target.SetActive(false);
      target.transform.SetParent(transform);

      _objectList.Add(target);
    }

  }

}