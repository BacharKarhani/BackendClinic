using backendclinic.Data;
using backendclinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backendclinic.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await _context.Appointments.Include(a => a.User).ToListAsync();
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, int status)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return false;

            appointment.Status = status;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

    }
}
