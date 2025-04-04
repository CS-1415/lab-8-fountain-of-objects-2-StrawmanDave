namespace lab08.Tests;

using System.Diagnostics;
using Lab08;

public class ArmorTests
{
    public Armor _Armor;
    [SetUp]
    public void Setup()
    {
        _Armor = new Armor();
    }
    
    [Test]
    public void TestTier()
    {
        _Armor.IsAcceptedMaterial("Wood");
        Debug.Assert(_Armor.Tier() == 1);
    }

    [Test]
    public void TestAcceptedMaterial()
    {
        Debug.Assert(_Armor.IsAcceptedMaterial("bleh") == false);
        Debug.Assert(_Armor.IsAcceptedMaterial(null) == false);
        Debug.Assert(_Armor.IsAcceptedMaterial("Wood") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("Iron") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("Steel") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("Titanium") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("Carbon Fiber") == true);
    }
}