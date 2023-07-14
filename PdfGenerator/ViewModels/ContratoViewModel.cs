namespace PdfGenerator.ViewModels
{
    public class ContratoViewModel
    {
        public string NomeCliente { get; set; }
        public string RgCliente { get; set; }
        public string CpfCliente { get; set; }
        public string EnderecoCliente { get; set; }
        public string DescricaoConsorcio { get; set; }
        public decimal ValorConsorcio { get; set; }
        public int Parcelas { get; set; }
        public decimal ValorParcela { get; set; }
        public int DuracaoConsorcio { get; set; }
        public string MesContemplacao { get; set; }
        public string LocalData { get; set; }

    }
}
