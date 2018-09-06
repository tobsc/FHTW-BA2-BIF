using System;
using System.Collections.Generic;
using DinkToPdf;
using HwInf.Common.Models;
using HwInf.PDFService;
using HwInf.PDFService.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HwInf.Services.PdfService
{
    public class PDFCreator : IPdfService
    {
        private static BasicConverter _pdfConverter;
        private readonly TemplateService.TemplateService _templateService;


        public PDFCreator(IRazorViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _pdfConverter = new BasicConverter(new PdfTools());
            _templateService = new TemplateService.TemplateService(viewEngine, serviceProvider, tempDataProvider);
        }

        public byte[] GenerateContrtactPdf(Order order, List<Damage> damages)
        {
            var vm = new ContractViewModel();
            vm.Refresh(order, damages);
            string documentContent = _templateService.RenderTemplateAsync(
                "Templates/ContractTemplate", vm);

            var output = _pdfConverter.Convert(new HtmlToPdfDocument()
            {
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = documentContent,
                    }
                }
            });

            return output;
        }

        public byte[] GenerateContractReturnPdf(Order order, List<Damage> damages)
        {
            var vm = new ContractViewModel();
            vm.Refresh(order, damages);
            string documentContent = _templateService.RenderTemplateAsync(
                "Templates/ContractTemplate", vm);

            var output = _pdfConverter.Convert(new HtmlToPdfDocument()
            {
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = documentContent,
                    }
                }
            });

            return output;
        }
    }
}
