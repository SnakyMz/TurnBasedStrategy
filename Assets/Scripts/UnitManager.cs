using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    List<Unit> units;
    List<Unit> friendlyUnits;
    List<Unit> enemyUnits;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one unit manager system" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        units = new List<Unit>();
        friendlyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Unit.OnUnitSpawn += UnitSpawn;
        Unit.OnUnitDeath += UnitDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UnitSpawn(Unit unit)
    {
        units.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnits.Add(unit);
        }
        else
        {
            friendlyUnits.Add(unit);
        }
    }

    void UnitDeath(Unit unit)
    {
        units.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnits.Remove(unit);
        }
        else
        {
            friendlyUnits.Remove(unit);
        }
    }

    void OnDestroy()
    {
        Unit.OnUnitSpawn -= UnitSpawn;
        Unit.OnUnitDeath -= UnitDeath;
    }

    public List<Unit> GetUnits()
    {
        return units;
    }

    public List<Unit> GetEnemyUnits()
    {
        return enemyUnits;
    }

    public List<Unit> GetFriendlyUnits()
    {
        return friendlyUnits;
    }
}
