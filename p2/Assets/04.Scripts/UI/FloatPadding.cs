[System.Serializable]
public struct FloatPadding
{
    public float left;
    public float right;
    public float top;
    public float bottom;

    public float horizontal { get { return left + right; } }
    public float vertical { get { return top + bottom; } }

    public FloatPadding(float padding) : this(padding, padding, padding, padding) { }
    public FloatPadding(float left, float right, float top, float bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }
}
