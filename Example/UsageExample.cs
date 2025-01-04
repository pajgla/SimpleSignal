using Signals;
using UnityEngine;

public sealed class TestSignal : Signal<bool>
{
    
}

public struct CustomSignalData
{
    public int SomeInteger;
    public GameObject SomeGO;
}

public sealed class TestSignalWithCustomData : Signal<CustomSignalData>
{
    
}

//Signals need to be stored somewhere - usually in a manager
public class ExampleManager
{
    public TestSignal TestSignal { get; private set; }
    public TestSignalWithCustomData TestSignalWithCustomData { get; private set; }

    static ExampleManager Instance;
    
    public static ExampleManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ExampleManager();
        }
        
        return Instance;
    }

    public void Initialize()
    {
        TestSignal = new TestSignal();
        TestSignalWithCustomData = new TestSignalWithCustomData();
    }
}

public class SubscribeExample
{
    public void Start()
    {
        ExampleManager exampleManager = ExampleManager.GetInstance();
        exampleManager.TestSignal.Subscribe(TestSignalCallback);
        exampleManager.TestSignalWithCustomData.Subscribe(TestSignalWithCustomDataCallback);
    }

    void TestSignalCallback(bool value)
    {
        Debug.Log(value);
    }

    void TestSignalWithCustomDataCallback(CustomSignalData data)
    {
        Debug.Log(data.SomeGO);
        Debug.Log(data.SomeInteger);
    }
}

public class DispatchExample
{
    public void DispatchSignals()
    {
        ExampleManager exampleManager = ExampleManager.GetInstance();
        exampleManager.TestSignal.Dispatch(false); //False is sent as signal data
        
        CustomSignalData newData = new CustomSignalData { SomeInteger = 1, SomeGO = new GameObject() };
        exampleManager.TestSignalWithCustomData.Dispatch(newData);
    }
}