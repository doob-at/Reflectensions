namespace doob.Reflectensions.Tests.TestClasses
{
    public class ActionInvokeTestClass<T>
    {
        public T Value { get; set; }

        public ActionInvokeTestClass() {}
        public ActionInvokeTestClass(T value)
        {
            Value = value;
        }
    }
}
