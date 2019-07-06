using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DarkEffect : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        public Transform target;

        [SerializeField]
        public float radius;

        public Vector3 GetScreenPosition(Camera cam)
        {
            return cam.WorldToScreenPoint(target.position);
        }
    }
    public Material _mainMaterial;
    //渐变像素数量
    public int _smoothLength = 20;
    //遮罩混合颜色
    public Color _darkColor = Color.black;
    //目标物体
    public List<Item> _items = new List<Item>();

    public int TargetScreenHeight = 540;

   // protected Material _mainMaterial;
    protected Camera _mainCamera;

    Vector4[] _itemDatas;
    Item _tmpItem;
    Vector4 _tmpVt;
    Vector3 _tmpPos;
    int _tmpScreenHeight;
    public void setRoleDis(float val)
    {
        _items[0].radius = val;
    }
    private void OnEnable()
    {
        if(_mainMaterial==null)
        {
            _mainMaterial = new Material(Shader.Find("Custom/MaskShader"));
        }

        _mainCamera = GetComponent<Camera>(); 
    }
    /// <summary>
    /// 增加目标点
    /// </summary>
    /// <param name="trs">物体的transform</param>
    /// <param name="Range"></param>
    public void addLightItem(Transform trs,int Range)
    {
        if(_items.Count>=9)
        {
            return;
        }
        if(continuesLightItem(trs))
        {
            return;
        }
        Item it = new Item();
        it.target = trs;
        it.radius = Range;
        _items.Add(it);
        Shader s = _mainMaterial.shader;
        _mainMaterial.shader = null;
        _mainMaterial.shader = s;
    }

    public bool continuesLightItem(Transform trs)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].target == trs)
            {
                return true;
            }
        }
        return false;
    }

    public void removeLightItem(Transform trs)
    {
        for(int i=0;i<_items.Count;i++)
        {
            if(_items[i].target == trs)
            {
                _items.RemoveAt(i);
                break;
            }
        }
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_items.Count > 0)
        {
            float scale = (float)Screen.height / (float)TargetScreenHeight;

            if (_itemDatas == null || _itemDatas.Length != _items.Count)
            {
                _itemDatas = new Vector4[_items.Count];
            }

            _tmpScreenHeight = Screen.height;

            for (int i = 0; i < _items.Count; i++)
            {
                _tmpItem = _items[i];
                _tmpPos = _tmpItem.GetScreenPosition(_mainCamera);
               
                _tmpVt.x = _tmpPos.x;

                if(Application.platform==RuntimePlatform.Android)
                {
                    _tmpVt.y = _tmpPos.y;
                }
                else
                {
                    _tmpVt.y = _tmpScreenHeight - _tmpPos.y;
                }
                
                _tmpVt.z = _tmpItem.radius * scale;
                _tmpVt.w = 0;
                //Debug.Log("_tmpVt:"+i+"->"+ _tmpVt);
                _itemDatas[i] = _tmpVt;
            }
            
            _mainMaterial.SetInt("_SmoothLength", (int)(_smoothLength * scale));
            _mainMaterial.SetColor("_DarkColor", _darkColor);
            _mainMaterial.SetInt("_ItemCnt", _itemDatas.Length);
            _mainMaterial.SetVectorArray("_Item", _itemDatas);
        }
        Graphics.Blit(source, destination, _mainMaterial);
    }
}