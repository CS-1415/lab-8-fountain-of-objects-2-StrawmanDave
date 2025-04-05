namespace Lab08;
interface IFleching
{
    public void DescribeFletching();
    public int GetCostFleching();
}

interface IArrowHead
{
    void DescribeArrowHead();
    int GetCostArrowHead();
}

interface IMaterial
{
    string? Material{get; set;}
    bool IsAcceptedMaterial(string input);
}