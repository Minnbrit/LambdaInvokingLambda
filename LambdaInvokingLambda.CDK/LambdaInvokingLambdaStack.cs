using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace LambdaInvokingLambda.CDK
{
    public class LambdaInvokingLambdaStack : Stack
    {
        internal LambdaInvokingLambdaStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var inventoryLambda = new Function(this, "Inventory", new FunctionProps
            {
                FunctionName = "Inventory",
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./Inventory/bin/Release/net6.0/Inventory.zip"),
                Handler = "Inventory::Inventory.Function::FunctionHandler",
            });

            var catalogLambda = new Function(this, "Catalog", new FunctionProps
            {
                FunctionName = "Catalog",
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("./Catalog/bin/Release/net6.0/Catalog.zip"),
                Handler = "Catalog::Catalog.Function::FunctionHandler",
                // Needed as I've seen it time out waiting for the first inventory lambda to be invoked
                Timeout = Duration.Seconds(30)
            });

            inventoryLambda.GrantInvoke(catalogLambda);
        }
    }
}
