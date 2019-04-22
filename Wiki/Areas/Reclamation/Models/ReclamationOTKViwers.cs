using System;

namespace Wiki.Areas.Reclamation.Models
{
    public struct ReclamationOTKViwers
    {
        int id_Reclamation;
        int planZakaz;
        string type;
        string close;
        string text;
        string description;
        float timeToSearch;
        float timeToEliminate;
        string answers;
        string devision;
        DateTime dateCreate;
        DateTime dateLastAnswer;
    }
}