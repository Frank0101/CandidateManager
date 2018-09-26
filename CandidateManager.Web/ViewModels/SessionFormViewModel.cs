using System.Collections.Generic;
using System.Web.Mvc;

namespace CandidateManager.Web.ViewModels
{
    public class SessionFormViewModel
    {
        public IEnumerable<SelectListItem> Candidates { get; set; }
        public IEnumerable<SelectListItem> Exercises { get; set; }
        public SessionViewModel Session { get; set; }
    }
}
