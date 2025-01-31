public class MazeCell
{
    public int x, y;
    public bool visited = false;
    public bool[] walls = { true, true, true, true }; // 상, 우, 하, 좌
    public MazeCell previous; // 경로 찾기를 위한 이전 셀 참조

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}