//구조체 : 공용 사용 변수들은 묶어서 정의하면 추가/삭제될 때 구조체에 선언함으로써 관리가 용이함.
[System.Serializable]
public struct PistolSetting
{
    public int damage;
    public float attackRate;//공속
    public float attackDistance;//사거리

}

