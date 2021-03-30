namespace Liquid.Core.Tests.DependencyInjection.Entities
{
    /// <summary>
    /// Test entity Class.
    /// </summary>
    /// <seealso cref="string" />
    internal class TestInterfaceClass : ITestInterface<string>
    {
        /// <summary>
        /// The string field
        /// </summary>
        private const string StringField = "Success";

        /// <summary>
        /// Gets the test.
        /// </summary>
        /// <returns></returns>
        public string GetTest()
        {
            return StringField;
        }
    }
}