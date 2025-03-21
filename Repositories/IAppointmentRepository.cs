﻿using backendclinic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backendclinic.Repositories
{
    public interface IAppointmentRepository
    {
        Task<bool> BookAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<bool> UpdateAppointmentStatusAsync(int appointmentId, int status, string reason = null); // 🆕 Added reason
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
    }
}
