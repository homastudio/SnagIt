using oforms.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace oforms.Handlers
{
    public class OFormHandler : ContentHandler
    {
        public OFormHandler(IRepository<OFormPartRecord> repo)
        {
            Filters.Add(StorageFilter.For(repo));

            OnRemoved<OFormPart>((context, form) => repo.Delete(form.Record));
        }
    }
}