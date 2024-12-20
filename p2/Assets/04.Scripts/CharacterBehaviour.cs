public abstract class CharacterBehaviour
{
    public bool IsComplete { get; private set; }
    
    public void UpdateBehaviour()
    {
        DoBehaviour();
        if(CalcComplete() == true)
        {
            IsComplete = true;
        }
    }

    protected abstract void DoBehaviour();
    protected abstract bool CalcComplete();
}
