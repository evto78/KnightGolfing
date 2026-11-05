using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActionScript : MonoBehaviour
{
    public GameObject ball;
    public List<GameObject> hitboxes;
    float m_moveTimer;
    static IdleState m_idle;
    static LightOneState m_lightOne;
    static LightTwoState m_lightTwo;

    public static  IdleState GetIdleState() { return  m_idle; }
    public static  LightOneState GetLightOneState() { return  m_lightOne; }
    public static  LightTwoState GetLightTwoState() { return  m_lightTwo; }

    [SerializeField]static ActionScript m_state;

    // Start is called before the first frame update
    void Start()
    {
        SetState(m_idle);
    }

    // Update is called once per frame
    void Update()
    {
        m_state.HandleInput(this);
        m_state.HandleUpdate(this);
    }

    public virtual void HandleInput(ActionScript _player)
    {

    }
    public virtual void HandleUpdate(ActionScript _player)
    {

    }
    public void SetState(ActionScript _state)
    {
        m_state = _state;
    }
}

public  class IdleState : ActionScript
{

    public override void HandleInput(ActionScript _player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            _player.SetState(ActionScript.GetLightOneState());
            hitboxes[0].SetActive(true);
        }
    }
    public override void HandleUpdate(ActionScript _player)
    {
        Debug.Log("Idle");
    }
}
public class LightOneState : ActionScript
{
    float m_lightOneMoveTimer = 0.5f;
    public override void HandleInput(ActionScript _player)
    {
        if (Input.GetMouseButtonDown(0) && m_lightOneMoveTimer <= 0)
        {
            _player.SetState(ActionScript.GetLightTwoState());
            hitboxes[1].SetActive(true);
            m_lightOneMoveTimer = 0.5f;
        }
    }

    public override void HandleUpdate(ActionScript _player)
    {
        while (m_lightOneMoveTimer >= 0)
        {
            m_lightOneMoveTimer -= Time.deltaTime;
        }
        
    }

}

public class LightTwoState : ActionScript
{
    float m_lightTwoMoveTimer = 0.5f;
}

