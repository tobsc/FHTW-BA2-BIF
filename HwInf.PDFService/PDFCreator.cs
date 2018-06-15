using System;
using System.Collections.Generic;
using DinkToPdf;
using HwInf.BusinessLogic.Models;
using HwInf.PDFService.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HwInf.PDFService
{
    public class PDFCreator
    {
        private static BasicConverter _pdfConverter;
        private readonly TemplateService _templateService;


        public PDFCreator(IRazorViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _pdfConverter = new BasicConverter(new PdfTools());
            _templateService = new TemplateService(viewEngine, serviceProvider, tempDataProvider);
        }

        public byte[] GeneratePdf(Order order, List<Damage> damages)
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
