using NUnit.Framework; // The testing library
using UnityEngine;     // Needed to create GameObjects

public class ShipTests
{
    [Test]
    // Test to verify that UpdateOxygen correctly subtracts oxygen
    public void UpdateOxygen_Subtracts_Correctly()
    {
        GameObject go = new GameObject();
        subScript ship = go.AddComponent<subScript>();
        
    
        ship.UpdateOxygen(0); 


        ship.UpdateOxygen(-20f);

        // Check the result
        // Expected: 80. Actual: ship.currentOxygen
        Assert.AreEqual(80f, ship.currentOxygen);

        // Cleanup
        Object.DestroyImmediate(go);
    }

    [Test]
    // Test to ensure oxygen does not drop below zero
    public void Oxygen_Does_Not_Drop_Below_Zero()
    {
        GameObject go = new GameObject();
        subScript ship = go.AddComponent<subScript>();

        // Remove 500 oxygen (way more than max)
        ship.UpdateOxygen(-500f);

        // Check the result
        // Should be clamped to 0, not -400
        Assert.AreEqual(0f, ship.currentOxygen);

        // Cleanup
        Object.DestroyImmediate(go);
    }

    [Test]
    // Test to ensure oxygen does not exceed max
    public void Oxygen_Does_Not_Exceed_Max()
    {
        GameObject go = new GameObject();
        subScript ship = go.AddComponent<subScript>();
        ship.currentOxygen = 90f;

        // Add a huge amount
        ship.UpdateOxygen(500f);

        // Check the result
        // Should be clamped to maxOxygen (100)
        Assert.AreEqual(100f, ship.currentOxygen);

        Object.DestroyImmediate(go);
}
}