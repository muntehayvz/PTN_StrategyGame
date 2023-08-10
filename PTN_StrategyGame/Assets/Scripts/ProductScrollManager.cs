using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject Content;
    public GameObject NotActiveContent;

    private ScrollRect scrollRect; 
    
    private RectTransform contentRectTransform;

    private PointerEventData pointerEventData;

    public PoolManager Pool;

    private int topIndex;
    private int bottomIndex;
    
    private void Start()
    {   
        var objectList = new List<GameObject>();
        for (var i = 0; i < Content.transform.childCount; i++)
        {
            objectList.Add(Content.transform.GetChild(i).gameObject);
        }
        Pool.SetPooledObjects(objectList);
        
        topIndex = Pool.GetPoolSize() - 1;
        
        contentRectTransform = Content.GetComponent<RectTransform>();
        scrollRect = GetComponent<ScrollRect>();
        
        StartCoroutine(EndOfFrame());
    }

    // Monitors scroll position and triggers methods to add or remove items accordingly.
    private void Update()
    {
        if (contentRectTransform.sizeDelta.y - contentRectTransform.localPosition.y < GetComponent<RectTransform>().rect.height*1.1)
        {
            AddToBottom();
        }
        if (contentRectTransform.localPosition.y < 100)
        {
            AddToTop();
        }
        if (contentRectTransform.localPosition.y > 250)
        {
            RemoveFromTop();
        }
        if (contentRectTransform.sizeDelta.y - contentRectTransform.localPosition.y > GetComponent<RectTransform>().rect.height*1.3)
        {
            RemoveFromBottom();
        }
    }

    // Resizes the content area of the scroll view after the end of the frame.
    private IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        //var topSpacing = _contentRectTransform.GetComponent<GridLayoutGroup>().padding.top;

        var topSpacing = contentRectTransform.GetComponent<VerticalLayoutGroup>().padding.top;

        var firstChildRTransform = contentRectTransform.GetChild(0).GetComponent<RectTransform>();
        var lastChildRTransform = contentRectTransform.GetChild(contentRectTransform.childCount - 1).GetComponent<RectTransform>();
        var firstChildTop = firstChildRTransform.localPosition.y +
                            firstChildRTransform.sizeDelta.y * firstChildRTransform.localScale.y;
        var lastChildBottom = lastChildRTransform.localPosition.y -
                              lastChildRTransform.sizeDelta.y * lastChildRTransform.localScale.y;
            
        contentRectTransform.sizeDelta = new Vector2(0, topSpacing + firstChildTop - lastChildBottom);

        StartCoroutine(EndOfFrame());
    }

    // Adds a new object to the top of the scroll view.
    private void AddToTop()
    {
        var newObject = Pool.GetObjectFromPool(topIndex);
        newObject.GetComponent<RectTransform>().SetParent(Content.GetComponent<RectTransform>(), false);
        newObject.GetComponent<RectTransform>().SetSiblingIndex(0);
        newObject.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = newObject.name;
            
        topIndex = topIndex - 1;
        if (topIndex == -1)
        {
            topIndex = Pool.GetPoolSize()-1;
        }
            
        contentRectTransform.localPosition = new Vector2(contentRectTransform.localPosition.x, contentRectTransform.localPosition.y + 100);

        if (pointerEventData != null)
        {
            scrollRect.OnBeginDrag(pointerEventData);
        }
    }

    // Adds a new object to the bottom of the scroll view.
    private void AddToBottom()
    {
        var newGameObject = Pool.GetObjectFromPool(bottomIndex);
        newGameObject.GetComponent<RectTransform>().SetParent(Content.GetComponent<RectTransform>(), false);
        newGameObject.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = newGameObject.name;
            
        bottomIndex = bottomIndex + 1;
        if (bottomIndex == 2)
        {
            bottomIndex = 0;
        }
    }

    // Removes an object from the top of the scroll view and adds it back to the pool.
    private void RemoveFromTop()
    {
        var child = contentRectTransform.GetChild(0);
            
        Pool.AddObjectToPool(child.gameObject);
        child.SetParent(NotActiveContent.GetComponent<RectTransform>());
            
        contentRectTransform.localPosition = new Vector2(contentRectTransform.localPosition.x, contentRectTransform.localPosition.y - 100); 

        if (pointerEventData != null)
        {
            scrollRect.OnBeginDrag(pointerEventData);
        }
        
        topIndex = topIndex + 1;
        if (topIndex == 2)
        {
            topIndex = 0;
        }
    }

    // Removes an object from the bottom of the scroll view and adds it back to the pool.
    private void RemoveFromBottom()
    {
        var child = contentRectTransform.GetChild(contentRectTransform.childCount-1);
            
        Pool.AddObjectToPool(child.gameObject);
        child.SetParent(NotActiveContent.GetComponent<RectTransform>());
        
        bottomIndex = bottomIndex - 1;
        if (bottomIndex == -1)
        {
            bottomIndex = Pool.GetPoolSize() - 1;
        }
    }

    // Records drag event data for future use.
    public void OnDrag(PointerEventData eventData)
    {
        pointerEventData = eventData;
    }

    // Records the start of drag event for future use.
    public void OnBeginDrag(PointerEventData eventData)
    {
        pointerEventData = eventData;
    }

    // Clears recorded drag event data.
    public void OnEndDrag(PointerEventData eventData)
    {
        pointerEventData = null;
    }
}
