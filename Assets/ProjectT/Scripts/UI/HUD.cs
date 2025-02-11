using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }
    [SerializeField]
    private InfoType _type;
    public InfoType Type { get { return _type; } }

    private TextMeshProUGUI _myText;
    private Slider _mySlider;


    private void Awake()
    {
        _myText = GetComponent<TextMeshProUGUI>();
        _mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (_type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.Exp;
                float maxExp = GameManager.Instance.NextExp[Mathf.Min(GameManager.Instance.Level, GameManager.Instance.NextExp.Length - 1)];
                _mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                _myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.Level);
                break;
            case InfoType.Kill:
                _myText.text = string.Format("{0:F0}", GameManager.Instance.Kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.MaxGameTime - GameManager.Instance.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                _myText.text = string.Format("{0:D2}:{1:D2}", min , sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Instance.Health;
                float maxHealth = GameManager.Instance.MaxHealth;
                _mySlider.value = curHealth / maxHealth;
                break;
            default:
                break;
        }
    }
}
