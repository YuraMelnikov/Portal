using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Areas.Reclamation.Models
{
    public class CorrectReclamation
    {
        Wiki.Reclamation reclamation;

        public Wiki.Reclamation Reclamation { get => reclamation; set => reclamation = value; }

        public CorrectReclamation(Wiki.Reclamation reclamation)
        {
            this.reclamation = reclamation;

        }

        bool GetCorrectFieldReclamation()
        {


            return true;
        }
    }
}