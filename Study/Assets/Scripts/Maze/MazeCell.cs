public class MazeCell
{
    public int x, y;
    public bool visited = false;
    public bool[] walls = { true, true, true, true }; // ��, ��, ��, ��
    public MazeCell previous; // ��� ã�⸦ ���� ���� �� ����

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}