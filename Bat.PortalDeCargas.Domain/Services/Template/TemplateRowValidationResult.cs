using System.Collections.Generic;
using System.Linq;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateRowValidationResult
    {
        public TemplateRowValidationResult(int rowNumber)
        {
            RowNumber = rowNumber;
            CellValidationResults = new List<TemplateValidationCellResult>();
        }

        /// <summary>
        ///     Obtém a lista de resultados de validação das celulas da linha
        /// </summary>
        public IList<TemplateValidationCellResult> CellValidationResults { get; }

        /// <summary>
        ///     Obtém o número da linha
        /// </summary>
        public int RowNumber { get; }

        /// <summary>
        ///     Informa se a linha é válida, i.e., se não existem erros de validação nas células da linha
        /// </summary>
        public bool isValid => CellValidationResults.All(c => c.IsValid);

        /// <summary>
        ///     Obtém os erros da linha
        /// </summary>
        /// <returns></returns>
        public string GetErrors()
        {
            return string.Join("\n", CellValidationResults.Where(c => !c.IsValid).Select(c => c.ToString()));
        }
    }
}
