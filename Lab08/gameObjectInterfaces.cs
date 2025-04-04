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
    public int Durabilty{get; set;}
    void DescribeMaterial();
    bool IsAcceptedMaterial(string input);
    int randomDurabilty();
}