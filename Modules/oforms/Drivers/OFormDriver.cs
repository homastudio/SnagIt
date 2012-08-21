using System.Linq;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using oforms.Models;
using oforms.Services;
using Orchard.Localization.Services;

namespace oforms.Drivers
{
    public class OFormDriver : ContentPartDriver<OFormPart>
    {
        private readonly ISerialService _serialService;
        private readonly ICultureManager _cultureManager;

        public OFormDriver(ISerialService serialService,
            ICultureManager cultureManager)
        {
            this._serialService = serialService;
            this._cultureManager = cultureManager;
        }

        protected override DriverResult Display(OFormPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_OForm_Display",
                () => shapeHelper.Parts_OForm_Display(
                    ContentPart: part, 
                    ContentItem: part.ContentItem, 
                    ValidSn: this._serialService.ValidateSerial(),
                    Culture: GetCurrentSiteCulture()));
        }

        //GET
        protected override DriverResult Editor(OFormPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_OForm_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/OForms.Form",
                    Model: part,
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            OFormPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        private string GetCurrentSiteCulture()
        {
            var siteCulture = _cultureManager.GetSiteCulture();
            if (string.IsNullOrEmpty(siteCulture))
            {
                return null;
            }

            return siteCulture.Split('-').FirstOrDefault();
        }
    }
}