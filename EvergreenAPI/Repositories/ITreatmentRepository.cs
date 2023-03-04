﻿using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface ITreatmentRepository
    {
        ICollection<Image> GetImages();
        ICollection<Treatment> GetTreatments();
        ICollection<Disease> GetDiseases();
        Treatment GetTreatment(int id);
        bool TreatmentExist(int id);
        bool CreateTreatment(Treatment treatment);
        bool UpdateTreatment(Treatment treatment);
        bool DeleteTreatment(Treatment treatment);
        bool Save();
    }
}
