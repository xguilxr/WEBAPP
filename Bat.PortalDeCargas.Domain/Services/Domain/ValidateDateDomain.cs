using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public class ValidateDateDomain:ValidateDomain
    {
        const int yearLengh = 4;
        const int monthDayLengh = 2;

        public ValidateDateDomain(IStringLocalizer<DimensionTranslation> stringLocalizer):base(stringLocalizer)
        {

        }
      
        public override IList<string> IsValidDomain(DimensionDTO Dimension, string value)
        {
            var erros = new List<string>();
            DateTime parsedDate = new DateTime();

            if (IsCompleteDate(Dimension))
            {

                if (!DateTime.TryParseExact(value, Dimension.DimensionFormat, null,
                                   DateTimeStyles.None, out parsedDate))
                    erros.Add(this.stringLocalizer["DimensionInvalidDate"].Value);   
            }
            else
            {
                var number = 0;

                if (Dimension.DimensionFormat.Contains("yyyy"))
                {
                    var position = Dimension.DimensionFormat.IndexOf("y");


                    var year = this.ExtractField(position, value, yearLengh);

                    if (!int.TryParse(year, out number))
                        erros.Add(this.stringLocalizer["DimensionInvalidYear"].Value);
                    
                    
                }


                if (Dimension.DimensionFormat.Contains("MM"))
                {
                    var position = Dimension.DimensionFormat.IndexOf("M");
                    var month =ExtractField(position, value, monthDayLengh);

                    if (!int.TryParse(month, out number))
                        erros.Add(this.stringLocalizer["DimensionInvalidMonth"].Value);

                    if (number > 12 || number < 1)
                        erros.Add(this.stringLocalizer["DimensionInvalidMonth"].Value);
                }

                if (Dimension.DimensionFormat.Contains("dd"))
                {
                    var position = Dimension.DimensionFormat.IndexOf("d");
                    var day = ExtractField(position, value, monthDayLengh);

                    if (!int.TryParse(day, out number))
                        erros.Add(this.stringLocalizer["DimensionInvalidDay"].Value);

                    if (number > 31 || number < 1)
                        erros.Add(this.stringLocalizer["DimensionInvalidDay"].Value);
                }

                if (Dimension.DimensionFormat.Contains("/"))
                {
                    var positon = Dimension.DimensionFormat.IndexOf("/");
                    var separator = value.Substring(positon, 1);
                    
                    if (separator != "/")
                        erros.Add(this.stringLocalizer["DimensionInvalidDate"].Value);
                }

                if (Dimension.DimensionFormat.Contains("-"))
                {
                    var positon = Dimension.DimensionFormat.IndexOf("-");
                    var separator = value.Substring(positon, 1);

                    if (separator != "-")
                        erros.Add(this.stringLocalizer["DimensionInvalidDate"].Value);
                }

            }

            return erros;
         
        }

        private string ExtractField(int position, string value, int size)
        {
            var totalSize = position + size;
            var retorno = "";

            if (totalSize <= value.Length)
                retorno = value.Substring(position,size);

            return retorno;
        }

        private bool IsCompleteDate(DimensionDTO Dimension)
        {
            return Dimension.DimensionFormat.Contains("y") & Dimension.DimensionFormat.Contains("M") && Dimension.DimensionFormat.Contains("d");
        }

    }
}
