using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerLevelSystem LevelSystem { get; private set; }
    public PlayerStats Stats { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        LevelSystem = new PlayerLevelSystem(1, 0);
        Stats = new PlayerStats(100, 100);

        FollowCamera followCam = Camera.main.GetComponent<FollowCamera>();
        followCam.SetTarget(gameObject);
        followCam.CameraComponent.fieldOfView = 90;

        _movement.MoveSpeed = 5;
    }
}

public class PlayerLevelSystem
{
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int MaxExp { get; private set; } = 100;

    public delegate void LevelChangeDelegate(int level);
    public event LevelChangeDelegate OnLevelUp;

    public delegate void ExpChangeDelegate(int exp);
    public event ExpChangeDelegate OnExpChanged;

    public PlayerLevelSystem(int level, int exp)
    {
        Level = level;
        Exp = exp;
    }

    public void AddExp(int value)
    {
        Exp += value;
        OnExpChanged?.Invoke(Exp);

        if (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            Level++;
            OnLevelUp?.Invoke(Level);
        }
    }
}

public class PlayerStats
{
    public int MaxLife = 100;
    public int MaxMana = 100;
    public int Life { get; private set; }
    public int Mana { get; private set; }

    public delegate void OnStatChanged(int value);
    public event OnStatChanged OnLifeChanged;
    public event OnStatChanged OnManaChanged;

    public PlayerStats(int life, int mana)
    {
        MaxLife = life;
        MaxMana = mana;

        Life = life;
        Mana = mana;
    }

    public void AddLife(int value)
    {
        Life += value;

        OnLifeChanged?.Invoke(Life);
    }

    public void AddMana(int value)
    {
        Mana += value;

        OnManaChanged?.Invoke(Mana);
    }
}
