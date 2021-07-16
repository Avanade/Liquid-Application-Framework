namespace Liquid.Core.UnitTests
{
    public class LocalizationTest
    {
        //private const string StringValueKey = "stringValueKey";
        //private ILocalization _subjectUnderTest;
        //private IServiceProvider _serviceProvider;

        //public LocalizationTest()
        //{
        //    IServiceCollection services = new ServiceCollection();
        //    var builder = new ConfigurationBuilder();

        //    builder.AddJsonFile("appsettings.json");

        //    var config = builder.Build();
        //    services.AddSingleton(config);

        //    services.AddLiquidConfiguration();
        //    services.AddLocalizationService();
        //    _serviceProvider = services.BuildServiceProvider();


        //    var cultureInfo = new CultureInfo("pt-BR");
        //    Thread.CurrentThread.CurrentCulture = cultureInfo;
        //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //    _subjectUnderTest = _serviceProvider.GetService<ILocalization>();
        //}

        ///// <summary>
        ///// Asserts if can read string from cache.
        ///// </summary>
        //[Fact]
        //public void Verify_if_can_read_string_from_file()
        //{
        //    var stringValue = _subjectUnderTest.Get(StringValueKey);
        //    Assert.Equal("Texto em português", stringValue);
        //    stringValue = _subjectUnderTest.Get(StringValueKey, "android");
        //    Assert.Equal("Texto em português", stringValue);
        //    stringValue = _subjectUnderTest.Get(StringValueKey, new CultureInfo("en-US"));
        //    Assert.Equal("English text", stringValue);
        //    stringValue = _subjectUnderTest.Get(StringValueKey, new CultureInfo("es-ES"), "iphone");
        //    Assert.Equal("texto en español", stringValue);
        //    stringValue = _subjectUnderTest.Get("InexistentKey");
        //    Assert.Equal("InexistentKey", stringValue);

        //}

        ///// <summary>
        ///// Verifies exceptions.
        ///// </summary>
        //[Fact]
        //public void Verify_Exceptions()
        //{
        //    Assert.Throws<ArgumentNullException>(() => { _serviceProvider.GetService<ILocalization>().Get(StringValueKey, (CultureInfo)null); });
        //    Assert.Throws<ArgumentNullException>(() => { _serviceProvider.GetService<ILocalization>().Get(null); });
        //}
    }
}
