using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtrasTracker : MonoBehaviour
{
    [SerializeField] private bool _canDoubleJump, _canDash, _canEnterBallMode, _canDropBombs;

    public bool CanDoubleJump { get => _canDoubleJump; set => _canDoubleJump = value; }
    public bool CanDash { get => _canDash; set => _canDash = value; }
    public bool CanEnterBallMode { get => _canEnterBallMode; set => _canEnterBallMode = value; }
    public bool CanDropBombs { get => _canDropBombs; set => _canDropBombs = value; }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("CanDoubleJump", _canDoubleJump ? 1 : 0);
        PlayerPrefs.SetInt("CanDash", _canDash ? 1 : 0);
        PlayerPrefs.SetInt("CanEnterBallMode", _canEnterBallMode ? 1 : 0);
        PlayerPrefs.SetInt("CanDropBombs", _canDropBombs ? 1 : 0);
    }

    public void LoadPlayerPrefs()
    {
        _canDoubleJump = PlayerPrefs.GetInt("CanDoubleJump", 0) == 1;
        _canDash = PlayerPrefs.GetInt("CanDash", 0) == 1;
        _canEnterBallMode = PlayerPrefs.GetInt("CanEnterBallMode", 0) == 1;
        _canDropBombs = PlayerPrefs.GetInt("CanDropBombs", 0) == 1;
    }
}
