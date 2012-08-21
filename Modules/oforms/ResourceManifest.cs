using Orchard.UI.Resources;

namespace oforms
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("AdminOform").SetUrl("oform.css");
            manifest.DefineStyle("AdminHelptooltip").SetUrl("jquery.helptooltip.css");
            
            manifest.DefineStyle("ValidatorCss").SetUrl("cmxform.css");

            manifest.DefineScript("Validator").SetUrl("jquery.validate.min.js").SetDependencies("jQuery");
            manifest.DefineScript("ValidatorAdditional").SetUrl("additional-methods.min.js").SetDependencies("Validator");
        }
    }
}
