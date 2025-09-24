using System;
using System.Linq;
using ClinicaCitas.Data;
using ClinicaCitas.Models;

namespace ClinicaCitas.Data
{
    public static class SeedMedicos
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Eliminar todas las citas asociadas a médicos para evitar conflictos de clave foránea
            if (context.Citas.Any())
            {
                context.Citas.RemoveRange(context.Citas);
                context.SaveChanges();
            }
            // Eliminar todos los médicos existentes
            if (context.Medicos.Any())
            {
                context.Medicos.RemoveRange(context.Medicos);
                context.SaveChanges();
            }

            // Insertar médicos reales: 3 por especialidad
            var especialidades = context.Especialidades.ToList();
            // Cardiología
            var cardiologiaId = especialidades.FirstOrDefault(e => e.Nombre == "Cardiología")?.EspecialidadId;
            // Neurología
            var neurologiaId = especialidades.FirstOrDefault(e => e.Nombre == "Neurología")?.EspecialidadId;
            // Hepatología
            var hepatologiaId = especialidades.FirstOrDefault(e => e.Nombre == "Hepatología")?.EspecialidadId;
            // Pediatría
            var pediatriaId = especialidades.FirstOrDefault(e => e.Nombre == "Pediatría")?.EspecialidadId;
            // Oftalmología
            var oftalmologiaId = especialidades.FirstOrDefault(e => e.Nombre == "Oftalmología")?.EspecialidadId;

            var medicos = new List<Medico>();
            if (cardiologiaId != null)
            {
                medicos.AddRange(new[] {
                    new Medico { Nombre = "Ana", Apellido = "García", EspecialidadId = cardiologiaId.Value },
                    new Medico { Nombre = "Carlos", Apellido = "Mendoza", EspecialidadId = cardiologiaId.Value },
                    new Medico { Nombre = "Elena", Apellido = "Santos", EspecialidadId = cardiologiaId.Value }
                });
            }
            if (neurologiaId != null)
            {
                medicos.AddRange(new[] {
                    new Medico { Nombre = "Luis", Apellido = "Martínez", EspecialidadId = neurologiaId.Value },
                    new Medico { Nombre = "Patricia", Apellido = "Vega", EspecialidadId = neurologiaId.Value },
                    new Medico { Nombre = "Jorge", Apellido = "Silva", EspecialidadId = neurologiaId.Value }
                });
            }
            if (hepatologiaId != null)
            {
                medicos.AddRange(new[] {
                    new Medico { Nombre = "Marta", Apellido = "Fernández", EspecialidadId = hepatologiaId.Value },
                    new Medico { Nombre = "Raúl", Apellido = "Castro", EspecialidadId = hepatologiaId.Value },
                    new Medico { Nombre = "Lucía", Apellido = "Morales", EspecialidadId = hepatologiaId.Value }
                });
            }
            if (pediatriaId != null)
            {
                medicos.AddRange(new[] {
                    new Medico { Nombre = "Pedro", Apellido = "López", EspecialidadId = pediatriaId.Value },
                    new Medico { Nombre = "Gabriela", Apellido = "Ríos", EspecialidadId = pediatriaId.Value },
                    new Medico { Nombre = "Andrés", Apellido = "Navarro", EspecialidadId = pediatriaId.Value }
                });
            }
            if (oftalmologiaId != null)
            {
                medicos.AddRange(new[] {
                    new Medico { Nombre = "Sofía", Apellido = "Ruiz", EspecialidadId = oftalmologiaId.Value },
                    new Medico { Nombre = "Manuel", Apellido = "Paredes", EspecialidadId = oftalmologiaId.Value },
                    new Medico { Nombre = "Valeria", Apellido = "Delgado", EspecialidadId = oftalmologiaId.Value }
                });
            }
            if (medicos.Count > 0)
            {
                context.Medicos.AddRange(medicos);
                context.SaveChanges();
            }
        }
    }
}
