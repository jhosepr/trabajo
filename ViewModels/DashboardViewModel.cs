namespace practica.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalMedicamentos { get; set; }
        public int MedicamentosVencidos { get; set; }
        public int MedicamentosPorVencer { get; set; }
        public int ProductosBajoStock { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalEstantes { get; set; }

        // Data for Charts
        public List<string> CategoriasNombres { get; set; } = new();
        public List<int> CategoriasConteo { get; set; } = new();
    }
}
