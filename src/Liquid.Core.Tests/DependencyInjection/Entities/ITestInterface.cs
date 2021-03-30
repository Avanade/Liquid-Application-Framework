namespace Liquid.Core.Tests.DependencyInjection.Entities
{
    /// <summary>
    /// Test Interface.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    internal interface ITestInterface<TObject>
    {
        TObject GetTest();
    }
}