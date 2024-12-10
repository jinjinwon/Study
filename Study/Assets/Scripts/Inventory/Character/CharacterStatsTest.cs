using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsTest : MonoBehaviour
{
    public CharacterTest characterTest;

    public Text text_attack;
    public Text text_defance;
    public Text text_workspeed;
    public Text text_hp;
    public Text text_stamina;
    public Text text_weight;

    public Text text_StatusPoint;

    public Button button_atk_Increase;
    public Button button_def_Increase;
    public Button button_workspeed_Increase;
    public Button button_hp_Increase;
    public Button button_stamina_Increase;
    public Button button_weight_Increase;

    public Button button_atk_decrease;
    public Button button_def_decrease;
    public Button button_workspeed_decrease;
    public Button button_hp_decrease;
    public Button button_stamina_decrease;
    public Button button_weight_decrease;

    public StatModifier statModifier_atk;
    public StatModifier statModifier_def;
    public StatModifier statModifier_workspeed;
    public StatModifier statModifier_hp;
    public StatModifier statModifier_stamina;
    public StatModifier statModifier_weight;

    private void Start()
    {
        characterTest._characterStats.OnStatChanged += UpdateText;
        characterTest._characterStats.OnStatusPointsChanged += UpdateStatusPoint;

        button_atk_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_atk));
        button_def_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_def));
        button_workspeed_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_workspeed));
        button_hp_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_hp));
        button_stamina_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_stamina));
        button_weight_Increase.onClick.AddListener(() => OnClickIncreaseStat(statModifier_weight));

        button_atk_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_atk));
        button_def_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_def));
        button_workspeed_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_workspeed));
        button_hp_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_hp));
        button_stamina_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_stamina));
        button_weight_decrease.onClick.AddListener(() => OnClickDecreaseStat(statModifier_weight));

        InitUI();
    }

    private void InitUI()
    {
        foreach(var pair in characterTest._characterStats._stats)
        {
            UpdateText(pair.Key, pair.Value);
        }

        UpdateStatusPoint(characterTest._characterStats.StatusPoints);
    }

    private void OnClickIncreaseStat(StatModifier stat)
    {
        characterTest._characterStats.IncreaseStatWithPoints(stat);
    }

    private void OnClickDecreaseStat(StatModifier stat)
    {
        characterTest._characterStats.DecreaseStatWithPoints(stat);
    }

    private void UpdateText(Stat stat, float point)
    {
        switch(stat.StatType)
        {
            case StatType.Attack: text_attack.text = $"데미지 : {point.ToString()}"; break;
            case StatType.Defence: text_defance.text = $"방어력 {point.ToString()}"; break;
            case StatType.WorkSpeed: text_workspeed.text = $"작업 속도 : {point.ToString()}"; break;
            case StatType.HP: text_hp.text = $"체력 : {point.ToString()}"; break;
            case StatType.Stamina: text_stamina.text = $"기력 : {point.ToString()}"; break;
            case StatType.Weight: text_weight.text = $"최대 중량 {point.ToString()}"; break;
        }
    }

    private void UpdateStatusPoint(int value)
    {
        text_StatusPoint.text = $"StatusPoint : {value.ToString()}";
    }
}
