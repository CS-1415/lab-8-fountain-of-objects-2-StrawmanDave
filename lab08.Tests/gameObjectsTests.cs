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
        _Armor.IsAcceptedMaterial("wood");
        Debug.Assert(_Armor.setTier() == 1);
    }

    [Test]
    public void TestAcceptedMaterial()
    {
        Debug.Assert(_Armor.IsAcceptedMaterial("bleh") == false);
        Debug.Assert(_Armor.IsAcceptedMaterial(null) == false);
        Debug.Assert(_Armor.IsAcceptedMaterial("wood") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("iron") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("steel") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("titanium") == true);
        Debug.Assert(_Armor.IsAcceptedMaterial("carbon fiber") == true);
    }
}