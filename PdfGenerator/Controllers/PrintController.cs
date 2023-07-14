using Microsoft.AspNetCore.Mvc;
using PdfGenerator.Services.Meta;
using PdfGenerator.ViewModels;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PdfGeneratorRenanzinho.Controllers
{
    [Route("/api/print")]
    public class PrintController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public PrintController(ITemplateService templateService)
        {
            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
        }

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var model = new ContratoViewModel
            {
                NomeCliente = "João da Silva",
                RgCliente = "12345678",
                CpfCliente = "123.456.789-00",
                EnderecoCliente = "Rua Teste, 123",
                DescricaoConsorcio = "Veículo",
                ValorConsorcio = 3000,
                Parcelas = 48,
                ValorParcela = 625,
                DuracaoConsorcio = 48,
                MesContemplacao = "Dezembro 2026",
                LocalData = "São Paulo, 13 de Julho de 2023"
            };

            var html = await _templateService.RenderAsync("Templates/Contrato", model);

            // Gerar o PDF

            var document = new PdfDocument();

            // Gerando PDF a partir de HTML
            TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.AddPdfPages(document, html, PageSize.A4);

            byte[] response = null;

            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }

            string Filename = "Invoice-" + model.NomeCliente + ".pdf";

            // Retorna o PDF como um arquivo
            return File(response, "application/pdf", Filename);

        }
    }
}