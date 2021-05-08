namespace doob.Reflectensions.Tests.TestClasses
{
    public interface ITransformer {
        Transformer<T> TransformTo<T>() where T : CamouflageMode;
    }

    
}
