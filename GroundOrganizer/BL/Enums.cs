namespace GroundOrganizer
{
    public enum TypeFound { Прямоугольный, Ленточный, Круглый }
    public enum PointFound { Центр, Угол}
    public enum TypeFlexStructure { Гибкая, Жесткая }
    public enum ResKey { eps_b_max, eps_b_min, sig_b_max, sig_b_min, eps_bult, eps_s_max, eps_s_min, sig_s_max, sig_s_min, asel, kf, Mxult, Myult, Nult, Mxcrc, Mycrc, num_itr, rep, info }
    public enum SecShape { прямоугольник, круг, кольцо, тавр_с_полками_вверху, тавр_с_полками_внизу, двутавр, короб, пользовательское }
    public enum ElementType { стержень, пластина }
    public enum CharMat { C, CL, N, NL }
    public enum LongForces { кратковременное, длительное }
    public enum Vlajnost { Ниже_40, От_40_до_75, Выше_75 }
    public enum Orientation { вертикальная, горизонтальная }
    public enum ClassBet { B10, B15, B20, B25, B30, B35, B40, B45, B50, B55, B60 }
    public enum ClassArm { A240, A400, A500, B500, A600, A800, A1000, Bp500, Bp1200 }
    public enum TypeDiagramm { трехлинейная, двухлинейная }
    public enum TypeLayer { Au, As1, As2, As3, As4, одиночный, радиальный, линейный, AuAs }
    public enum ParamsLayer { AsDs, AsNd, DsNd }
}