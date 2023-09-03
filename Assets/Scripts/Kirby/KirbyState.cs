using UnityEngine;

public abstract class KirbyState : MonoBehaviour, KirbyFSM<string, KirbyState>.IFSMState
{
    [SerializeField] protected string _key;

    protected KirbyController kc;

    public string GetKey => _key;
    public bool interactCollisionSprite = false;
    public bool interactActionInput = false;

    public void Initialize(KirbyController con)
    {
        kc = con;
        kc.GetFSM.AddState(_key, this);

        OnStateInitialize();
    }

    public virtual void OnStateInitialize()
    {

    }

    public virtual void OnPrePhysCheck()
    {

    }

    public virtual void OnPostPhysCheck()
    {

    }

    public virtual void OnLand()
    {

    }

    public virtual void OnHit()
    {

    }

    public abstract void Enter();

    public abstract void Excute();

    public abstract void Exit();
}
