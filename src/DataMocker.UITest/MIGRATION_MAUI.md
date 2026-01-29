# Migration to .NET MAUI UITest Framework

This document describes the migration of DataMocker.UITest from Xamarin.UITest to .NET MAUI UITest framework compatibility.

## Changes Summary

### 1. Project Structure
- **Updated**: `DataMocker.UITest.csproj` to modern SDK-style format
- **Target Frameworks**: Now supports `netstandard2.1`, `net8.0`, and `net9.0`
- **Removed**: Dependency on `Xamarin.UITest`
- **Added**: `System.Text.Json` for JSON serialization (replacing Newtonsoft.Json)

### 2. API Changes

#### Platform Enum → TestDevice Enum
- **Old**: `Xamarin.UITest.Platform` (Android, iOS)
- **New**: `DataMocker.UITest.TestDevice` (Android, iOS, Windows, Mac)

#### IApp → IUITestContext
- **Old**: `Xamarin.UITest.IApp` interface
- **New**: `DataMocker.UITest.IUITestContext` interface
- **Purpose**: Abstraction for MAUI UITestContext compatibility

### 3. Updated Classes

#### TestInitializer
- **Changed**: `Initialize(IApp, Platform, Type)` → `Initialize(IUITestContext, TestDevice, Type)`
- **Usage**: Now accepts MAUI UITestContext instead of Xamarin.UITest IApp

#### AppEnvironmentConfiguration
- **Changed**: Now uses `IUITestContext` and `TestDevice` instead of `IApp` and `Platform`
- **Updated**: JSON deserialization to handle nullable values

#### MockFrameworkConfiguration
- **Added**: Support for Windows and Mac backdoor methods
- **Updated**: `GetBackDoorName()` now uses `TestDevice` enum with switch expression

### 4. New Files

#### IUITestContext.cs
- Interface for MAUI UITestContext compatibility
- Provides `Invoke(string methodName, string argument)` method for backdoor calls

#### TestDevice.cs
- Enum representing test device platforms (Android, iOS, Windows, Mac)

#### UITestContextExtensions.cs
- Extension methods for easier integration with MAUI UITestContext
- `InitializeDataMocker()` - Helper method to initialize DataMocker in test setup
- `AsUITestContext()` - Adapter for existing MAUI UITestContext instances

### 5. Configuration Updates

#### MockFrameworkConfiguration.json
- **Added**: `WindowsBackdoorMethod` and `MacBackdoorMethod` properties
- **Maintained**: Backward compatibility with existing Android and iOS configurations

## Migration Guide for Test Projects

### Before (Xamarin.UITest)
```csharp
[TestFixture(Platform.Android)]
[TestFixture(Platform.iOS)]
public class BaseTest
{
    protected IApp app;
    protected Platform platform;

    public BaseTest(Platform platform)
    {
        this.platform = platform;
    }

    [SetUp]
    public void BeforeEachTest()
    {
        app = AppInitializer.StartApp(platform);
        DataMocker.UITest.TestInitializer.Initialize(app, platform, GetType());
    }
}
```

### After (MAUI UITest)
```csharp
public class BaseTest : UITestBase  // or your MAUI test base class
{
    [SetUp]
    public void BeforeEachTest()
    {
        // Assuming Context is UITestContext and Device is TestDevice from MAUI
        var testContext = Context.AsUITestContext(); // Use extension method
        var testDevice = Device; // Map MAUI TestDevice to DataMocker TestDevice
        
        DataMocker.UITest.TestInitializer.Initialize(testContext, testDevice, GetType());
        
        // Or use the extension method:
        // UITestContextExtensions.InitializeDataMocker(Context, Device, GetType());
    }
}
```

### TestDevice Mapping
If your MAUI test framework uses a different TestDevice enum, you'll need to map it:
```csharp
DataMocker.UITest.TestDevice MapToDataMockerTestDevice(YourFramework.TestDevice device)
{
    return device switch
    {
        YourFramework.TestDevice.Android => DataMocker.UITest.TestDevice.Android,
        YourFramework.TestDevice.iOS => DataMocker.UITest.TestDevice.iOS,
        YourFramework.TestDevice.Windows => DataMocker.UITest.TestDevice.Windows,
        YourFramework.TestDevice.Mac => DataMocker.UITest.TestDevice.Mac,
        _ => throw new NotSupportedException($"Device {device} is not supported")
    };
}
```

## Breaking Changes

1. **Platform Enum**: Must migrate from `Xamarin.UITest.Platform` to `DataMocker.UITest.TestDevice`
2. **IApp Interface**: Must migrate from `Xamarin.UITest.IApp` to `DataMocker.UITest.IUITestContext`
3. **Target Framework**: Minimum .NET Standard 2.1 or .NET 8.0+
4. **Package Version**: Bumped to 2.0.0 (major version change indicates breaking changes)

## Compatibility

- **Backward Compatibility**: Not maintained - this is a breaking change migration
- **MAUI Compatibility**: Designed to work with .NET MAUI UITest framework patterns
- **Multi-platform**: Now supports Android, iOS, Windows, and Mac

## Notes

- The `IUITestContext` interface is designed to be compatible with MAUI's UITestContext pattern
- The `UITestContextExtensions.AsUITestContext()` method can adapt existing MAUI UITestContext instances
- All nullable reference types have been properly annotated for better type safety
