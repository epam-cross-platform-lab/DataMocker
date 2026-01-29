# MAUI UITest Migration Guide

This guide explains how to complete the migration of DataMockerSample.UITests to .NET MAUI UITest framework.

## Overview

The UITests project has been migrated from Xamarin.UITest to be compatible with .NET MAUI UITest frameworks. The main changes include:

1. **Project File**: Updated to SDK-style format targeting .NET 10.0
2. **Base Classes**: Updated to use `TestDevice` instead of `Platform`
3. **Context**: Updated to use `IUITestContext` abstraction instead of `IApp`
4. **Test Fixtures**: Updated to use `TestDevice` enum

## Official MAUI UITest Pattern

According to the [MAUI UITest guide](https://github.com/dotnet/maui/wiki/UITests), the recommended approach is:

- **Test classes should inherit from `_IssuesUITest`** (provided by MAUI UITest framework)
- The `_IssuesUITest` base class provides `Context` and `Device` properties
- Tests use Appium with NUnit for cross-platform testing

## Integration Steps

### Step 1: Choose Your MAUI UITest Framework

The migration supports multiple MAUI UITest frameworks:
- **MAUI's `_IssuesUITest` base class** (recommended - official MAUI pattern)
- **Appium with NUnit** (alternative approach)
- **Custom UITest framework** (as long as it provides a context with `Invoke` method)

### Step 2: Set Up the Test Context

You need to set the `Context` property in your test classes. The exact implementation depends on your chosen framework:

#### Option A: Using MAUI's `_IssuesUITest` (Recommended)

According to the [MAUI UITest guide](https://github.com/dotnet/maui/wiki/UITests), this is the official pattern:

```csharp
[TestFixture(TestDevice.Android)]
[TestFixture(TestDevice.iOS)]
public class MobileTest : _IssuesUITest  // Inherit from MAUI's base class
{
    public MobileTest(TestDevice device) : base(device)
    {
        // _IssuesUITest handles Context and Device initialization
    }

    [SetUp]
    public override void SetUp()
    {
        // IMPORTANT: Call base.SetUp() first to initialize Context
        base.SetUp();
        
        // Now initialize DataMocker with the Context from _IssuesUITest
        var dataMockerContext = Context.AsUITestContext();
        DataMocker.UITest.TestInitializer.Initialize(dataMockerContext, Device, GetType());
    }

    [Test]
    public void MyTest()
    {
        // Your test code - Context is available from _IssuesUITest
    }
}
```

**Key Points:**
- Inherit from `_IssuesUITest` (not `BaseTest`)
- Call `base.SetUp()` first to ensure `Context` is initialized
- Then initialize DataMocker using the `Context` from the base class

#### Option B: If using BaseTest (Legacy/Custom Pattern)

```csharp
public class MobileTest : BaseTest
{
    // If your framework provides UITestBase with Context property
    public MobileTest(TestDevice device) : base()
    {
        Device = device;
        // Context = base.Context; // Set this if available from base class
    }

    [SetUp]
    public override void BeforeEachTest()
    {
        // Set Context from your test framework
        Context = GetUITestContext(); // Implement this based on your framework
        base.BeforeEachTest();
    }
}
```

#### Option B: If using Appium

```csharp
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;

public class MobileTest : BaseTest
{
    private AppiumDriver? _driver;

    public MobileTest(TestDevice device)
    {
        Device = device;
    }

    [SetUp]
    public override void BeforeEachTest()
    {
        // Initialize Appium driver
        if (Device == TestDevice.Android)
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("appPackage", "com.epam.DataMockerSample");
            options.AddAdditionalCapability("appActivity", ".MainActivity");
            _driver = new AndroidDriver(new Uri("http://localhost:4723"), options);
        }
        else if (Device == TestDevice.iOS)
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("bundleId", "com.epam.DataMockerSample");
            _driver = new IOSDriver(new Uri("http://localhost:4723"), options);
        }

        // Wrap the driver as Context (you may need to create an adapter)
        Context = CreateUITestContextAdapter(_driver);
        base.BeforeEachTest();
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Quit();
    }
}
```

### Step 3: Create UITestContext Adapter (if needed)

If your test framework doesn't directly provide an object with an `Invoke(string, string)` method, you need to create an adapter:

```csharp
using DataMocker.UITest;

public class AppiumUITestContextAdapter : IUITestContext
{
    private readonly AppiumDriver _driver;

    public AppiumUITestContextAdapter(AppiumDriver driver)
    {
        _driver = driver;
    }

    public void Invoke(string methodName, string argument)
    {
        // Implement backdoor method invocation for your framework
        // This is typically done through ExecuteScript or similar
        _driver.ExecuteScript("mobile: " + methodName, argument);
    }
}
```

### Step 4: Update UI Interaction Methods

The tests use dynamic dispatch (`app.WaitForElement`, `app.Tap`, etc.). Ensure your `Context` object supports these methods:

- `WaitForElement(string automationId, string timeoutMessage = null)`
- `Tap(string automationId)`
- `EnterText(string automationId, string text)`
- `ClearText(string automationId)` or `ClearText(Func<AppQuery, AppQuery> query)`

If your framework uses different method names, you can:

1. **Create a wrapper class** that adapts your framework's API to the expected interface
2. **Update the tests** to use your framework's specific API
3. **Use extension methods** to add compatibility methods

### Step 5: Configure App Package Identifiers

Update `AppInitializer.cs` with your actual app package identifiers if they differ from the defaults.

## Testing the Migration

1. **Build the project**: Ensure it compiles without errors
2. **Run a simple test**: Start with `AppLaunches()` test
3. **Verify DataMocker integration**: Check that mock data is being loaded correctly
4. **Run all tests**: Execute the full test suite

## Common Issues and Solutions

### Issue: "Context is not initialized"

**Solution**: Ensure you set the `Context` property in your `SetUp` method or constructor before calling `base.BeforeEachTest()`.

### Issue: "Method not found on Context"

**Solution**: Your `Context` object needs to support the UI interaction methods. Create an adapter or wrapper class that provides these methods.

### Issue: Lambda expressions not working

**Solution**: The lambda syntax `c => c.Id("ItemDescription")` is Xamarin.UITest specific. You may need to update these to use your framework's query syntax, or create a compatibility layer.

### Issue: Platform-specific code

**Solution**: Replace `Platform.Android` and `Platform.iOS` checks with `TestDevice.Android` and `TestDevice.iOS`.

## Example: Complete Integration with Appium

Here's a complete example using Appium:

```csharp
using System;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Interfaces;
using DataMocker.UITest;

namespace DataMockerSample.UITests
{
    [TestFixture(TestDevice.Android)]
    [TestFixture(TestDevice.iOS)]
    public class MobileTest : BaseTest
    {
        private AppiumDriver? _driver;

        public MobileTest(TestDevice device)
        {
            Device = device;
        }

        [SetUp]
        public override void BeforeEachTest()
        {
            // Initialize Appium
            var serverUri = new Uri("http://localhost:4723");
            
            if (Device == TestDevice.Android)
            {
                var options = new AppiumOptions();
                options.AddAdditionalCapability("appPackage", "com.epam.DataMockerSample");
                options.AddAdditionalCapability("appActivity", ".MainActivity");
                _driver = new AndroidDriver(serverUri, options);
            }
            else if (Device == TestDevice.iOS)
            {
                var options = new AppiumOptions();
                options.AddAdditionalCapability("bundleId", "com.epam.DataMockerSample");
                _driver = new IOSDriver(serverUri, options);
            }

            // Create adapter for DataMocker
            Context = new AppiumUITestContextAdapter(_driver);
            
            base.BeforeEachTest();
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
        }
    }
}
```

## Additional Resources

- [MAUI UITest Wiki](https://github.com/dotnet/maui/wiki/UITests)
- [Appium Documentation](https://appium.io/docs/en/latest/)
- [DataMocker.UITest Migration Guide](../../../src/DataMocker.UITest/MIGRATION_MAUI.md)

## Notes

- The `app` property uses dynamic dispatch, so method calls are resolved at runtime
- Ensure your test framework's context object supports the required UI interaction methods
- The DataMocker framework only requires the `Invoke` method for backdoor calls
- All other UI interactions depend on your chosen test framework
