using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : MonoBehaviour
{
    private PlayerCharacter _player;

    [SerializeField] private Image _lifeBar;
    [SerializeField] private Image _manaBar;
    [SerializeField] private Slider _expBar;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerCharacter>();
        Debug.Assert(_player != null, "PlayerCharacter not found");

        OnLifeChanged(_player.Stats.Life);
        OnManaChanged(_player.Stats.Mana);

        _player.Stats.OnLifeChanged += OnLifeChanged;
        _player.Stats.OnManaChanged += OnManaChanged;
        
        OnExpChanged(_player.LevelSystem.Exp);

        _player.LevelSystem.OnExpChanged += OnExpChanged;
    }

    private void OnDestroy()
    {
        _player.Stats.OnLifeChanged -= OnLifeChanged;
        _player.Stats.OnManaChanged -= OnManaChanged;
    }

    private void OnLifeChanged(int value)
    {
        _lifeBar.fillAmount = (float)value / _player.Stats.MaxLife;
    }

    private void OnManaChanged(int value)
    {
        _manaBar.fillAmount = (float)value / _player.Stats.MaxMana;
    }

    private void OnExpChanged(int exp)
    {
        _expBar.value = (float)exp / _player.LevelSystem.MaxExp;
    }
}
