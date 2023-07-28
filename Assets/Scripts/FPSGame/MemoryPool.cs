using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{

    private class PoolItem
    {
        public bool isActive;//gameobject의 활성화.비활성화
        public GameObject gameObject; // 화면에 보이는 실제 게임 오브젝트
    }

    private int increaseCount = 5;      //오브젝트가 부족할 때 Instantiate()로 추가 생성되는 오브젝트 개수
    private int maxCount;       //현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;        //현재 게임에서 활성화된 오브젝트 개수

    private GameObject poolObject;  //오브젝트 폴링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList;    //관리되는 모든 오브젝트를 저장하는 리스트
    public int MaxCount => maxCount;    //외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount;      //외부에서 현재 활성화 되어 있는 오브젝트 개수 확인을 위한 프로퍼티

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for(int i = 0; i<increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();
            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);

        }
    }

    public void DestroyObjects()//씬이 바뀌거나 게임종료시 한번수행하여 모든 게임 오브젝트를 한번에 삭제한다.
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i =0; i<count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivePoolItem()//비활성화 오브젝트 중 하나를 활성화로 만들어 사용 비활성오브젝트 없으면 instantiateobjects() 호출해 추가생성
    {
        if (poolItemList == null) return null;

        if(maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for(int i =0;   i < count;   ++ i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivePoolItem(GameObject removeObject)//현재 활성화 상태인 오브젝트 removeobject를 비활성화   
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for(int i =0; i<count;++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject==removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactiveAllPoolItems()//활성화중인 모든 오브젝트를 보이지않게함.
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i=0;i<count;++i)
        {
            PoolItem poolItem = poolItemList[i];

            if( poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

            }
        }
        activeCount = 0;
    }
}
