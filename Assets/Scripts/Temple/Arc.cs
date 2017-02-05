public class Arc {
    public readonly float AngleStart;
    public readonly float AngleEnd;

    public float Angle {
        get { return AngleEnd - AngleStart; }
    }

    public Arc(float angleStart, float angleEnd) {
        AngleStart = angleStart;
        AngleEnd = angleEnd;
    }
}