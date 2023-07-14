using Microsoft.AspNetCore.Mvc;
using PdfGenerator.Extensions;
using PdfGenerator.Services.Meta;
using PdfGenerator.ViewModels;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Threading.Tasks;

namespace PdfGenerator.Controllers
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
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = PuppeteerExtensions.ExecutablePath
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });
            return File(pdfContent, "application/pdf", $"Invoice-{model.NomeCliente}.pdf");
        }
    }
}