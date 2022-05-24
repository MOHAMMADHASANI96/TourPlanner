using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.PdfGenerator
{

    //--https://www.questpdf.com/documentation/getting-started.html#content-implementation
    public class ReportTemplate : IDocument
    {
        public PdfQuest Model { get; }

        public ReportTemplate(PdfQuest model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().BorderColor(Colors.Black).Element(ComposeHeader);
                    page.Content().Padding(10).Element(ComposeTable);
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }
        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
            container
                .Border(1)
                .Padding(20)
                .Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text($"Tour Name: {Model.TourItem.Name}").Style(titleStyle).Black();

                        column.Item().Text(text =>
                        {
                            text.Span("From: ").SemiBold();
                            text.Span($"{Model.TourItem.From}");
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("To: ").SemiBold();
                            text.Span($"{Model.TourItem.To}");
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Distance: ").SemiBold();
                            text.Span($"{Model.TourItem.Distance}");
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Transport Type: ").SemiBold();
                            text.Span($"{Model.TourItem.TransportTyp}");
                        });

                    });

                });
        }

        void ComposeTable(IContainer container)
        {
            container
            .Column(column =>
             {
                 column.Item().Column(column =>
                 {
                     column.Item().Image(Model.Image, ImageScaling.FitArea);
                 });

                 column
                 .Item()
                 .Padding(10)
                 .Table(table =>
                 {

                     // step 1
                     table.ColumnsDefinition(columns =>
                     {
                         columns.RelativeColumn(3);
                         columns.RelativeColumn();
                         columns.RelativeColumn();
                         columns.RelativeColumn();
                         columns.RelativeColumn();
                     });

                     // step 2
                     table.Header(header =>
                     {
                         header.Cell().Element(CellStyle).AlignLeft().Text("Date");
                         header.Cell().Element(CellStyle).AlignLeft().Text("Report");
                         header.Cell().Element(CellStyle).AlignLeft().Text("Difficulty");
                         header.Cell().Element(CellStyle).AlignLeft().Text("Total Time");
                         header.Cell().Element(CellStyle).AlignLeft().Text("Rating");

                         static IContainer CellStyle(IContainer container)
                         {
                             return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                         }
                     });

                     // step 3
                     foreach (var log in Model.TourLogs)
                     {
                         table.Cell().Element(CellStyle).Text(log.DateTime);
                         table.Cell().Element(CellStyle).AlignLeft().Text(log.Report);
                         table.Cell().Element(CellStyle).AlignLeft().Text(log.Difficulty);
                         table.Cell().Element(CellStyle).AlignLeft().Text(log.TotalTime);
                         table.Cell().Element(CellStyle).AlignLeft().Text(log.Rating);

                         static IContainer CellStyle(IContainer container)
                         {
                             return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                         }
                     }
                 });

            });
        }
    }
}
