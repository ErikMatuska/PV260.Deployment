// See https://aka.ms/new-console-template for more information
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

Console.WriteLine("Hello, World!");

await Deployment.RunAsync<MyStack>();

class MyStack : Stack
{
    public MyStack()
    {
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("resourceGroup");

        // Create an App Service Plan
        var appServicePlan = new AppServicePlan("appServicePlan", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuDescriptionArgs
            {
                Name = "B1",
                Tier = "Basic"
            },
        });

        // Create the App Service
        var app = new WebApp("blazorApp", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            ServerFarmId = appServicePlan.Id,
            SiteConfig = new SiteConfigArgs
            {
                AppSettings = new[]
                {
                    new NameValuePairArgs
                    {
                        Name = "WEBSITE_RUN_FROM_PACKAGE",
                        Value = "1",
                    },
                },
            },
        });

        // Output the endpoint
        this.Endpoint = app.DefaultHostName.Apply(endpoint => $"https://{endpoint}");
    }

    [Output]
    public Output<string> Endpoint { get; set; }
}
