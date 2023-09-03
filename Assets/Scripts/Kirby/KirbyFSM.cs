using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;

public class KirbyFSM<T1,T2> where T2 : KirbyFSM<T1, T2>.IFSMState
{

    private Dictionary<T1, T2> stateList;

    private T2 _current;

    public T2 Current => _current;

    public KirbyFSM()
    {
        stateList = new Dictionary<T1, T2>();
    }

    public interface IFSMState
    {
        void Enter();
        void Excute();
        void Exit();
    }
    
    public void AddState(T1 key, T2 value)
    {
        stateList.Add(key, value);
    }

    public void RemoveState(T1 key)
    {
        stateList.Remove(key);
    }

    public void SetState(T1 key) 
    {
        _current = stateList[key];
    }

    public void SwitchState(T1 nextKey)
    {
        T2 next = stateList[nextKey];
        _current.Exit();
        next.Enter();
        _current = next;
    }

    public void SwitchStateAndExcute(T1 nextKey)
    {
        T2 next = stateList[nextKey];
        _current.Exit();
        next.Enter();
        _current = next;
        _current.Excute();
    }
}


