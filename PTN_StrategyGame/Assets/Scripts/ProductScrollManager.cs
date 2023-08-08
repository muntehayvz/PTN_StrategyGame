using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject Content;
    public GameObject NotActiveContent;

    private ScrollRect _scrollRect; 
    
    private RectTransform _contentRectTransform;

    private PointerEventData _pointerEventData;

    public PoolManager Pool;
    
    private int _bottomIndex;
    private int _topIndex;
    
    private void Start()
    {   
        var units = new List<GameObject>();
        for (var i = 0; i < Content.transform.childCount; i++)
        {
            units.Add(Content.transform.GetChild(i).gameObject);
        }
        Pool.SetUnits(units);
        
        _topIndex = Pool.GetObjectCount() - 1;
        
        _contentRectTransform = Content.GetComponent<RectTransform>();
        _scrollRect = GetComponent<ScrollRect>();
        
        StartCoroutine(EndOfFrame());
    }

    private void Update()
    {
        if (_contentRectTransform.sizeDelta.y - _contentRectTransform.localPosition.y < GetComponent<RectTransform>().rect.height*1.1)
        {
            AddToBottom();
        }
        if (_contentRectTransform.localPosition.y < 100) //Make dynamic later
        {
            AddToTop();
        }
        if (_contentRectTransform.localPosition.y > 250) //Make dynamic later
        {
            RemoveFromTop();
        }
        if (_contentRectTransform.sizeDelta.y - _contentRectTransform.localPosition.y > GetComponent<RectTransform>().rect.height*1.3)
        {
            RemoveFromBottom();
        }
        
        //15 top padding, 10 spacing + 90 child's height.. Make dynamic later
    }
    
    private IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        //var topSpacing = _contentRectTransform.GetComponent<GridLayoutGroup>().padding.top;

        var topSpacing = _contentRectTransform.GetComponent<VerticalLayoutGroup>().padding.top;

        var firstChildRTransform = _contentRectTransform.GetChild(0).GetComponent<RectTransform>();
        var lastChildRTransform = _contentRectTransform.GetChild(_contentRectTransform.childCount - 1).GetComponent<RectTransform>();
        var firstChildTop = firstChildRTransform.localPosition.y +
                            firstChildRTransform.sizeDelta.y * firstChildRTransform.localScale.y;
        var lastChildBottom = lastChildRTransform.localPosition.y -
                              lastChildRTransform.sizeDelta.y * lastChildRTransform.localScale.y;
            
        _contentRectTransform.sizeDelta = new Vector2(0, topSpacing + firstChildTop - lastChildBottom);

        StartCoroutine(EndOfFrame());
    }

    private void AddToTop()
    {
        var newGameObject = Pool.GetObjectFromPool(_topIndex);
        newGameObject.GetComponent<RectTransform>().SetParent(Content.GetComponent<RectTransform>(), false);
        newGameObject.GetComponent<RectTransform>().SetSiblingIndex(0);
        newGameObject.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = newGameObject.name;
            
        _topIndex = _topIndex - 1;
        if (_topIndex == -1)
        {
            _topIndex = Pool.GetObjectCount()-1;
        }
            
        _contentRectTransform.localPosition = new Vector2(_contentRectTransform.localPosition.x, _contentRectTransform.localPosition.y + 100); // 100, spacing + child's height Make dynamic later

        if (_pointerEventData != null)
        {
            _scrollRect.OnBeginDrag(_pointerEventData);
        }
    }

    private void AddToBottom()
    {
        var newGameObject = Pool.GetObjectFromPool(_bottomIndex);
        newGameObject.GetComponent<RectTransform>().SetParent(Content.GetComponent<RectTransform>(), false);
        newGameObject.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = newGameObject.name;
            
        _bottomIndex = _bottomIndex + 1;
        if (_bottomIndex == 4)
        {
            _bottomIndex = 0;
        }
    }

    private void RemoveFromTop()
    {
        var child = _contentRectTransform.GetChild(0);
            
        Pool.AddObjectToPool(child.gameObject);
        child.SetParent(NotActiveContent.GetComponent<RectTransform>());
            
        _contentRectTransform.localPosition = new Vector2(_contentRectTransform.localPosition.x, _contentRectTransform.localPosition.y - 100); // 100, spacing + child's height Make dynamic later

        if (_pointerEventData != null)
        {
            _scrollRect.OnBeginDrag(_pointerEventData);
        }
        
        _topIndex = _topIndex + 1;
        if (_topIndex == 4)
        {
            _topIndex = 0;
        }
    }

    private void RemoveFromBottom()
    {
        var child = _contentRectTransform.GetChild(_contentRectTransform.childCount-1);
            
        Pool.AddObjectToPool(child.gameObject);
        child.SetParent(NotActiveContent.GetComponent<RectTransform>());
        
        _bottomIndex = _bottomIndex - 1;
        if (_bottomIndex == -1)
        {
            _bottomIndex = Pool.GetObjectCount() - 1;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _pointerEventData = eventData;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _pointerEventData = eventData;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _pointerEventData = null;
    }
}
