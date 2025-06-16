using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //TODO с zenject заинжектить левел и буллет манагеры, и тогда на сцене только один инсталлер будет

    [SerializeField] Transform locationContainer;
    [SerializeField] Transform towersContainer;
    [SerializeField] Transform spawnersContainer;
    [SerializeField] LevelInfo[] levels;

    //можно использовать Zenject Bind AsSingle NoLazy Вместо статик Синглтона
    //[Inject] если Zenject
    [SerializeField] BulletManager bulletManager;

    [ContextMenu("Reset")]
    void ResetLevel()
    {
        Awake();
    }

    void ClearLevel()
    {
        DestroyChildenIn(locationContainer);

        bulletManager.ResetMe();
        DestroyChildenIn(towersContainer);

        //todo очистить пулы у имеющихся спавнеров или отписать монстров
        DestroyChildenIn(spawnersContainer);
    }


    void DestroyChildenIn(Transform tr)
    {
        for (int i = tr.childCount - 1; i >= 0; i--)
            Destroy(tr.GetChild(i).gameObject);
    }

    void CreateLevel(int index)
    {
        Instantiate(levels[index].locationPrefab, locationContainer);

        Tower currentTower;

        for (int i = 0; i < levels[index].towers.Length; i++)
        {
            currentTower = Instantiate(levels[index].towers[i].prefab, towersContainer).GetComponent<Tower>();
            currentTower.transform.position = levels[index].towers[i].position;
            bulletManager.AddTower(currentTower);
        }

        for (int i = 0; i < levels[index].spawners.Length; i++)
        {
            Instantiate(levels[index].spawners[i].prefab, spawnersContainer).transform.position = levels[index].spawners[i].position;
        }

    }

    private void Awake()
    {
        ClearLevel();

        int currentLevel = 0;//prefs load
        CreateLevel(currentLevel);
    }

}
