# Mock Data Framework

**Xamarin** as a mobile development platform, provides a wide range of tools and services covering learning, coding, debugging, applications profiling, analyzing visual tree and many other aspects. Along with these important features, the **automation of UI testing** capability is beneficial as well. Powered by [Visual Studio App Center](https://appcenter.ms), apps may even be tested on thousands of real devices in the cloud.

**Mock Data Framework** helps you to sample data for UI tests, without implementing new infrastructure and mock classes. The framework simulates server for your application providing custom response per each request. Testers can modify mocked data without involving developers. Data is stored in json, which doesn't require programming knowledge.

### Current Status

Framework is considered as a Release Candidate version.

### Authors

- Dzianis Zhukouski , EPAM Systems.
- Andrei Melnikau, EPAM Systems.

### External Dependencies

- Newtonsoft.Json (13.0.1);
- Xamarin.UITest (3.2.2);
- NUnit (3.13.2);
- ASP.NET Core (2.0.9), for the Mock Server component;

### Platform Dependencies

DataMocker.Mock and DataMocker.SharedModels:
- .NET Standart 2.1

DataMocker.UITest:
- .NET Framework 4.6.1

### Cloning Instruction

```
git clone https://github.com/epam-xamarin-lab/DataMocker.git
```

### Quick Start

##### Install nuget packages:

- [DataMocker.UITest](https://www.nuget.org/packages/DataMocker.UITest/), into your Xamarin.UITest-dependent project
- [DataMocker.Mock](https://www.nuget.org/packages/DataMocker.Mock/), into your native and platform-agnostic projects where you will embed your data samples (if presented)

##### Mock your Api service layer with Mock data service (you should use `MockHttpHandler` for your `HttpClient`);

API interface example:
``` 
public interface IRestApi
{
    Task<HttpResponseMessage> GetAsync(Uri uri);

    Task<HttpResponseMessage> PostAsync(Uri uri, HttpContent content);

    Task<HttpResponseMessage> PutAsync(Uri uri, HttpContent content);

    Task<HttpResponseMessage> DeleteAsync(Uri uri);
}
```

API mock implementation example:
```
public class MockRestApi : IRestApi
{
    protected HttpClient Client;
    
    public MockRestApi(IMockHandlerIntializer mockHandlerInitializer)
    {
        _client = new HttpClient(mockHandlerInitializer.GetMockerHandler());
    }

    public Task<HttpResponseMessage> GetAsync(Uri uri) => _client.GetAsync(uri);

    public Task<HttpResponseMessage> PostAsync(Uri uri, HttpContent content) => _client.PostAsync(uri, content);

    public Task<HttpResponseMessage> PutAsync(Uri uri, HttpContent content) => _client.PutAsync(uri, content);

    public Task<HttpResponseMessage> DeleteAsync(Uri uri) => _client.DeleteAsync(uri);
}
```

As an example you can create mock API builder class:
```
public class MockApiBuilder
{
    public static IRestApi GetMockApi(string environmentArguments)
    {
        var handlerInitializer = new MockHandlerInitializer(environmentArguments, typeof(MockDataComponent).Assembly);
        return new Services.MockRestApi(handlerInitializer);
    }
}
```

##### Create backdoor methods in `AppDelegate.cs` and `MainActivity.cs`
[About Backdoors](https://developer.xamarin.com/guides/testcloud/uitest/working-with/backdoors/)

`AppDelegate.cs`:
```
[Register("AppDelegate")]
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        ...
        #if ENABLE_TEST_CLOUD
        Xamarin.Calabash.Start();
        #endif
        ...
    }

    [Export("setupEnvironmentBackdoors:")]
    public NSString SetupEnvironmentBackdoors(NSString value)
    {
        Core.App.RegisterMockHandlerInitializer((string)value);
        return NSString.Empty;
    }
}
```

`MainActivity.cs`:
```
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    protected override void OnCreate(Bundle bundle){...}

    [Java.Interop.Export("SetupEnvironmentBackdoors")]
    public void SetupEnvironmentBackdoors(string value)
    {
        Core.App.RegisterMockHandlerInitializer(value);
        return;
    }
}
```

##### Fill `MockFrameworkConfiguration.json` in the your UI tests project and set it as `Copy to output directory`
`MockFrameworkConfiguration.json`:
```
{
  "Url": "http://localhost:5000/api",
  "Delay": 150,
  "UseEmbeddedDevice": true, /* Set true, if you don't want to remove url and work with Embedded resource */
  "AndroidBackdoorMethod": "SetupEnvironmentBackdoors",
  "IosBackdoorMethod": "setupEnvironmentBackdoors:"
}
```

##### Create tests

Initialize framework in test scenario setup method. As example:
```
[TestFixture(Platform.Android)]
[TestFixture(Platform.iOS)]
public class BaseTestScenario 
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

### Guides and Samples

Please review the detailed docs and samples provided in this repository.

### Release Notes

##### 1.0
    - Added main mock date logic;
    - Added test and data localization;
    - Added shared folders logic;
    - Added mock server;
    - Added url routing feature; 

### License Information

Licensed under Apache 2.0, [LICENSE](https://github.com/epam-xamarin-lab/DataMocker/blob/master/LICENSE)
